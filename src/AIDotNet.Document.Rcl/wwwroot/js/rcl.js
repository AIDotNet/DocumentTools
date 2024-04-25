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
    initTextEditor: (id) => {

        const editor = document.getElementById(id);
        if (!editor) return;


        editor.addEventListener('input', function (event) {
            adjustTextareaHeight(event.target);
        });

        function adjustTextareaHeight(textarea) {
            debugger
            // 重置高度，允许浏览器计算新的滚动高度
            textarea.style.height = 'auto';
            // 设置新的高度
            textarea.style.height = textarea.scrollHeight + 'px';
            // 限制最大高度，超过6行显示滚动条
            const maxLines = 6;
            const lineHeight = parseInt(window.getComputedStyle(textarea).lineHeight);
            const maxHeight = maxLines * lineHeight;
            if (textarea.scrollHeight > maxHeight) {
                textarea.style.height = maxHeight + 'px';
            }
        }
    },
    uploadFilePic: (quillElement, element, index) => {
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
    }, loadDocs: (url, id) => {

        fetch(url)
            .then(resp => resp.blob()).then(blob => docx.renderAsync(blob, document.getElementById(id)));

    }
};


window.MasaBlazor.extendMarkdownIt = function (parser) {
    const {md, scope} = parser;
    if (window.markdownitEmoji) {
        md.use(window.markdownitEmoji);
    }

    if (scope === "document") {
        // addHeadingRules(md);
        // addCodeRules(md);
        // addImageRules(md);
        // addBlockquoteRules(md);
        // addTableRules(md);
        // addHtmlInlineRules(md)

        parser.useContainer("code-group");
        parser.useContainer("code-group-item");
        // addCodeGroupRules(parser);
    }

    md.renderer.rules.code_block = renderCode(md.renderer.rules.code_block, md.options);
    md.renderer.rules.fence = renderCode(md.renderer.rules.fence, md.options);

    function renderCode(origRule, options) {
        return (...args) => {
            const [tokens, idx] = args;
            const content = tokens[idx].content
                .replaceAll('"', '&quot;')
                .replaceAll("'", "&lt;");
            const origRendered = origRule(...args);

            if (content.length === 0)
                return origRendered;

            return `
<div style="position: relative;border-radius: 12px;">
	${origRendered}
	<button class="markdown-it-code-copy" onclick="copy('${Encode64(tokens[idx].content)}')" style="position: absolute; top: 5px; right: 5px; cursor: pointer; outline: none;" title="复制">
		<span style="font-size: 21px; opacity: 0.4;color: #ffffff;" class="mdi mdi-content-copy"></span>
	</button>
</div>
`;
        };
    }

    window.copy = async (e) => {
        navigator.clipboard.writeText(Decode64(e))
    }

    /**
     * 编码base64
     */
    function Encode64(str) {
        return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g,
            function toSolidBytes(match, p1) {
                return String.fromCharCode('0x' + p1);
            }));
    }

    /**
     * 解码base64
     */
    function Decode64(str) {
        return decodeURIComponent(atob(str).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
    }

    function addHeadingRules(md) {
        md.renderer.rules.heading_open = (tokens, idx, options, env, self) => {
            const level = tokens[idx].markup.length;
            const next = tokens[idx + 1];
            const children = next ? next.children : [];
            const [, href] = children[2].attrs[0];
            const content = children[0].content;

            tokens[idx].tag = "app-heading";
            tokens[idx].attrSet("content", content);
            tokens[idx].attrSet("href", href);
            tokens[idx].attrSet("level", level);

            return self.renderToken(tokens, idx, options);
        };
        md.renderer.rules.heading_close = (tokens, idx, options, env, self) => {
            tokens[idx].tag = "app-heading";

            return self.renderToken(tokens, idx, options);
        };

    }

    function addHtmlInlineRules(md) {
        md.renderer.rules.html_inline = (tokens, idx, options, env, self) => {
            const content = tokens[idx].content;
            if (content.startsWith("</")) {
                return content;
            }

            let tag = content.split(' ')[0].substring(1);
            if (tag.endsWith('>')) {
                tag = tag.replace('>', '');
            }

            if (customElements.get(tag)) {
                return content.replace(">", ` masa-blazor-custom-element>`)
            }

            return content;
        }
    }

    function addCodeRules(md) {
        // md.renderer.rules.fence = (tokens, idx, options, env, self) => {
        //     if (tokens[idx].markup === "```") {
        //         debugger
        //         const content = tokens[idx].content;
        //
        //         const [lang, fileName, lineHighlights] = resolveCodeInfo(tokens[idx].info)
        //
        //         return `<default-app-markup code="${content.replaceAll(
        //             '"',
        //             "&quot;"
        //         )}" language="${lang}" file-name="${fileName || ""}" line-highlights="${lineHighlights || ""}"></default-app-markup>\n`;
        //     }
        // };
    }

    function addBlockquoteRules(md) {
        md.renderer.rules.blockquote_open = (tokens, idx, options, env, self) => {
            tokens[idx].tag = "div";
            tokens[idx].attrSet("class", "m-alert__content");
            return (`<div role="alert" class="m-alert m-alert--doc m-sheet m-alert--border m-alert--border-left m-alert--text info--text"><div class="m-alert__wrapper"><i class="m-icon theme--dark info--text mdi mdi-information m-alert__icon"></i><div class="m-alert__border m-alert__border--left"></div>${self.renderToken(tokens, idx, options)}`);
        };
        md.renderer.rules.blockquote_close = (tokens, idx, options, env, self) => {
            return self.renderToken(tokens, idx, options) + "</div></div></div>";
        };
    }


    function addTableRules(md) {
        md.renderer.rules.table_open = (tokens, idx, options, env, self) => {
            return (
                '<div masa-blazor-html class="m-sheet m-sheet--outlined m-sheet--no-bg rounded theme--light mb-2"><div masa-blazor-html class="m-data-table m-data-table--fixed-height theme--light"><div class="m-data-table__wrapper">' +
                self.renderToken(tokens, idx, options)
            );
        };
        md.renderer.rules.td_open = (tokens, idx, options, env, self) => {
            const nextToken = tokens[idx + 1];
            if (nextToken.type === "inline") {
                if (nextToken.content.startsWith('tags:')) {
                    return `<td><app-tag label="${nextToken.content}">`
                }
            }
            return self.renderToken(tokens, idx, options);
        }
        md.renderer.rules.td_close = (tokens, idx, options, env, self) => {
            const prevToken = tokens[idx - 1];
            if (prevToken.type === "inline") {
                if (prevToken.content.startsWith('tags:')) {
                    return "</app-tag></td>"
                }
            }
            return self.renderToken(tokens, idx, options);
        };
        md.renderer.rules.table_close = (tokens, idx, options, env, self) => {
            return self.renderToken(tokens, idx, options) + "</div></div></div>";
        }
    }

    function addCodeGroupRules(parser) {
        parser.md.renderer.rules["container_code-group_open"] = (
            tokens,
            idx,
            options,
            env,
            self
        ) => {
            debugger
            let nextIndex = idx;
            let nextToken = tokens[idx];

            const dic = {};

            while (nextToken) {
                nextIndex++;
                nextToken = tokens[nextIndex];

                if (nextToken.type === "container_code-group-item_open") {
                    const item = nextToken.info.replace("code-group-item", "").trim();

                    nextIndex++;
                    nextToken = tokens[nextIndex];

                    if (nextToken.type === "fence") {
                        const {content: code, info} = nextToken;
                        const [lang, fileName, lineHighlights] = resolveCodeInfo(info)

                        dic[item] = {code, lang, fileName, lineHighlights};
                    }
                }

                if (nextToken.type === "container_code-group_close") {
                    break;
                }
            }

            const g_attr = `code_group_${idx}`;
            parser.afterRenderCallbacks.push(() => {
                const selector = `[${g_attr}]`;
                const element = document.querySelector(selector);
                if (element) {
                    element.model = dic;
                }
            });

            return `<app-code-group masa-blazor-custom-element ${g_attr}>\n`;
        };
        parser.md.renderer.rules["container_code-group_close"] = (
            tokens,
            idx,
            options,
            env,
            self
        ) => {
            return `</app-code-group>\n`;
        };
    }

    function resolveCodeInfo(info) {
        info = (info || "").trim();
        const [lang, ...res] = info.split(/\s+/);

        let fileName, lineHighlights;

        const f = res.find(u => u.startsWith("f:"))
        const l = res.find(u => u.startsWith("l:"))

        if (res.length > 0 && !f && res[0] !== l) {
            fileName = res[0];
        } else {
            fileName = f && f.substring(2)
        }

        lineHighlights = l && l.substring(2)

        return [lang, fileName, lineHighlights];
    }

    function addImageRules(md) {
        md.renderer.rules.image = (tokens, idx, options, env, self) => {
            tokens[idx].attrSet("width", "100%");
            return self.renderToken(tokens, idx, options);
        };
    }
}

