using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPSC571Project6.Models
{
    
    public class ChartDataClass
    {
        public int questionnaire_Id { get; set; }
        [Key]
        public int question_Id{ get; set; }
        public String title { get; set; }
        public String question { get; set; }
        public int value_1_count { get; set; }
        public int value_2_count { get; set; }
        public int value_3_count { get; set; }
        public int value_4_count { get; set; }
        public int value_5_count { get; set; }

    }
}
