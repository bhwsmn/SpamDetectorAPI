using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpamDetector.Services;

namespace SpamDetector
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
            services.AddControllers(options =>
                {
                    // Return HTTP 406 Not Acceptable if Accept header is anything beside application/json or application/xml
                    options.ReturnHttpNotAcceptable = true;

                    // Fixes the routing issue for async controller methods, when using CreatedAtAction() 
                    options.SuppressAsyncSuffixInActionNames = false;
                }).AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(options => { options.SuppressMapClientErrors = true; });
            services.AddCors();
            services.AddSingleton<SpamDetectorService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}