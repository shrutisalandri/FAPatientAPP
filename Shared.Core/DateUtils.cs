using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Core
{
    public static class DateUtils
    {
        public static void BuildStartAndEndTime(DateTime startDateTime, DateTime finishDateTime,
                                                        out long millisStartDate, out long millisFinishDate)
        {
            //DateTime startDateTime = DateTime.UtcNow;
            //DateTime finishDateTime =  new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 23, 59, 59);
            millisStartDate = (long)((startDateTime - new DateTime(1970, 1, 1)).TotalMilliseconds / 1000);
            millisFinishDate = (long)((finishDateTime - new DateTime(1970, 1, 1)).TotalMilliseconds / 1000);
        }

        public static string ToYYYYMMDD(DateTime date)
        {
            //return date.ToString(ConfHelper.GetProperty("date.format"));
            return date.ToString("yyyyMMdd");
        }

        public static string ToHHMM(DateTime date)
        {
            //return date.ToString(ConfHelper.GetProperty("date.format"));
            return date.ToString("HHmm");
        }

        /**
		 * (eg. 03 Mar 2014). For PPMP
		 */
        public static string ToDD_MMM_YYYY(DateTime date)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
            return date.ToString("dd MMM yyyy", ci);
        }

        public static string ToYYYY_MM_DD(DateTime date)
        {
            //return date.ToString(ConfHelper.GetProperty("date.format"));
            return date.ToString("yyyy'-'MM'-'dd");
        }

        //public static string To2012-07-16T15:15:00(

        public static DateTime BuildDateFromZString(string dateAsString)
        {
            // The dateAsString will have the following format: 2012-09-27T02:30:00Z"
            DateTime dateTime = new DateTime();
            try
            {
                dateTime = Convert.ToDateTime(dateAsString);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
            return dateTime;
        }

        public static DateTime AddDurationInMinutes(DateTime original, int durationInMinutes)
        {
            return original.AddMinutes(durationInMinutes);
        }

        public static int CalculateSecondsFromMidnight(DateTime dateTime)
        {
            return (int)dateTime.TimeOfDay.TotalSeconds;
        }

        public static int CalculateAppointmentLengthInSeconds(DateTime startTime, DateTime endTime)
        {
            return (int)(endTime.TimeOfDay.TotalSeconds - startTime.TimeOfDay.TotalSeconds);
        }

        public static int CalculateTimeDifferenceInMinutes(DateTime firstTime, DateTime secondTime)
        {
            TimeSpan duration = secondTime - firstTime;
            int minutes = (duration.Days * 24 * 60) + (duration.Hours * 60) + duration.Minutes;
            return minutes;
        }

        public static DateTime CalculateStartDateAndTime(DateTime date, int secondsFromMidnight)
        {
            return date.AddSeconds(secondsFromMidnight);
        }

        public static DateTime CalculateEndDateAndTime(DateTime date, int secondsFromMidnight, int lengthInSeconds)
        {
            return date.AddSeconds(secondsFromMidnight + lengthInSeconds);
        }

        public static DateTime DateAtMidnight(DateTime date)
        {
            return date.Date;
        }

        public static bool IsMidnight(DateTime date)
        {
            return date.Equals(DateAtMidnight(date));
        }

        // Format of time: HH:MM
        public static DateTime BuildPPMPDate(DateTime onlyDate, string timeAsHHMM)
        {
            DateTime timeOnlyDate = DateTime.Parse(timeAsHHMM);
            DateTime result = onlyDate;
            result = result.AddHours(timeOnlyDate.Hour);
            result = result.AddMinutes(timeOnlyDate.Minute);
            return result;
        }

        public static bool DateBetweenTwoDatesIncludingBoth(DateTime startDate, DateTime endDate, DateTime theDate)
        {
            return theDate >= startDate && theDate <= endDate;
        }

        public static string ToISODateTimeString(DateTime dt)
        {
            return dt.ToString("s");
        }

        public static int SlotsBetween(DateTime startTime, DateTime endTime, int minutesPerSlot)
        {
            return (int)CalculateTimeDifferenceInMinutes(startTime, endTime) / minutesPerSlot;
        }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public static bool PeriodsConflict(TimeSpan period1Start, TimeSpan period1End, TimeSpan period2Start, TimeSpan period2End)
        {
            // Period1 begins and ends within or matches exactly Period2
            if (period1Start >= period2Start && period1End <= period2End)
                return true;

            // Period1 starts during Period2
            if (period1Start >= period2Start && period1Start < period2End)
                return true;

            // Period1 ends during Period2
            if (period1End > period2Start && period1End <= period2End)
                return true;

            // Period1 starts before and ends after Period2
            if (period1Start < period2Start && period1End > period2End)
                return true;

            return false;
        }

        public static bool PeriodsConflict(int period1Start, int period1End, int period2Start, int period2End)
        {
            return PeriodsConflict(TimeSpan.FromMinutes(period1Start), TimeSpan.FromMinutes(period1End), TimeSpan.FromMinutes(period2Start), TimeSpan.FromMinutes(period2End));
        }
        public static bool DateTimesConflict(DateTime date1Start, DateTime date1End, DateTime date2Start, DateTime date2End)
        {
            return date1Start < date2End && date2Start < date1End;
        }

    }
}
