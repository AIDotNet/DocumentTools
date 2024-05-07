using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIDotNet.Document.Rcl.Shared.Components.SideBars
{
    public partial class MultiSideBar
    {
        [Parameter, EditorRequired]
        public StringNumber Width { get; set; } = null!;

        [Parameter]
        public bool Clipped { get; set; }

        [Parameter, EditorRequired]
        public List<MenuItem> Menus { get; set; } = null!;

        bool Permanent;
        bool Mini;
        bool Temp;
        bool? open;
        SideBarType _sideBarType;

        static SideBarType SideBarType_LS = SideBarType.Full;
        protected override async Task OnInitializedAsync()
        {
            await GetSideBarType();
        }

        async Task GetSideBarType()
        {
            _sideBarType = SideBarType_LS;

            (Permanent, Mini, Temp) = _sideBarType switch
            {
                SideBarType.Hidden => (false, false, true),
                SideBarType.Mini => (true, true, false),
                SideBarType.Full => (true, false, false),
                _ => (true, false, false),
            };

            if (_sideBarType is SideBarType.Hidden)
            {
                open = false;
            }

            _modeSwitchModels = _allModeSwitchModels.Where(x => x.type != _sideBarType).ToArray();

            await Task.Delay(200);
        }

        async Task SetSideBarType()
        {
            SideBarType_LS = _sideBarType;
            await Task.Delay(200);
        }

        async Task SetType(SideBarType type, bool force = false)
        {
            if (force || _sideBarType != type)
            {
                _sideBarType = type;

                (Permanent, Mini, Temp) = _sideBarType switch
                {
                    SideBarType.Hidden => (false, false, true),
                    SideBarType.Mini => (true, true, false),
                    SideBarType.Full => (true, false, false),
                    _ => (true, false, false),
                };

                if (_sideBarType is SideBarType.Hidden)
                {
                    open = false;
                }

                _modeSwitchModels = _allModeSwitchModels.Where(x => x.type != _sideBarType).ToArray();

                await SetSideBarType();
            }
        }

        private (SideBarType type, string color, string icon, string label)[] _modeSwitchModels = [];

        private readonly (SideBarType type, string color, string icon, string label)[] _allModeSwitchModels =
        [
            (SideBarType.Hidden, "teal darken-2", "mdi-backburger", SideBarType.Hidden.ToString()),
            (SideBarType.Mini, "teal darken-2", "mdi-backburger", SideBarType.Mini.ToString()),
            (SideBarType.Full, "teal darken-2", "mdi-backburger", SideBarType.Full.ToString()),
        ];
    }
}
