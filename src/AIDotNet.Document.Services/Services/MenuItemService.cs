namespace AIDotNet.Document.Services.Services
{
    public sealed class MenuItemService : IMenuItemService
    {
        public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync()
        {
            var list = new List<MenuItem>
            {
                new ("1首页","mdi-home","/"),
                new ("AI工具","mdi-home","/ai-tools"),
                new (1),
                new ("1我的文件夹","mdi-home","/my-folder"),
            };

            return await Task.FromResult(list);
        }
    }
}
