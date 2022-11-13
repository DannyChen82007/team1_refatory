using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace TestProject2
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}
