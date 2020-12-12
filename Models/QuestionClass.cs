using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPSC571Project6.Models
{
    public class QuestionClass
    {
        [Key]
        public int id { get; set; }
        public int questionnaire_id { get; set; }
        public String question  { get; set; }

    }
}
