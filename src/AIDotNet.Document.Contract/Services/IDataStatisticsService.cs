using AIDotNet.Document.Contract.Models;

namespace AIDotNet.Document.Contract.Services;

public interface IDataStatisticsService
{
    /// <summary>
    /// 获取数据统计
    /// </summary>
    /// <returns></returns>
    Task<DataStatisticsDto> GetDataStatisticsAsync();   
}