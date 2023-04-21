using BookStore_Auth_Backend_API_DataAccess.Identity;
using BookStore_Auth_Backend_API_DataAccess.ServiceContract;
using BookStore_Auth_Backend_API_DataAccess.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//Add Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        builder =>
        {
            builder.WithOrigins("AuthBookApiServices")
                   .AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        }
        );
});

//Add ConnectionString
string cs = builder.Configuration.GetConnectionString("ConStr");
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ApplicationDbContext>(option => option.UseSqlServer
      (cs, b => b.MigrationsAssembly("BookStore_Auth_Backend_API_DataAccess")));
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUser, User>();
builder.Services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
builder.Services.AddTransient<UserManager<ApplicationUser>, ApplicationUserManager>();
builder.Services.AddTransient<SignInManager<ApplicationUser>, ApplicationSignInManager>();
builder.Services.AddTransient<RoleManager<ApplicationRole>, ApplicationRoleManager>();
builder.Services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddUserStore<ApplicationUserStore>()
.AddUserManager<ApplicationUserManager>()
.AddRoleManager<ApplicationRoleManager>()
.AddSignInManager<ApplicationSignInManager>()
.AddRoleStore<ApplicationRoleStore>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<ApplicationRoleStore>();
builder.Services.AddScoped<ApplicationUserStore>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("MyPolicy");
app.UseRouting();
app.UseAuthorization();


//Code for Creating User With Code for Testing !!!

//IServiceScopeFactory serviceScopeFactory = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IServiceScopeFactory>();
//using (IServiceScope scope = serviceScopeFactory.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
//    var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//    //To Create Admin Role
//    if (!await roleManager.RoleExistsAsync("Admin"))
//    {
//        var role = new ApplicationRole();
//        role.Name = "Admin";
//        await roleManager.CreateAsync(role);
//    }
//    //To Create Employee Role
//    if (!await roleManager.RoleExistsAsync("User"))
//    {
//        var role = new ApplicationRole();
//        role.Name = "User";
//        await roleManager.CreateAsync(role);
//    }
//    //To Create Admin User
//    if (await userManger.FindByNameAsync("admin") == null)
//    {
//        var user = new ApplicationUser();
//        user.UserName = "admin";
//        user.Email = "admin@gmail.com";
//        var userPassword = "Admin@321";
//        var chkuser = await userManger.CreateAsync(user, userPassword);
//        if (chkuser.Succeeded)
//        {
//            await userManger.AddToRoleAsync(user, "Admin");
//        }
//    }
//    //To Create User
//    if (await userManger.FindByNameAsync("User") == null)
//    {
//        var user = new ApplicationUser();
//        user.UserName = "user";
//        user.Email = "user@gmail.com";
//        var userPassword = "User@321";
//        var chkuser = await userManger.CreateAsync(user, userPassword);
//        if (chkuser.Succeeded)
//        {
//            await userManger.AddToRoleAsync(user, "User");
//        }
//    }
//}

app.MapControllers();

app.Run();

