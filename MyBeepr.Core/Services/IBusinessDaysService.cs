using System;
using System.Threading.Tasks;

namespace MyBeepr.Core.Services
{
    public interface IBusinessDaysService
    {
        Task<long> CalcBusinessDaysAsync(DateTime startDate, DateTime endDate);
    }
}