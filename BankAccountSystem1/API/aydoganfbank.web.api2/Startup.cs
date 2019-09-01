using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aydoganfbank.web.api2.Middlewares;
using AydoganFBank.Context.IoC;
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
                        c.SetConnStr(Configuration.GetConnectionString("AydoganFBankDatabase"));
                    }).Singleton();

                _.Scan(s =>
                {
                    s.Assembly("AydoganFBank.Context");
                    s.Assembly("AydoganFBank.AccountManagement");
                    s.Assembly("AydoganFBank.Service");
                    s.WithDefaultConventions();
                });

                //_.Scan(s =>
                //{
                //    s.Assembly("AydoganFBank.Service");
                //    s.WithDefaultConventions();
                //});


                _.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}
