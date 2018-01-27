using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Siritsit.Models
{

    [Serializable]
    public class CustomerAccount
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
}