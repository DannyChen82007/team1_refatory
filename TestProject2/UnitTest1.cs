#region

using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace TestProject2
{
    [TestFixture]
    public class BudgetServiceTests
    {
        IBudgetRepo _budgetRepo;
        BudgetService service;

        [SetUp]
        public void Setup()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
        }

        [Test]
        public void query_whole_month()
        {
            var a = new List<Budget>()
                    {
                        new Budget()
                        {
                            YearMonth = "202212", Amount = 3100
                        },
                    };

            _budgetRepo.GetAll().Returns(a);

            var service = new BudgetService(_budgetRepo);

            DateTime start = new DateTime(2022, 12, 01);
            var result = service.Query(start, new DateTime(2022, 12, 31));

            Assert.AreEqual(3100m, result);
        }

        [Test]
        public void invalid_period()
        {
            var a = new List<Budget>()
                    {
                        new Budget()
                        {
                            YearMonth = "202212", Amount = 3100
                        },
                    };

            _budgetRepo.GetAll().Returns(a);

            var service = new BudgetService(_budgetRepo);

            DateTime start = new DateTime(2022, 12, 16);
            var result = service.Query(start, new DateTime(2022, 12, 14));

            Assert.AreEqual(0m, result);
        }

        [Test]
        public void query_single_day()
        {
            var a = new List<Budget>()
                    {
                        new Budget()
                        {
                            YearMonth = "202212", Amount = 3100
                        },
                    };

            _budgetRepo.GetAll().Returns(a);

            var service = new BudgetService(_budgetRepo);

            DateTime start = new DateTime(2022, 12, 15);
            var result = service.Query(start, new DateTime(2022, 12, 15));

            Assert.AreEqual(100m, result);
        }

        [Test]
        public void cross_year()
        {
            var a = new List<Budget>()
                    {
                        new Budget()
                        {
                            YearMonth = "202212", Amount = 3100
                        },
                        new Budget()
                        {
                            YearMonth = "202301", Amount = 310
                        },
                        new Budget()
                        {
                            YearMonth = "202302", Amount = 28
                        },
                    };

            _budgetRepo.GetAll().Returns(a);

            var service = new BudgetService(_budgetRepo);

            DateTime start = new DateTime(2022, 12, 23);
            var result = service.Query(start, new DateTime(2023, 2, 12));

            Assert.AreEqual(900 + 310 + 12, result);
        }
    }
}