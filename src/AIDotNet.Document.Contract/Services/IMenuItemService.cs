using AIDotNet.Document.Contract.Models;

namespace AIDotNet.Document.Contract.Services;

public interface IMenuItemService
{
	Task<IEnumerable<MenuItem>> GetMenuItemsAsync();
}