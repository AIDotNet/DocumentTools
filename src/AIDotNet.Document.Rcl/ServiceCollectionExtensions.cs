namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentRcl(this IServiceCollection services)
        {
            services.AddMasaBlazor();

            services.AddBlazorContextMenu();

            return services;
        }
    }
}
