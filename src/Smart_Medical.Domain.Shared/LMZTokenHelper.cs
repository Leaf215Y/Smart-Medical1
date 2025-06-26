using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical
{
    public class LMZTokenHelper
    {
        private readonly IConfiguration _configuration;

        //构造函数注入
        public LMZTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }




    }
}
