
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDocumentService(this IServiceCollection services)
		{
			services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddSingleton<IFreeSql>((services) => new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=document.db")
                .UseAutoSyncStructure(true) //自动同步实体结构到数据库
                .Build());

            services.AddScoped<IFolderService, FolderService>();
			return services;
		}
	}
}
