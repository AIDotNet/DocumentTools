namespace AIDotNet.Document.Services.Services
{
    public sealed class MenuItemService : IMenuItemService
    {
        public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync()
        {
            var list = new List<MenuItem>
            {
                new ("首页","mdi-home","/"),
                new ("AI工具","mdi-home","/ai-tools"),
                new (1),
                new ("我的文件夹","mdi-folder-text","/my-folder"),
            };

            return await Task.FromResult(list);
        }
    }
}
