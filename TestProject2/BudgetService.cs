#region

using System;
using System.Linq;

#endregion

namespace TestProject2
{
    public class BudgetService
    {
        readonly IBudgetRepo _budget;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budget = budgetRepo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return 0;
            }

            var result = _budget.GetAll();

            var queryEnd = end.ToString("yyyyyMM");

            decimal reuslt = 0;

            if (start.Year == end.Year && start.Month == end.Month)
            {
                reuslt = GetDayBudget(start, end);
            }
            else
            {
                int month = GetMonth(start, end);

                for (int i = 0; i <= month; i++)
                {
                    if (i == 0)
                    {
                        reuslt += GetDayBudget(start, new DateTime(start.Year, start.Month, 1).AddMonths(1).AddDays(-1));
                    }
                    else if (i == month)
                    {
                        reuslt += GetDayBudget(new DateTime(end.Year, end.Month, 1), end);
                    }
                    else
                    {
                        var temp = start.AddMonths(i);
                        reuslt += GetDayBudget(new DateTime(temp.Year, temp.Month, 1), new DateTime(temp.Year, temp.Month, 1).AddMonths(1).AddDays(-1));
                    }
                }
            }

            return reuslt;
        }

        /// <summary>
        ///     同年月跨日
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private decimal GetDayBudget(DateTime start, DateTime end)
        {
            decimal result = 0;

            var queryStart = start.ToString("yyyyMM");

            var diff = end.Date - start.Date;

            decimal diffStart = diff.Days + 1; // 同年月跨日

            var monthsday = DateTime.DaysInMonth(start.Year, start.Month); // 當月有幾天

            var budgetResult = _budget.GetAll().FirstOrDefault(a => a.YearMonth == queryStart);

            if (budgetResult != null)
            {
                result = (diffStart * budgetResult.Amount / monthsday);
            }
            else
            {
                result = 0;
            }

            return result;
        }

        private int GetMonth(DateTime start, DateTime end)
        {
            var mCount = 0;
            var sYM = start.Year * 100 + start.Month; //202202
            var eYM = end.Year * 100 + end.Month; //202302
            while (sYM < eYM)
            {
                var YM = start.AddMonths(++mCount);
                sYM = YM.Year * 100 + YM.Month;
            }

            return mCount;
        }
    }
}