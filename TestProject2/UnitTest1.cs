#region

using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace TestProject2
{
    public class Tests
    {
        IBudgetRepo _budgetRepo;
        BudgetService service;

        [SetUp]
        public void Setup()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
        }

        [Test]
        public void Test1()
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

            var result = service.Query(new DateTime(2022, 12, 01), new DateTime(2022, 12, 31));

            Assert.AreEqual(3100m, result);
        }

        [Test]
        public void 非法的()
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

            var result = service.Query(new DateTime(2022, 12, 16), new DateTime(2022, 12, 14));

            Assert.AreEqual(0m, result);
        }

        [Test]
        public void 查一天()
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

            var result = service.Query(new DateTime(2022, 12, 15), new DateTime(2022, 12, 15));

            Assert.AreEqual(100m, result);
        }

        [Test]
        public void 跨年()
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

            var result = service.Query(new DateTime(2022, 12, 23), new DateTime(2023, 2, 12));

            Assert.AreEqual(1222m, result);
        }
    }
}