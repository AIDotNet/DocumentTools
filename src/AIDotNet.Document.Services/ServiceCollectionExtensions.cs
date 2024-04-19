namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentService(this IServiceCollection services)
        {
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddSingleton<IFreeSql>((_) => new FreeSqlBuilder()
                .UseConnectionString(DataType.Sqlite, "Data Source=document.db")
                .UseAutoSyncStructure(true) //自动同步实体结构到数据库
                .Build());
            
            services.AddSingleton<IFileStorageService, FileStorageService>();
            services.AddSingleton<ILiteDatabase>(_ => new LiteDatabase("file-document.db"));

            services.AddScoped<IFolderService, FolderService>();
            services.AddSingleton<ISettingService, SettingService>();

            services.AddTransient((provider) =>
            {
                var settingService = provider.GetRequiredService<ISettingService>();
            
                var options = settingService.GetSetting<OpenAIOptions>(SettingExtensions.OpenAI.Default);
            
                return new KernelMemoryBuilder()
                    .WithOpenAI(new OpenAIConfig()
                    {
                        TextModel = options.ChatModel,
                        Endpoint = options.Endpoint,
                        EmbeddingModel = options.EmbeddingModel,
                    }, httpClient: new HttpClient(new OpenAIHttpClientHanlder(options.Endpoint)))
                    .WithCustomTextPartitioningOptions(new TextPartitioningOptions
                    {
                        MaxTokensPerParagraph = 1000,
                        MaxTokensPerLine = 300,
                        OverlappingTokens = 0,
                    })
                    .WithSimpleVectorDb(new SimpleVectorDbConfig
                    {
                        StorageType = FileSystemTypes.Disk
                    })
                    .Build();
            });

            return services;
        }
        
        public static IServiceProvider UseDocumentService(this IServiceProvider provider)
        {
            var settingService = provider.GetRequiredService<ISettingService>();
            
            settingService.Update();
            return provider;
        }
    }
}