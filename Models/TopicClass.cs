using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPSC571Project6.Models
{
    public class TopicClass
    {
        [Key]
        public int rowno { get; set; }
        public String subject_Name { get; set; }
    }
}
