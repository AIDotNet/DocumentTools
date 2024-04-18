using AIDotNet.Document.Contract.Models;

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
				new (true),
				new ("我的文件夹","mdi-home","/my-folder",new List<MenuItem>()
				{
					new ("我的资源",string.Empty,"/my-folder"),
				}),
			};

			return await Task.FromResult(list);
		}
	}
}
