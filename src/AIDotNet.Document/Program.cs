using AIDotNet.Document;
using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;

internal class Program
{
	[STAThread]
	private static void Main(string[] args)
	{
		var builder = PhotinoBlazorAppBuilder.CreateDefault(args);

		builder.RootComponents.Add<App>("#app");

		builder.Services.AddDocumentRcl();
		builder.Services.AddDocumentService();

		var app = builder.Build();

		app.MainWindow
			.SetTitle("AIDotNet文档助手")
			.SetResizable(true);

		AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
		{
			
		};

		app.Run();
	}
}