using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIDotNet.Document.Rcl.Shared.Parts
{
    public partial class TopBarPart
    {
        [Parameter, EditorRequired]
        public StringNumber Height { get; set; } = 40;

        [Parameter]
        public bool Clipped { get; set; }
    }
}
