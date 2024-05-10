import { sendCloseWindowMessage, sendMinimizeWindowMessage } from "./framework";

declare var windowService: any;

/**
 * 关闭窗口
 */
export function CloseWindow(){
    sendCloseWindowMessage();
}

/**
 * 缩小窗口
 */
export function MinimizeWindow(){
    sendMinimizeWindowMessage()
}

