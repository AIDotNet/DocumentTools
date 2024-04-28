using AIDotNet.Document.Services.Domain;

namespace AIDotNet.Document.Services.Services;

public sealed class DataStatisticsService(IFreeSql freeSql) : IDataStatisticsService
{
    public async Task<DataStatisticsDto> GetDataStatisticsAsync()
    {
        var dataStatistics = new DataStatisticsDto
        {
        };

        var note = await freeSql.Select<Folder>()
            .Where(x => x.IsFolder == false)
            .OrderByDescending(x => x.CreatedTime)
            .FirstAsync();

        if (note != null)
        {
            dataStatistics.LatestUsageNote = new FolderItemDto()
            {
                Id = note.Id,
                Name = note.Name,
                ParentId = note.ParentId,
                Size = note.Size,
                IsFolder = note.IsFolder,
                Status = note.Status,
                CreatedTime = note.CreatedTime,
                Type = note.Type,
            };
        }

        dataStatistics.TotalNoteCount = await freeSql.Select<Folder>()
            .Where(x => x.IsFolder == false)
            .CountAsync();

        var latestNote = await freeSql.Select<Folder>()
            .Where(x => x.IsFolder == false)
            .OrderByDescending(x => x.UpdateTime)
            .FirstAsync();

        if (latestNote != null)
        {
            dataStatistics.LatestUpdateNote = new FolderItemDto()
            {
                Id = latestNote.Id,
                Name = latestNote.Name,
                ParentId = latestNote.ParentId,
                Size = latestNote.Size,
                IsFolder = latestNote.IsFolder,
                Status = latestNote.Status,
                CreatedTime = latestNote.CreatedTime,
                Type = latestNote.Type,
            };
        }

        return dataStatistics;
    }
}