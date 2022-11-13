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

            if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
            {
                return GetDayBudget(start, end);
            }

            decimal result = 0;

            var currentDate = start;
            while (currentDate < new DateTime(end.Year, end.Month, 1).AddMonths(1))
            {
                var budget = this._budget.GetAll().FirstOrDefault(a => a.YearMonth == currentDate.ToString("yyyyMM"));

                if (budget != null)
                {
                    DateTime overlappingStart;
                    DateTime overlappingEnd;
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        overlappingStart = start;
                        overlappingEnd = new DateTime(start.Year, start.Month, 1).AddMonths(1).AddDays(-1);
                    }
                    else if (budget.YearMonth == end.ToString("yyyyMM"))
                    {
                        overlappingStart = new DateTime(end.Year, end.Month, 1);
                        overlappingEnd = end;
                    }
                    else
                    {
                        overlappingStart = budget.FirstDay();
                        overlappingEnd = budget.LastDay(); 
                    }
                    
                    decimal diffStart = (overlappingEnd.Date - overlappingStart.Date).Days + 1; // 同年月跨日

                    var daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month); // 當月有幾天
                    
                    result += diffStart * budget.Amount / daysInMonth;
                }

                currentDate = currentDate.AddMonths(1);
            }

            return result;
        }

        /// <summary>
        ///     同年月跨日
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private decimal GetDayBudget(DateTime start, DateTime end)
        {
            //decimal result = 0;

            decimal diffStart = (end.Date - start.Date).Days + 1; // 同年月跨日

            var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month); // 當月有幾天

            var budget = this._budget.GetAll().FirstOrDefault(a => a.YearMonth == start.ToString("yyyyMM"));

            if (budget != null)
            {
                return (diffStart * budget.Amount / daysInMonth);
            }

            return 0;

        }
    }
}