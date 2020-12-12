using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPSC571Project6.Models
{
    public class QuestionnaireClass
    {
        [Key]
        public int id { get; set; }
        public int topic_id { get; set; }
        public String title { get; set; }
    }
}
