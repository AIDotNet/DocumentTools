using System.Diagnostics.CodeAnalysis;
using AIDotNet.Document.Contract;
using Azure.AI.OpenAI;
using Microsoft.SemanticKernel;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentService(this IServiceCollection services)
        {
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddSingleton<IFreeSql>((_) => new FreeSqlBuilder()
                .UseConnectionString(DataType.Sqlite, "Data Source=document.db;attachs=file_storage")
                .UseAutoSyncStructure(true) //自动同步实体结构到数据库
                .Build());

            services.AddSingleton<IFileStorageService, FileStorageService>();

            services.AddSingleton<IFolderService, FolderService>();
            services.AddSingleton<ISettingService, SettingService>();
            services.AddSingleton<IDataStatisticsService, DataStatisticsService>();
            services.AddSingleton<IChatService, ChatService>();

            services.AddTransient<Kernel>(provider =>
            {
                var settingService = provider.GetRequiredService<ISettingService>();

                var options = settingService.GetSetting<OpenAIOptions>(Constant.Settings.OpenAIOptions);

                options ??= new();

                var kernel = Kernel.CreateBuilder()
                    .AddOpenAIChatCompletion(
                        modelId: options.ChatModel,
                        apiKey: options.ApiKey,
                        httpClient: new HttpClient(new OpenAIHttpClientHanlder(options.Endpoint)))
                    .Build();
                return kernel;
            });

            services.AddScoped<IKernelService, KernelService>();

            services.AddTransient((services) =>
            {
                var settingService = services.GetRequiredService<ISettingService>();
                
                var options = settingService.GetSetting<OpenAIOptions>(Constant.Settings.OpenAIOptions);

                options ??= new();

                return new KernelMemoryBuilder()
                    .WithOpenAI(new OpenAIConfig()
                    {
                        TextModel = options.ChatModel,
                        APIKey = options.ApiKey,
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
                        StorageType = FileSystemTypes.Disk,
                    })
                    .Build<MemoryServerless>();
            });

            return services;
        }

        public static IServiceProvider UseDocumentService(this IServiceProvider provider)
        {
            var settingService = provider.GetRequiredService<ISettingService>();
            var folderService = provider.GetRequiredService<IFolderService>();
            
            
            settingService.Update();
            
            
            return provider;
        }
    }
}