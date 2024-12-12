using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Adding to container Http Factory dependecy injection
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options=>
        {
            options.ExpireTimeSpan = TimeSpan.FromHours(10);
            options.LoginPath = "/auth/Login";
            options.AccessDeniedPath = "/auth/AccessDenied";
        });
//Adding Coupon Service to the http client
//<ICouponService,CouponService> This means that when we call ICouponService the object will be the type of CouponService
builder.Services.AddHttpClient<ICouponService,CouponService>();
builder.Services.AddHttpClient<IOrderService,OrderService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IShoppingCartService, ShoppingCartService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

SD.CouponAPIBase = builder.Configuration["ServiceUrls:CouponApi"];
SD.OrderAPIBase = builder.Configuration["ServiceUrls:OrderApi"];
SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthApi"];
SD.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartApi"];
SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductApi"];



//Coupling the BaseService object to the IBaseService class
//AddScoped means the lifetime of the dependecy, it will be per request
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService,CouponService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