window.prismHighlightLines = function (pre) {
    if (!pre) return;
    try {
        setTimeout(() => {
            Prism.plugins.lineHighlight.highlightLines(pre)();
        }, 300) // in code-group-item, need to wait for 0.3s transition animation
    } catch (err) {
        console.error(err);
    }
}

window.updateThemeOfElementsFromMarkdown = function (isDark) {
    const customElements = document.querySelectorAll('[masa-blazor-custom-element]');

    [...customElements].map(e => {
        e.setAttribute("dark", isDark)
    });

    const elements = document.querySelectorAll('[masa-blazor-html]');
    [...elements].map(e => {
        if (isDark) {
            if (e.className.includes('theme--light')) {
                e.className = e.className.replace('theme--light', 'theme--dark')
            } else {
                e.className += " theme--dark";
            }
        } else {
            if (e.className.includes('theme--dark')) {

                e.className = e.className.replace('theme--dark', 'theme--light')
            } else {
                e.className += " theme--light";
            }
        }
    })
}


window.scrollToElement = function (hash, offset) {
    if (!hash) return;

    const el = document.querySelector(hash);
    if (!el) return;

    const top = el.getBoundingClientRect().top;
    const offsetPosition = top + window.pageYOffset - offset;

    window.scrollTo({top: offsetPosition, behavior: "smooth"});
};

window.updateHash = function (hash) {
    if (!hash) return
    history.replaceState({}, "", window.location.pathname + hash)
}

window.backTop = function () {
    window.scrollTo({top: 0, behavior: "smooth"})
};

window.activeNavItemScrollIntoView = function (ancestorSelector) {
    setTimeout(() => {
        const activeListItem = document.querySelector(
            `${ancestorSelector} .m-list-item--active:not(.m-list-group__header)`
        );

        if (!activeListItem) return;
        activeListItem.scrollIntoView({behavior: "smooth"});
    }, 500)
};

window.switchTheme = function (dotNetHelper, dark, x, y) {
    document.documentElement.style.setProperty('--x', x + 'px')
    document.documentElement.style.setProperty('--y', y + 'px')
    document.startViewTransition(() => {
        dotNetHelper.invokeMethodAsync('ToggleTheme', dark);
    });
}

window.isDarkPreferColor = function () {
    return window.matchMedia('(prefers-color-scheme: dark)').matches
}
