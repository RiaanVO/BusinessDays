using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MyBeepr.Core.Services;


namespace MyBeepr.Controllers
{
    [Route("/api/[controller]")]
    public class BusinessDaysController : Controller
    {
        private readonly IBusinessDaysService _businessDaysService;
        
        public BusinessDaysController(IBusinessDaysService businessDaysService)
        {
            _businessDaysService = businessDaysService;   
        }

        [HttpGet]
        public async Task<BusinessDaysResponse> GetBusinessDays([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var numBusinessDays = await _businessDaysService.CalcBusinessDaysAsync(start, end);
            return new BusinessDaysResponse()
            {
                NumBusinessDays = numBusinessDays
            };
        }
    }
}