using System.ComponentModel;

namespace MyBeepr.Core.Models
{
    public enum EHolidayType : byte
    {
        [Description("FD")]
        FixedDate = 1,

        [Description("SD")]
        ShiftingDay = 2,
        
        [Description("OD")]
        OccurrenceDay = 3,
    }
}