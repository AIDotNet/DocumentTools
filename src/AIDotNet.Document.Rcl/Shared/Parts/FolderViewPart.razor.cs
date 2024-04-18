//namespace AIDotNet.Document.Rcl.Shared.Parts
//{
//    public partial class FolderViewPart
//    {
//        [CascadingParameter, EditorRequired]
//        public string TableContextMenuId { get; set; } = null!;

//        [Parameter, EditorRequired]
//        public List<FolderItemDto> Folders { get; set; }

//        private StringNumber _selectedItem;

//        private void SelectItem(FolderItemDto item)
//        {
//            _selectedItem = item.Id;

//            NavigationManager.NavigateTo($"/my-folder?folderId={item.Id}");
//        }

//        private bool _myFolderExpanded = false;

//        void OnMyFolderExpandedChanged(bool value)
//        {
//            if ((value, _myFolderExpanded) is (true, false))
//            {
//                NavigationManager.NavigateTo("/my-folder");
//            }

//            _myFolderExpanded = value;
//        }

//        [Parameter]
//        public EventCallback<FolderItemDto> OnBlur { get; set; }

//        async Task OnBlurAsync(FolderItemDto model)
//        {
//            if (string.IsNullOrWhiteSpace(model.Name))
//            {
//                return;
//            }

//            await OnBlur.InvokeAsync(model);
//        }

//        MyClass? curAdd;
//        void OnAdd(string id)
//        {
//            curAdd = new MyClass()
//            {
//                Id = id,
//                Name = "新建文件夹",
//            };
//        }
//        async Task AddChanged(string value)
//        {
//            if (string.IsNullOrWhiteSpace(value))
//            {
//                return;
//            }

//            curAdd!.Name = value;

//            var id = await FolderService.CreateAsync(new FolderItemDto()
//            {
//                Name = curAdd!.Name,
//                ParentId = curAdd.Id
//            });
//            var parent = models.First(x => x.Id == curAdd.Id);
//            (parent.Children ??= new()).Add(new MyClass()
//            {
//                Id = id,
//                Name = curAdd!.Name,
//            });
//        }
//        async Task Test() => await AddChanged(curAdd!.Name);


//        async Task OnDelete(string id)
//        {
//            await FolderService.RemoveAsync(id);

//        }




//        MyClass? curEdit;

//        void OnRename(MyClass model)
//        {
//            curEdit = model;
//        }
//        async Task EditChanged(string value)
//        {
//            if (string.IsNullOrWhiteSpace(value))
//            {
//                return;
//            }

//            curEdit!.Name = value;

//            var dto = (await FolderService.GetTreeFolderAsync()).FirstOrDefault(x => x.Id == curEdit.Id);
//            if (dto is null)
//            {
//                return;
//            }

//            await FolderService.UpdateAsync(new FolderItemDto()
//            {
//                Id = dto.Id,
//                Name = curEdit.Name,
//                ParentId = dto.ParentId
//            });
//        }

//        async Task Test2() => await EditChanged(curEdit!.Name);
//    }
//}
