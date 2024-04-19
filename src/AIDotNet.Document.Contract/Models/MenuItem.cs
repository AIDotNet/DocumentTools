namespace AIDotNet.Document.Contract.Models
{
    public sealed class MenuItem
    {
        public string Id { get; set; }

        public string Title { get; private set; }

        public string Icon { get; private set; }

        public string Href { get; private set; }

        public bool IsDivider { get; set; }

        public int DivNumber { get; init; }

        public MenuItem(string title, string icon, string href)
        {
            Id = Guid.NewGuid().ToString("N");

            Title = title;
            Icon = icon;
            Href = href;

            DivNumber = 0;
            IsDivider = false;
        }

        public MenuItem(int divNumber)
        {
            Id = Guid.NewGuid().ToString("N");

            Title = string.Empty;
            Icon = string.Empty;
            Href = string.Empty;

            DivNumber = divNumber;
            IsDivider = true;
        }
        protected MenuItem()
        {

        }
    }
}
