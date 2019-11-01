// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AccountApp.DependencyResolution {
    using System;
    using System.Linq;
    using AydoganFBank.Context.IoC;
    using AydoganFBank.Context.IoC.Lifecycle;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry() {
            For<ICoreContextConfigurer>().Use<CoreContextConfigurer>()
                    .Ctor<Action<CoreContextConfigurer>>().Is(c =>
                    {
                        c.DBConnectionString("metadata=res://*/Model.csdl|res://*/Model.ssdl|res://*/Model.msl;provider=System.Data.SqlClient;provider connection string='data source=.\\;initial catalog=AydoganFBank;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework'");
                        c.LogFileDirectory(@"C:\Users\Faruk\Documents\GitHub\BankAccountSystem-1\BankAccountSystem1\logs");
                    });

            Scan(
                scan => {
                    scan.Assembly("AydoganFBank.Context");
                    scan.Assembly("AydoganFBank.Database");
                    scan.Assembly("AydoganFBank.AccountManagement");
                    scan.Assembly("AydoganFBank.Service");
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.Convention<ObjectLifecycleRegistrationConvention>();
					scan.With(new ControllerConvention());
                });
            //For<IExample>().Use<Example>();
        }

        #endregion
    }

    public class ObjectLifecycleRegistrationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsClass)
            {
                if (type.GetInterfaces().Any(@interface => @interface.Name == typeof(ITransientObject).Name))
                {
                    registry.For(type).LifecycleIs(StructureMap.Pipeline.Lifecycles.Transient);
                }
                else if (type.GetInterfaces().Any(@interface => @interface.Name == typeof(ISingletonObject).Name))
                {
                    registry.For(type).LifecycleIs(StructureMap.Pipeline.Lifecycles.Singleton);
                }
                else
                {
                    registry.For(type).LifecycleIs(StructureMap.Pipeline.Lifecycles.Unique);
                }
            }
        }
    }
}