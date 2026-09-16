using MiniCMS.Web.Components;
using MiniCMS.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Thêm MudBlazor
builder.Services.AddMudServices();

// Token & Auth Handler
builder.Services.AddScoped<TokenStorageService>();
builder.Services.AddTransient<AuthHeaderHandler>();

// Cấu hình HttpClient gọi tới Backend API kèm Token Handler
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7297/"); // Thay port API backend của bạn
})
.AddHttpMessageHandler<AuthHeaderHandler>();

// Đăng ký HttpClient mặc định dùng cấu hình trên
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
