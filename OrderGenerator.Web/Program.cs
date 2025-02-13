using OrderGenerator.Web.Services;
using OrderGenerator.Web.Services.Interfaces;
using QuickFix.Logger;
using QuickFix.Store;
using QuickFix;
using QuickFix.Transport;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var settings = new SessionSettings("initiator.cfg");
var storeFactory = new FileStoreFactory(settings);
var logFactory = new FileLogFactory(settings);
//FixInitiator application = new FixInitiator();


builder.Services.AddSingleton(settings);
builder.Services.AddSingleton<IMessageStoreFactory>(storeFactory);
builder.Services.AddSingleton<ILogFactory>(logFactory);
builder.Services.AddSingleton<IFixInitiator, FixInitiator>();
var app = builder.Build();


var fixInitiator = app.Services.GetRequiredService<IFixInitiator>();

var initiator = new SocketInitiator(fixInitiator as IApplication, storeFactory, settings, logFactory);
initiator.Start();

//Console.WriteLine("FIX Initiator iniciado. Pressione qualquer tecla para sair...");
//Console.ReadKey();

//initiator.Stop();

//builder.Services.AddSingleton<FixInitiator>();
//builder.Services.AddSingleton<IFixInitiator, FixInitiator>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Order}/{action=Index}/{id?}");

app.Run();
