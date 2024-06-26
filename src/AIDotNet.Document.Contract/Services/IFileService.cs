﻿namespace AIDotNet.Document.Contract.Services;

public interface IFileService
{
    /// <summary>
    /// 打开文件
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    Task OpenFileAsync(string filter,Action<string> callback);

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="suffixName"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    Task SaveFileAsync(string name,string suffixName,Action<string> callback);
}