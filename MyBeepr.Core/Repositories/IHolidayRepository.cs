using System.Collections.Generic;
using System.Threading.Tasks;
using MyBeepr.Core.Models;

namespace MyBeepr.Core.Repositories
{
    public interface IHolidayRepository
    {
        Task<List<Holiday>> ListAsync();
    }
}