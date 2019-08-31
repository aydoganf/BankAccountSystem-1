using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Managers;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Context.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StructureMap;

namespace aydoganfbank.web.api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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
                //_.Scan(s =>
                //{
                //    s.Assembly("AydoganFBank.AccountManagement");
                //    //s.ConnectImplementationsToTypesClosing(typeof(IDomainEntityBuilder<,>));
                //    //s.ConnectImplementationsToTypesClosing(typeof(IDbEntityMapper<,>));
                //    s.WithDefaultConventions();
                //});

                _.For<IAccountManager>().Use<AccountManager>();
                _.For<IPersonManager>().Use<PersonManager>();
                _.For<ICompanyManager>().Use<CompanyManager>();
                _.For<ICreditCardManager>().Use<CreditCardManager>();
                _.For<IPersonRepository>().Use<PersonRepository>();

                _.Scan(s =>
                {
                    s.Assembly("AydoganFBank.Context");
                    s.Assembly("AydoganFBank.AccountManagement");
                    //s.WithDefaultConventions();
                    s.IncludeNamespace("AydoganFBank.Context");
                    s.ConnectImplementationsToTypesClosing(typeof(IDomainEntityBuilder<,>));
                    s.ConnectImplementationsToTypesClosing(typeof(IDbEntityMapper<,>));
                    s.WithDefaultConventions();                    
                });

                _.Scan(s => 
                {
                    s.Assembly("AydoganFBank.Service");
                    s.WithDefaultConventions();
                });

                
                _.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}
