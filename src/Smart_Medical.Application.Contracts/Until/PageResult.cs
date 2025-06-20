using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Until
{
    public class PageResult<T>
    {
        public int TotleCount { get; set; }
        public int TotlePage { get; set; }
        public T Data { get; set; }
    }
    public class Seach
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
