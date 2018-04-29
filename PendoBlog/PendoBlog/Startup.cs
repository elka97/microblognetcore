using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PendoBlog.Models;

namespace PendoBlog
{
    using Swashbuckle.AspNetCore.Swagger;
    //  using PendoBlog.Models;

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
           services.AddMvc();

            //Scaffold-DbContext "Server=ELINAR-LAP-IL;Initial Catalog=MicroBlogPendo;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
            var connection = @"Server=ELINAR-LAP-IL;Initial Catalog=MicroBlogPendo;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            services.AddDbContext<MicroBlogPendoContext>(options => options.UseSqlServer(connection));

            services.AddMvc();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info {
                    Version = "v1",
                    Title = "Pendo micro blog API",
                    Description = "Pendo micro blog API with ASP.NET Core 2.0 Web API",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "elina roytman", Email = "elka97@hotmail.com" }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pendo micro blog API V1");
            });

            
        }

    }
}
