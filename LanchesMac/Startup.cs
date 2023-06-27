using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace LanchesMac;
public class Startup
{
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {

        //Criado as seguintes duas linhas para conectar com o banco de dados
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


        services.Configure<IdentityOptions>(options =>              //Configurações de LOGIN pelo Identity
         {
             // Default Password settings.
             options.Password.RequireDigit = true;              //Requires a number between 0-9 in the password.
             options.Password.RequireLowercase = false;         //Requires a lowercase character in the password.	
             options.Password.RequireNonAlphanumeric = false;   //Requires a non-alphanumeric character in the password.
             options.Password.RequireUppercase = false;         //Requires an uppercase character in the password.	
             options.Password.RequiredLength = 6;               //The minimum length of the password.	
             options.Password.RequiredUniqueChars = 1;          //Requires the number of distinct characters in the password.
         });



        services.AddTransient<ILancheRepository, LancheRepository>();
        services.AddTransient<ICategoriaRepository, CategoriaRepository>();
        services.AddTransient<IPedidoRepository, PedidoRepository>();
        services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

        services.AddAuthorization(options =>            //Adiciona a Politica de Autorizaçao por perfil ADMIN que será usada na autorização da controller Admin na Area 
        {
            options.AddPolicy("Admin", politica => {
                politica.RequireRole("Admin");
            });
        });


        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();  //Servico para utilizar os servicos do HttpContext
                                                                             //AddSingleton = vale por todo tempo de vida da aplicação 

        services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));
        //ADdScope cria uma instancia a cada request, ou seja se dois clientes solicitarem o objeto carrinho ao
        //mesmo tempo, eles vão obter instancias diferentes 


        services.AddControllersWithViews();


        services.AddPaging(options => {                     //Service do Pacote ReflectionIT para adicionar filtro e paginação
            options.ViewName = "Bootstrap5";
            options.PageParameterName = "pageindex";
        });




        services.AddMemoryCache(); //habilita o cache
        services.AddSession();    //habilita o middleware
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedUserRoleInitial seedUserRoleInitial) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        } else {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        //cria os perfis
        seedUserRoleInitial.SeedRoles();

        //cria os usuarios e atribui ao perfil
        seedUserRoleInitial.SeedUsers();

        app.UseSession(); //utiliza o session


        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => {

            endpoints.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
 );


            endpoints.MapControllerRoute(
                name: "categoriaFiltro",
                pattern: "Lanche/{action}/{categoria?}",
                defaults: new { Controller = "Lanche", Action = "List" }
                );



            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}