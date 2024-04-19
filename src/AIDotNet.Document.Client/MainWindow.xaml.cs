using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AIDotNet.Document.Client
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var services = new ServiceCollection();
			services.AddWpfBlazorWebView();
			services.AddDocumentRcl();
			services.AddDocumentService();
#if DEBUG
			services.AddBlazorWebViewDeveloperTools();
#endif

			BlazroWeb.RootComponents.Add(new Microsoft.AspNetCore.Components.WebView.Wpf.RootComponent()
			{
				Selector = "#app",
				ComponentType = typeof(Document.App),
			});

			var app = services.BuildServiceProvider();


			Resources.Add("services", app);
		}
	}
}