namespace AIDotNet.Document.Services.Services
{
    public sealed class MenuItemService : IMenuItemService
    {
        public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync()
        {
            var list = new List<MenuItem>
            {
                new ("首页","mdi-home","/"),
                new ("AI工具","mdi-robot-happy","/ai-toolkit"),
                new (1),
                new ("我的文件夹","mdi-folder-multiple","/my-folder"),
            };

            return await Task.FromResult(list);
        }
    }
}
