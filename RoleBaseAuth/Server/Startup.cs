namespace RoleBaseAuth.Server
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using RoleBaseAuth.Server.Data;
    using RoleBaseAuth.Server.Interfaces;
    using RoleBaseAuth.Server.Models;
    using RoleBaseAuth.Shared;

    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AdditionalUserClaimsPrincipalFactory>();
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddClaimsPrincipalFactory<AdditionalUserClaimsPrincipalFactory>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Blazor CRUD API",
                    Version = "v1",
                    Description = "CRUD API Services that act as the backend to the Blazor CRUD."
                });

                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                //c.CustomSchemaIds(r => r.FullName);
            });

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                {
                    const string OpenId = "openid";

                    //options.IdentityResources[OpenId].UserClaims.Add(CustomClaimTypes.Planet);
                    //options.ApiResources.Single().UserClaims.Add(CustomClaimTypes.Planet);

                    options.IdentityResources[OpenId].UserClaims.Add(JwtClaimTypes.Email);
                    options.ApiResources.Single().UserClaims.Add(JwtClaimTypes.Email);

                    options.IdentityResources[OpenId].UserClaims.Add(JwtClaimTypes.Role);
                    options.ApiResources.Single().UserClaims.Add(JwtClaimTypes.Role);
                });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove(JwtClaimTypes.Role);
            services.AddAuthentication().AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();

            //services.AddAuthorization(options =>
            //{
            //    options.AddMarsPolicy();
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
               IApplicationBuilder app,
               IWebHostEnvironment env,
               RoleManager<IdentityRole> roleManager,
               UserManager<ApplicationUser> userManager)
        {

            ApplicationDbInitialiser.SeedRoles(roleManager);
            ApplicationDbInitialiser.SeedUsers(userManager);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "blazorcrud.api v1");
                c.DefaultModelsExpandDepth(-1);
            });
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
