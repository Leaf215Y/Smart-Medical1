using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.UserLoginECC
{
    public class ResultLoginDto
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public bool? UserSex { get; set; }
        public string token { get; set; }
    }
}
