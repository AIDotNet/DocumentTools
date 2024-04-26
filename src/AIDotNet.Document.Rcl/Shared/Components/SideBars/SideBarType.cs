using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIDotNet.Document.Rcl.Shared.Components.SideBars
{
    public enum SideBarType
    {
        [Description("完整展开")]
        Full = 0,
        [Description("紧凑图标")]
        Mini = 1,
        [Description("抽屉模式")]
        Hidden = 2,
    }
}
