//namespace AIDotNet.Document.Rcl.Shared.Parts
//{
//    public partial class ContextMenuPart
//    {
//        [CascadingParameter, EditorRequired]
//        public string TableContextMenuId { get; set; } = null!;

//        [Parameter, EditorRequired]
//        public List<FolderItemDto> Folders { get; set; } = null!;

//        [Parameter]
//        public EventCallback<string?> OnNewFolderClick { get; set; }

//        async Task NewFolderClick(string? id) => await OnNewFolderClick.InvokeAsync(id);

//        [Parameter]
//        public EventCallback<string?> OnRemoveClick { get; set; }

//        async Task RemoveClick(string? id) => await OnRemoveClick.InvokeAsync(id);

//        //[Parameter]
//        //public EventCallback<FolderItemDto> OnUpdateNameClick { get; set; }
//        //async Task UpdateNameClick(FolderItemDto model) => await OnUpdateNameClick.InvokeAsync(model);

//        [Parameter]
//        public EventCallback<string> OnUpdateNameClick { get; set; }
//        async Task UpdateNameClick(string? id) 
//        {
//            if (string.IsNullOrWhiteSpace(id))
//            {
//                return;
//            }
            
//            await OnUpdateNameClick.InvokeAsync(id);
//        }
//    }
//}
