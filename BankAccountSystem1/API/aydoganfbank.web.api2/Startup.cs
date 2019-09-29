using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aydoganfbank.web.api2.Middlewares;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Context.IoC;
using AydoganFBank.Context.IoC.Lifecycle;
using AydoganFBank.Context.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

namespace aydoganfbank.web.api2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                var oldFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
                opt.OutputFormatters.Remove(oldFormatter);

                oldFormatter.PublicSerializerSettings.ContractResolver = new CamelCaseContractResolver();

                var newFormatter = new CustomJsonOutputFormatter(oldFormatter.PublicSerializerSettings, ArrayPool<char>.Shared);
                opt.OutputFormatters.Add(newFormatter);

                opt.Filters.Add(typeof(AuthenticationRequiredFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            return ConfigureIoC(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseMvc();
        }



        public IServiceProvider ConfigureIoC(IServiceCollection services)
        {
            var container = new Container(_ =>
            {
                _.For<ICoreContextConfigurer>().Use<CoreContextConfigurer>()
                    .Ctor<Action<CoreContextConfigurer>>().Is(c =>
                    {
                        c.DBConnectionString(Configuration.GetConnectionString("AydoganFBankDatabase"));
                        c.LogFileDirectory(@"C:\Users\Faruk\Documents\GitHub\BankAccountSystem-1\BankAccountSystem1\logs");
                    });

                _.Scan(s =>
                {
                    s.Assembly("AydoganFBank.Context");
                    s.Assembly("AydoganFBank.Database");
                    s.Assembly("AydoganFBank.AccountManagement");
                    s.Assembly("AydoganFBank.Service");
                    s.WithDefaultConventions();
                    s.Convention<ObjectLifecycleRegistrationConvention>();
                });

                _.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }

        public class ObjectLifecycleRegistrationConvention : IRegistrationConvention
        {
            public void ScanTypes(TypeSet types, Registry registry)
            {
                types.FindTypes(TypeClassification.Concretes | TypeClassification.Closed).ToList()
                    .ForEach(type => 
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
                    });
            }
        }
    }
}
