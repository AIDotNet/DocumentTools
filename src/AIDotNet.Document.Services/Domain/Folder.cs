﻿using FreeSql.DataAnnotations;

namespace AIDotNet.Document.Services.Domain;

[Table(Name = "folder")]
public class Folder
{
    [Column(IsPrimary = true)]
    public string Id { get; set; }

    public string Name { get; set; }

    public string? ParentId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
    
    /// <summary>
    /// 是否目录
    /// </summary>
    public bool IsFolder { get; set; }

    /// <summary>
    /// 如果是文件，文件大小
    /// </summary>
    public long Size { get; set; }
    
    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parentId"></param>
    public Folder(string name, string? parentId)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        ParentId = parentId;
        CreatedTime = DateTime.Now;
        IsFolder = true;
        Size = 0;
        IsDeleted = false;
    }
    
    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parentId"></param>
    /// <param name="size"></param>
    public Folder(string name, string? parentId,long size = 0)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        ParentId = parentId;
        CreatedTime = DateTime.Now;
        IsFolder = false;
        Size = size;
        IsDeleted = false;
    }
    

    protected Folder()
    {
    }
}
