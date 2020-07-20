using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MyBeepr.Core.Models;
using MyBeepr.Core.Repositories;

namespace MyBeepr.Data.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly List<Holiday> _holidays;
        public HolidayRepository() : base()
        {
            _holidays = new List<Holiday>();
            try
            { 
                var lineNum = 0;
                using (var sr = new StreamReader("HolidayData.csv"))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        if (lineNum != 0 && line != null)
                        {
                            try
                            {
                                _holidays.Add(ExtractHoliday(line));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Failed to extract a holiday from line: " + lineNum);
                                Console.WriteLine(e.Message);
                            }
                        }
                        lineNum += 1;
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Supplied holiday data file could not be read");
                Console.WriteLine(e.Message);
            }
        }

        private static Holiday ExtractHoliday(string line)
        {
            var lineData = line.Split(',');
            Enum.TryParse(lineData[0], out EHolidayType hType);
            var day = lineData[1] != "" ? int.Parse(lineData[1]) : (int?) null;
            var dayOfWeek = lineData[2] != "" ? int.Parse(lineData[2]) : (int?) null;
            var occurrenceInMonth = lineData[3] != "" ? int.Parse(lineData[3]) : (int?) null;
            var month = int.Parse(lineData[4]);
            return new Holiday(hType, day, dayOfWeek, occurrenceInMonth, month);
        }

        public async Task<List<Holiday>> ListAsync()
        {
            return _holidays;
        }
    }
}