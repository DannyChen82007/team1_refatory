namespace TestProject2
{
    using System;

    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth, "yyyyMM", null);
        }

        public DateTime LastDay()
        {
            var firstDay = this.FirstDay();
            var daysInMonth = DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
            return new DateTime(firstDay.Year, firstDay.Month, daysInMonth);
        }
    }
}
