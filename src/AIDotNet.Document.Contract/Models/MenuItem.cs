namespace AIDotNet.Document.Contract.Models
{
	public sealed class MenuItem
	{
		public string Id { get; set; }
		public string Title { get; private set; }

		public string Icon { get; private set; }

		public string Href { get; private set; }

		public List<MenuItem>? Children { get; set; }

		public bool IsIsDivider { get; set; }

		public MenuItem(string title, string icon, string href, List<MenuItem>? children = null)
		{
			Title = title;
			Icon = icon;
			Href = href;
			Children = children;
			Id = Guid.NewGuid().ToString("N");
		}

		public MenuItem(bool isDivider)
		{
			Title = string.Empty;
			Icon = string.Empty;
			Href = string.Empty;
			Children = null;
			IsIsDivider = isDivider;
		}
		protected MenuItem()
		{

		}
	}
}
