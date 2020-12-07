using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPSC571Project6.Models
{
    public class TestClass
    {
        [Key]
        public String test1 { get; set; }
        public int test2 { get; set; }
    }
}
