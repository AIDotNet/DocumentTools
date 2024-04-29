using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIDotNet.Document.Rcl.Shared.Components.NavModule
{
    public partial class BtnTree<TItem>
    {
        [Parameter]
        public bool Mini { get; set; }

        [Parameter, EditorRequired]
        public List<TItem> Items { get; set; } = new();

        [Parameter, EditorRequired]
        public Func<TItem, string> Color { get; set; }

        [Parameter]
        public string? ActiveClass { get; set; } = "primaryText";

        #region List parameters

        [Parameter]
        public bool Outlined { get; set; }

        [Parameter]
        public bool Shaped { get; set; }

        [Parameter]
        public bool Dense { get; set; }

        [Parameter]
        public bool Flat { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Nav { get; set; }

        [Parameter]
        public bool Routable { get; set; }

        [Parameter]
        public StringNumber Height { get; set; }

        [Parameter]
        public StringNumber MinHeight { get; set; }

        [Parameter]
        public StringNumber MinWidth { get; set; }

        [Parameter]
        public StringNumber MaxHeight { get; set; }

        [Parameter]
        public StringNumber MaxWidth { get; set; }

        [Parameter]
        public StringNumber Width { get; set; }

        #endregion
    }
}
