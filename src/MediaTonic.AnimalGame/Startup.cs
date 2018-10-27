using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using MediaTonic.AnimalGame.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace MediaTonic.AnimalGame
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var assembly = typeof(Program).GetTypeInfo().Assembly;

            services.AddAutoMapper(assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", 
                    new Info {
                    Title = "MediaTonic Animal Game API",
                    Description = "To Start retrieve, use User method /api/User/GetByUserName with parameter of mn123 to bring up user with animals and relevant Id's",
                    Version = "v1"
                });

                c.DescribeAllEnumsAsStrings();
            });

            SeedData();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MediaTonic Animal Game API");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void SeedData()
        {
            var user1 = User.CreateUser("Michael Nee", "mn123");
            var user2 = User.CreateUser("Michael Nee", "mn1234");
            var user3 = User.CreateUser("Michael Nee", "mn12345");
            var user4 = User.CreateUser("Michael Nee", "mn12345");
            var user5 = User.CreateUser("Michael Nee", "mn123456");

            var animal = Animal.CreateAnimal(API.Enums.AnimalType.Dog, "Joey", user1);

        }
    }
}
