using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FileUpload
{
    public class Startup
    {
        static readonly string AllowOrigins = "AllowAll";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureFileProvider();

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Formatting = Formatting.Indented;
            })
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(options =>
            {
                options.AddPolicy(AllowOrigins, builder =>
                {
                    builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()
                            .AllowCredentials();
                });
            });

            //services.AddDbContext<HotelDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("HotelAPI"))
            //);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                { Title = "FileUpload", Version = "v1" });
            });
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
            app.UseSwaggerUI(c => {
                //c.DefaultModelRendering(ModelRendering.Example);
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelListingAPI");
                //c.RoutePrefix = string.Empty;
                string swaggerJsonPath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonPath}/swagger/v1/swagger.json", "HotelListingAPI");
            });

            app.UseCors("AllowOrigins");

            app.UseHttpsRedirection();
            app.UseMvc();

           //app.UseMvc(routes =>
           // {
           //     routes.MapRoute(
           //     name: "default",
           //     template: "{Controller}/{action=Index}/{id?}"
           //     );
           // });
        }
    }
}
