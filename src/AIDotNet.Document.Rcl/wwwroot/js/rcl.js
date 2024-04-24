
hljs.configure({
    languages: ['javascript',
        'ruby',
        'python',
        'java',
        'csharp',
        'html',
        'xml',
        'css',
        'json',
        'markdown',
        'sql',
        'typescript',
        'bash',
        'shell',
        'yaml',
        'ini',
        'dockerfile',
        'plaintext']
});

window.addEventListener('DOMContentLoaded', () => {
    document.body.addEventListener('mousedown', evt => {
        const {target} = evt;
        const appRegion = getComputedStyle(target)['-webkit-app-region'];

        if (appRegion === 'drag') {
            chrome.webview.hostObjects.sync.eventForwarder.MouseDownDrag();
            evt.preventDefault();
            evt.stopPropagation();
        }
    });
});

window.util = {
    copyToClipboard: (text) => {
        navigator.clipboard.writeText(text).then(() => {
            console.log('Text copied to clipboard');
        }, (err) => {
            console.error('Failed to copy text to clipboard', err);
        });
    },
    markdownUploadFile: (element, index) => {
        debugger
        let _that = this;
        let vditor = element.Vditor;
        let fileInput = element.querySelector('input[type=file]')
        let files = fileInput.files;
        for (let i = 0; i < vditorFiles.length; i++) {
            let file = vditorFiles[i];
            // 随机生成文件名
            const random = Math.random().toString(36).substring(7);
            const fileName = `${random}.png`;
            let reader = new FileReader();
            reader.onload = async function (event) {
                let fileData = event.target.result;
                let imageUrl = await fileStorageService.CreateOrUpdateImageAsync(fileName, btoa(String.fromCharCode.apply(null, new Uint8Array(fileData))));
                let succFileText = "";
                if (vditor && vditor.vditor.currentMode === "wysiwyg") {
                    succFileText += `\n <img alt=${imageUrl} src="${imageUrl}">`;
                } else {
                    succFileText += `\n![${imageUrl}](${imageUrl})`;
                }
                //Insert the uploaded picture into the editor
                document.execCommand("insertHTML", false, succFileText);
                index += 1;
                if (index < files.length) {
                    _that.util.markdownUploadFile(element, index);
                } else {
                    fileInput.value = '';
                }

                // 上传完成释放file
                file.release();
            };
            reader.readAsArrayBuffer(file);
        }
    },
    uploadFilePic: (quillElement, element, index) => {
        debugger;
        let _that = this;
        let quill = quillElement.__quill;//get quill editor
        let fileInput = element.querySelector('input.ql-image[type=file]')//get fileInput
        let files = fileInput.files;
        // 循环files
        for (let i = 0; i < files.length; i++) {
            let file = files[i];
            const random = Math.random().toString(36).substring(7);
            const fileName = `${random}.png`;
            let reader = new FileReader();
            reader.onload = async function (event) {
                let fileData = event.target.result;
                let value = await fileStorageService.CreateOrUpdateImageAsync(fileName, btoa(String.fromCharCode.apply(null, new Uint8Array(fileData))));
                let length = quill.getSelection().index;
                quill.insertEmbed(length, 'image', value);//Insert the uploaded picture into the editor
                quill.setSelection(length + 1);
                index += 1;
                if (index < files.length) {
                    _that.window.util.uploadFilePic(quillElement, element, index);
                } else {
                    fileInput.value = '';
                }
            };
            reader.readAsArrayBuffer(file);
        }
    },
    createTempUr: (data) => {
        var blob = new Blob([new Uint8Array(data)], {type: 'application/octet-stream'});
        var url = URL.createObjectURL(blob);
        return url;
    },
    AILevitatedSphereInit: () => {
        // var aiImg = document.getElementById('ai-img');
        // document.getElementById('floating-ball').addEventListener('mouseover', function() {
        //     aiImg.src = 'ai.png';
        //     this.style.width = '60px';
        //     this.style.border = 'none';
        // });
        //
        // document.getElementById('floating-ball').addEventListener('mouseout', function() {
        //     this.style.width = '20px';
        //     aiImg.src = 'ai-1.png';
        // });
    },
    scrollToBottom: (id) => {
        const element = document.getElementById(id);
        if (element) {
            element.scrollTop = element.scrollHeight;
        }
    },
    dragElement: (id) => {
        var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
        var elmnt = document.getElementById(id);
        if (!elmnt) return;
        elmnt.onmousedown = dragMouseDown;

        function dragMouseDown(e) {
            e = e || window.event;
            pos3 = e.clientX;
            pos4 = e.clientY;
            document.onmouseup = closeDragElement;
            document.onmousemove = elementDrag;
        }

        function elementDrag(e) {
            e = e || window.event;
            pos1 = pos3 - e.clientX;
            pos2 = pos4 - e.clientY;
            pos3 = e.clientX;
            pos4 = e.clientY;
            var newTop = elmnt.offsetTop - pos2;
            var newLeft = elmnt.offsetLeft - pos1;

            // 获取窗口的宽度和高度
            var winWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
            var winHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;

            // 检查元素的位置是否超出了窗口的边界
            if (newLeft < 0) {
                newLeft = 0;
            } else if (newLeft + elmnt.offsetWidth > winWidth) {
                newLeft = winWidth - elmnt.offsetWidth;
            }

            if (newTop < 0) {
                newTop = 0;
            } else if (newTop + elmnt.offsetHeight > winHeight) {
                newTop = winHeight - elmnt.offsetHeight;
            }

            // 设置元素的新位置
            elmnt.style.top = newTop + "px";
            elmnt.style.left = newLeft + "px";
        }

        function closeDragElement() {
            document.onmouseup = null;
            document.onmousemove = null;
        }
    },
    showFloatingPanel: () => {
        var panel = document.getElementById("floating-ball");
        panel.style.height = "450px";
        panel.style.width = "250px";
    },
    hideFloatingPanel: () => {
        var panel = document.getElementById("floating-ball");
        panel.style.height = "60px";
        panel.style.width = "60px";
    }
};