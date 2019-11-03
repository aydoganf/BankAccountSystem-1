using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDispacher
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemlyTypes = System.Reflection.Assembly.Load("AydoganFBank.AccountManagement").GetTypes();
            List<Type> repositoryTypes = new List<Type>();
            List<Type> managerTypes = new List<Type>();
            List<Type> dataTypes = new List<Type>();

            foreach (var type in assemlyTypes)
            {
                if (type.IsInterface)
                {
                    if (type.Name.EndsWith("Repository"))
                    {
                        repositoryTypes.Add(type);
                        continue;
                    }

                    if (type.Name.EndsWith("Manager"))
                    {
                        managerTypes.Add(type);
                        continue;
                    }

                    dataTypes.Add(type);
                }

                if (type.IsEnum)
                {
                    dataTypes.Add(type);
                }
            }

            Console.WriteLine("Repository Types:");
            foreach (var type in repositoryTypes)
                Console.WriteLine(type);

            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("------------------------------------------------------------");

            Console.WriteLine("Manager Types:");
            foreach (var type in managerTypes)
                Console.WriteLine(type);

            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("------------------------------------------------------------");

            Console.WriteLine("Data Types:");
            foreach (var type in dataTypes)
                Console.WriteLine(type);

            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("------------------------------------------------------------");

            Console.WriteLine();

            foreach (var managerType in managerTypes)
            {
                Console.WriteLine(managerType.Name);

                foreach (var methodInfo in managerType.GetMethods())
                {
                    StringBuilder sb = new StringBuilder();

                    var mReturnType = methodInfo.ReturnType;
                    string mReturnTypeStr = mReturnType.Name;

                    if (mReturnType.GetGenericArguments().Length != 0)
                    {
                        var genericType = mReturnType.GetGenericArguments()[0];
                        mReturnTypeStr = string.Format("List<{0}>", genericType.Name);
                    }

                    var mParams = methodInfo.GetParameters();

                    sb.Append("public ");
                    sb.Append(mReturnTypeStr);
                    sb.Append(" ");
                    sb.Append(methodInfo.Name);
                    sb.Append("(");

                    for (int i = 0; i < mParams.Length; i++)
                    {
                        sb.Append(mParams[i].ParameterType.Name);
                        sb.Append(" ");
                        sb.Append(mParams[i].Name);
                        if (i != mParams.Length - 1)
                            sb.Append(", ");
                    }

                    sb.Append(");");
                    Console.WriteLine("\t{0}", sb.ToString());
                }
            }


            var dataIncludes = dataTypes.Select(dt => dt.Namespace).Distinct();

            StringBuilder dataClassBuilder = new StringBuilder();
            StringBuilder serviceDataBuilder = new StringBuilder();
            StringBuilder serviceApiBuilder = new StringBuilder();
            StringBuilder serviceBuilder = new StringBuilder();
            StringBuilder serviceContextApiBuilder = new StringBuilder();


            #region Data classes namespace

            dataClassBuilder.Append("namespace AydoganFBank.Service.Dispatcher.Data\n{");

            NamespaceIncluder(dataClassBuilder, new string[] { "System" });
            NamespaceIncluder(dataClassBuilder, dataIncludes.ToArray());
            dataClassBuilder.Append("\n");

            foreach (var dataType in dataTypes.Where(dt => dt.IsEnum == false))
            {
                var customTypeName = dataType.Name.Substring(1);
                dataClassBuilder.Append("\n\tpublic class ");
                dataClassBuilder.Append(customTypeName);
                dataClassBuilder.Append("\n\t{");

                foreach (var property in dataType.GetProperties())
                {
                    PropertyBuilder(property, dataClassBuilder);
                }

                dataClassBuilder.Append("\n\t}\n\n");
            }

            dataClassBuilder.Append("\n}\n");

            #endregion

            #region Service Data Builder namespace

            serviceDataBuilder.Append("\nnamespace AydoganFBank.Service.Dispatcher.Data.Builder\n{");

            NamespaceIncluder(serviceDataBuilder, new string[]
            {
                "System",
                "System.Collections.Generic",
                "AydoganFBank.Service.Dispatcher.Data"
            });
            NamespaceIncluder(serviceDataBuilder, dataIncludes.ToArray());
            serviceDataBuilder.Append("\n");

            serviceDataBuilder.Append("\n\tpublic class ServiceDataBuilder");
            serviceDataBuilder.Append("\n\t{");

            foreach (var dataType in dataTypes.Where(dt => dt.IsEnum == false))
            {
                var customTypeName = dataType.Name.Substring(1);
                DataBuilderGenerator(dataType, customTypeName, serviceDataBuilder);
            }

            serviceDataBuilder.Append("\n\t}");
            serviceDataBuilder.Append("\n}");
            #endregion

            #region Service API namespace
            serviceApiBuilder.Append("\n\nnamespace AydoganFBank.Service.Dispatcher.Api");
            serviceApiBuilder.Append("\n{");

            NamespaceIncluder(serviceApiBuilder, new string[]
            {
                "System",
                "AydoganFBank.Service.Dispatcher.Data",
                "System.Collections.Generic"
            });

            NamespaceIncluder(serviceApiBuilder, dataTypes.Select(dt => dt.Namespace).Distinct().ToArray());

            serviceApiBuilder.Append("\n");

            foreach (var managerType in managerTypes)
            {
                string apiName = string.Format("{0}Service", managerType.Name);

                InterfaceDefiner(apiName, serviceApiBuilder);
                ApiInterfaceBuilder(managerType, serviceApiBuilder);
            }

            serviceApiBuilder.Append("\n}");
            #endregion

            #region Service namespace
            serviceBuilder.Append("\n\nnamespace AydoganFBank.Service.Dispatcher.Services");
            serviceBuilder.Append("\n{");

            NamespaceIncluder(serviceBuilder, new string[]
            {
                "System",
                "System.Collections.Generic",
                "AydoganFBank.Service.Dispatcher.Data",
                "AydoganFBank.Service.Dispatcher.Data.Builder",
                "AydoganFBank.Service.Dispatcher.Api"
            });
            NamespaceIncluder(serviceBuilder, dataTypes.Select(dt => dt.Namespace).Distinct().ToArray());
            NamespaceIncluder(serviceBuilder, managerTypes.Select(mt => mt.Namespace).Distinct().ToArray());

            serviceBuilder.Append("\n");

            foreach (var managerType in managerTypes)
            {
                string className = string.Format("{0}Service", managerType.Name.Substring(1));
                string manager = managerType.Name.Substring(1).ToLowerFirstChar();
                ClassDefiner(className, serviceBuilder, string.Format("I{0}", className));
                serviceBuilder.Append("\n\t{");

                ClassIoCFiledsGenerator(serviceBuilder, new ParameterDefiniton[]
                {
                    new ParameterDefiniton()
                    {
                        ParameterType = managerType.Name,
                        ParameterName = managerType.Name.Substring(1).ToLowerFirstChar()
                    },
                    new ParameterDefiniton()
                    {
                        ParameterType = "ServiceDataBuilder",
                        ParameterName = "serviceDataBuilder"
                    }
                });

                ConstructorDefiner(className, serviceBuilder, new ParameterDefiniton[]
                {
                    new ParameterDefiniton()
                    {
                        ParameterType = managerType.Name,
                        ParameterName = managerType.Name.Substring(1).ToLowerFirstChar()
                    },
                    new ParameterDefiniton()
                    {
                        ParameterType = "ServiceDataBuilder",
                        ParameterName = "serviceDataBuilder"
                    }
                });

                foreach (var method in managerType.GetMethods())
                {
                    ServiceMethodGenerator(method, manager, serviceBuilder);
                }

                serviceBuilder.Append("\n\t}");
            }

            serviceBuilder.Append("\n}\n");
            #endregion

            #region Service Context API namespace
            serviceContextApiBuilder.Append("\nnamespace AydoganFBank.Service.Dispatcher.Context");
            serviceContextApiBuilder.Append("\n{");

            NamespaceIncluder(serviceContextApiBuilder, new string[]
            {
                "AydoganFBank.Service.Dispatcher.Api"
            });

            serviceContextApiBuilder.Append("\n");

            InterfaceDefiner("IServiceContext", serviceContextApiBuilder);
            serviceContextApiBuilder.Append("\n\t{");

            managerTypes.ForEach(mt =>
            {
                serviceContextApiBuilder.Append("\n\t\t");
                serviceContextApiBuilder.Append(string.Format("{0}Service", mt.Name));
                serviceContextApiBuilder.Append(" ");
                serviceContextApiBuilder.Append(string.Format("{0}Service", mt.Name.Substring(1)));
                serviceContextApiBuilder.Append(" { get; }");
            });

            serviceContextApiBuilder.Append("\n\t}\n");
            // interface ends here

            ClassDefiner("ServiceContext", serviceContextApiBuilder, "IServiceContext");
            serviceContextApiBuilder.Append("\n\t{");

            ClassInterfaceImplementation(serviceContextApiBuilder, managerTypes.Select(mt => new ParameterDefiniton()
            {
                ParameterType = string.Format("{0}Service", mt.Name),
                ParameterName = string.Format("{0}Service", mt.Name.Substring(1))
            }).ToArray());

            ConstructorDefiner("ServiceContext", serviceContextApiBuilder, managerTypes.Select(mt => new ParameterDefiniton()
            {
                ParameterType = string.Format("{0}Service", mt.Name),
                ParameterName = string.Format("{0}Service", mt.Name.Substring(1))
            }).ToArray());

            serviceContextApiBuilder.Append("\n\t}");

            serviceContextApiBuilder.Append("\n}");
            #endregion


            string outputPath = "";
            switch (Environment.MachineName)
            {
                // home
                case "DESKTOP-FL7VPQG":
                    outputPath = @"C:\Users\Faruk\source\repos\BankAccountSystem-1\BankAccountSystem1\Service\ServiceDispacher\output\test.cs";
                    break;
                default:
                    outputPath = @"C:\Users\Faruk\Documents\GitHub\BankAccountSystem-1\BankAccountSystem1\Service\ServiceDispacher\output\test.cs";
                    break;
            }

            File.WriteAllText(outputPath, dataClassBuilder.ToString());
            File.AppendAllText(outputPath, serviceDataBuilder.ToString());
            File.AppendAllText(outputPath, serviceApiBuilder.ToString());
            File.AppendAllText(outputPath, serviceBuilder.ToString());
            File.AppendAllText(outputPath, serviceContextApiBuilder.ToString());

            Console.ReadKey();
        }

        public static void ClassInterfaceImplementation(StringBuilder sb, params ParameterDefiniton[] members)
        {
            for (int i = 0; i < members.Length; i++)
            {
                sb.Append("\n\t\tpublic ");
                sb.Append(members[i].ParameterType);
                sb.Append(" ");
                sb.Append(members[i].ParameterName);
                sb.Append(" { get; private set; }");
            }
            sb.Append("\n\n");
        }

        public static void ServiceMethodGenerator(MethodInfo method, string manager, StringBuilder sb)
        {
            var customReturnType = GetCustomReturnType(method.ReturnType);
            var customReturnTypeName = GetCustomReturnTypeName(method.ReturnType);
            var mParams = method.GetParameters();

            if (mParams.Any(p => p.ParameterType.IsInterface))
                return;

            MethodDefiner(sb, customReturnType, method.Name,
                mParams.Select(p => new ParameterDefiniton()
                {
                    ParameterType = p.ParameterType.Name,
                    ParameterName = p.Name
                }).ToArray());
            sb.Append("\n\t\t{");

            bool isMethodReturnTypeList = method.IsReturnTypeList();

            sb.Append("\n\t\t\tvar result = ");
            sb.Append(manager);
            sb.Append(".");
            sb.Append(method.Name);
            sb.Append("(");
            for (int i = 0; i < mParams.Length; i++)
            {
                sb.Append(mParams[i].Name);
                if (i != mParams.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(");");

            if (isMethodReturnTypeList)
            {
                sb.Append("\n\t\t\treturn serviceDataBuilder.");
                sb.Append(customReturnTypeName);
                sb.Append("ListBuilder(result);");
            }
            else
            {
                if (customReturnTypeName == "Object")
                {
                    sb.Append("\n\t\t\treturn result;");
                }
                else
                {
                    sb.Append("\n\t\t\treturn serviceDataBuilder.");
                    sb.Append(customReturnTypeName);
                    sb.Append("Builder(result);");
                }
            }

            sb.Append("\n\t\t}\n");
        }

        public static void ClassIoCFiledsGenerator(StringBuilder sb, params ParameterDefiniton[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                sb.Append("\n\t\tprivate readonly ");
                sb.Append(fields[i].ParameterType);
                sb.Append(" ");
                sb.Append(fields[i].ParameterName);
                sb.Append(";");
            }
            sb.Append("\n\n");
        }

        public static void ConstructorDefiner(string className, StringBuilder sb, params ParameterDefiniton[] ctorParams)
        {
            sb.Append("\t\tpublic ");
            sb.Append(className);
            sb.Append("(");
            for (int i = 0; i < ctorParams.Length; i++)
            {
                sb.Append(ctorParams[i].ParameterType);
                sb.Append(" ");
                sb.Append(ctorParams[i].ParameterName);
                if (i != ctorParams.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(")\n\t\t{");
            for (int i = 0; i < ctorParams.Length; i++)
            {
                sb.Append("\n\t\t\tthis.");
                sb.Append(ctorParams[i].ParameterName);
                sb.Append(" = ");
                sb.Append(ctorParams[i].ParameterName);
                sb.Append(";");
            }
            sb.Append("\n\t\t}\n");
        }

        public static void ClassDefiner(string name, StringBuilder sb, params string[] interfaces)
        {
            sb.Append("\n\tpublic class ");
            sb.Append(name);
            if (interfaces.Length != 0)
            {
                for (int i = 0; i < interfaces.Length; i++)
                {
                    if (i == 0)
                        sb.Append(" : ");

                    sb.Append(interfaces[i]);

                    if (i != interfaces.Length - 1)
                        sb.Append(", ");
                }
            }
        }

        public static void InterfaceDefiner(string name, StringBuilder sb)
        {
            sb.Append("\n\tpublic interface ");
            sb.Append(name);
        }

        public static string GetCustomReturnType(Type returnType)
        {
            var customReturnType = returnType.Name;
            if (returnType.IsInterface)
            {
                customReturnType = returnType.Name.Substring(1);
            }
            else if (returnType.GetGenericArguments().Length != 0)
            {
                var genericArg = returnType.GetGenericArguments()[0];
                customReturnType = string.Format("List<{0}>", returnType.GetGenericArguments()[0].Name);
                if (genericArg.IsInterface)
                {
                    customReturnType = string.Format("List<{0}>", returnType.GetGenericArguments()[0].Name.Substring(1));
                }
            }
            return customReturnType;
        }

        public static string GetCustomReturnTypeName(Type returnType)
        {
            if (returnType.IsInterface)
            {
                return returnType.Name.Substring(1);
            }
            else if (returnType.GetGenericArguments().Length != 0)
            {
                return returnType.GetGenericArguments()[0].Name.Substring(1);
            }

            return returnType.Name;
        }

        public static void ApiInterfaceBuilder(Type type, StringBuilder sb)
        {
            sb.Append("\n\t{");
            foreach (var method in type.GetMethods())
            {
                var returnType = method.ReturnType;
                var mParams = method.GetParameters();
                if (mParams.Any(p => p.ParameterType.IsInterface))
                {
                    continue;
                }
                var customReturnType = GetCustomReturnType(returnType);

                sb.Append("\n\t\t");
                sb.Append(customReturnType);
                sb.Append(" ");
                sb.Append(method.Name);
                sb.Append("(");
                for (int i = 0; i < mParams.Length; i++)
                {
                    sb.Append(mParams[i].ParameterType.Name);
                    sb.Append(" ");
                    sb.Append(mParams[i].Name);
                    if (i != mParams.Length - 1)
                        sb.Append(", ");
                }
                sb.Append(");\n");
            }
            sb.Append("\n\t}\n");
        }

        public static void NamespaceIncluder(StringBuilder sb, params string[] namespaces)
        {
            foreach (var ns in namespaces)
            {
                sb.Append("\n\tusing ");
                sb.Append(ns);
                sb.Append(";");
            }
        }

        public static void PropertyBuilder(PropertyInfo propertyInfo, StringBuilder sb)
        {
            string propertyType = propertyInfo.PropertyType.Name;
            if (propertyInfo.PropertyType.IsInterface)
            {
                propertyType = propertyType.Substring(1);
            }

            sb.Append("\n\t\tpublic ");
            sb.Append(propertyType);
            sb.Append(" ");
            sb.Append(propertyInfo.Name);
            sb.Append(" { get; set; }");
        }

        public static void MethodDefiner(StringBuilder sb, string typeOut, string methodName, ParameterDefiniton[] parameters)
        {
            sb.Append("\n\t\tpublic ");
            sb.Append(typeOut);
            sb.Append(" ");
            sb.Append(methodName);
            sb.Append("(");

            for (int i = 0; i < parameters.Length; i++)
            {
                sb.Append(parameters[i].ParameterType);
                sb.Append(" ");
                sb.Append(parameters[i].ParameterName);
                if (i != parameters.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(")");
        }

        public static void BuilderMethodBodyGenerator(Type typeIn, string typeOut, string itemParameterName, StringBuilder sb)
        {
            sb.Append("\n\t\t{");

            // null check
            sb.Append("\n\t\t\tif (");
            sb.Append(itemParameterName);
            sb.Append(" == null) return null;");

            sb.Append("\n\t\t\treturn new ");
            sb.Append(typeOut);
            sb.Append("()");
            sb.Append("\n\t\t\t{");

            // build object and return
            foreach (var property in typeIn.GetProperties())
            {
                if (property.PropertyType.IsInterface)
                {
                    var customType = property.PropertyType.Name.Substring(1);
                    sb.Append("\n\t\t\t\t");
                    sb.Append(property.Name);
                    sb.Append(" = ");
                    sb.Append(string.Format("{0}Builder({1}.{2}),", customType, itemParameterName, property.Name));
                }
                else
                {
                    sb.Append("\n\t\t\t\t");
                    sb.Append(property.Name);
                    sb.Append(" = ");
                    sb.Append(itemParameterName);
                    sb.Append(".");
                    sb.Append(property.Name);
                    sb.Append(",");
                }
            }

            sb.Append("\n\t\t\t};");

            sb.Append("\n\t\t}");
            sb.Append("\n");
        }

        public static void BuilderListMethodBodyGenerator(string typeOut, string listParameterName, string itemMethodName, StringBuilder sb)
        {
            string returnObj = string.Format("List<{0}> ret = new List<{0}>();", typeOut);
            sb.Append("\n\t\t{");

            sb.Append("\n\t\t\t");
            sb.Append(returnObj);

            sb.Append(string.Format("\n\t\t\tforeach (var item in {0})", listParameterName));
            sb.Append("\n\t\t\t\tret.Add(");
            sb.Append(itemMethodName);
            sb.Append("(item));");

            sb.Append("\n\t\t\treturn ret;");

            sb.Append("\n\t\t}");
            sb.Append("\n");
        }

        public static void DataBuilderGenerator(Type typeIn, string typeOut, StringBuilder sb)
        {
            #region for item
            string itemMethodName = string.Format("{0}Builder", typeOut);
            string itemParameterName = typeIn.Name.ToLower();
            MethodDefiner(sb, typeOut, itemMethodName, new ParameterDefiniton[]
            {
                new ParameterDefiniton()
                {
                    ParameterName = itemParameterName,
                    ParameterType = typeIn.Name
                }
            });
            BuilderMethodBodyGenerator(typeIn, typeOut, itemParameterName, sb);
            #endregion

            #region for list
            string listMethodName = string.Format("{0}ListBuilder", typeOut);
            string listParameterName = string.Format("{0}s", typeIn.Name.ToLower());

            MethodDefiner(sb, string.Format("List<{0}>", typeOut), listMethodName, new ParameterDefiniton[]
            {
                new ParameterDefiniton()
                {
                    ParameterType = string.Format("List<{0}>", typeIn.Name),
                    ParameterName = listParameterName
                }
            });
            BuilderListMethodBodyGenerator(typeOut, listParameterName, itemMethodName, sb);
            #endregion
        }

    }

    public class ParameterDefiniton
    {
        public string ParameterType { get; set; }
        public string ParameterName { get; set; }
    }

    public static class Extentions
    {
        public static string ToLowerFirstChar(this string input)
        {
            string newString = input;
            if (!String.IsNullOrEmpty(newString) && Char.IsUpper(newString[0]))
                newString = Char.ToLower(newString[0]) + newString.Substring(1);
            return newString;
        }

        public static bool IsReturnTypeList(this MethodInfo methodInfo)
        {
            return methodInfo.ReturnType.GetGenericArguments().Length != 0;
        }
    }
}
