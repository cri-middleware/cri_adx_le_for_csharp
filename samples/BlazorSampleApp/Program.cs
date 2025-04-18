using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorSampleApp;
using CriWare;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

CriBaseCSharp.ErrorCallback.Event += (error) => Console.WriteLine(error);

CriAtomCSharp.GetDefaultConfig(out var config);
CriAtomCSharp.Initialize(config);

await builder.Build().RunAsync();
