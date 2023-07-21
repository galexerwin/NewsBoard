using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;
using System;
using NetBoard.Security;

namespace NetBoard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // config bundle
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // get settings 
            Configuration.GetSection("AppSettings").Bind(AppSettings.Default);

            services.AddRazorPages().AddNewtonsoftJson();
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
            // add application insights
            //services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
            // add sessions handler
            services.AddSession(options =>
            {
                options.Cookie.Name = ".NewsBoard.Session";
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true; // check me
                options.Cookie.IsEssential = true;
            });
            // for session services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // logging services
            services.AddLogging(options => {
                options.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Warning);
                options.AddFilter("Microsoft", LogLevel.Information);
                options.AddConsole();
                options.AddDebug();
                options.SetMinimumLevel(LogLevel.Information);
                options.AddApplicationInsights(Configuration["ApplicationInsights:InstrumentationKey"]);
            });
            // configure Azure AD services
            //services.AddOptions();
            //services.AddMicrosoftIdentityWebAppAuthentication(Configuration, "Authentication:AzureAdB2C")
            //        .EnableTokenAcquisitionToCallDownstreamApi(new string[] { Configuration["WebApp:WebAppScope"] })
            //        .AddInMemoryTokenCaches();

            //services.AddControllersWithViews(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //}).AddMicrosoftIdentityUI();

            //services.AddRazorPages();






            //services.AddInMemoryTokenCaches();
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddAzureAdB2C(options => Configuration.Bind("Authentication:AzureAdB2C", options))
            .AddCookie();
            // 
            services.AddControllersWithViews();
            // Add framework services.
            services.AddMvc();

            // configure appsettings section AzureAdB2C, into IOptions
            //services.AddOptions();
            //services.Configure<OpenIdConnectOptions>(Configuration.GetSection("Authentication:AzureAdB2C"));
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                //_logger.LogInformation("Configuring for Development");
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
                app.UseRouteDebugger();
#pragma warning disable CS0618 // Type or member is obsolete
                WebpackDevMiddleware.UseWebpackDevMiddleware(app, new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
#pragma warning restore CS0618 // Type or member is obsolete

                
            }
            else
            {

                //_logger.LogInformation("Configuring for Production");

                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }
            // use https only
            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                // api routes
                endpoints.MapDefaultControllerRoute();
                // default route
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapFallbackToController("Index", "Home");
            });
            /*
             loggerFactory.AddConsole();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();            
             app.UseBrowserLink();
             */
        }
    }
}