using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPSC571Project6.Models
{
    public class AnswerClass
    {
        [Key]
        public int id { get; set; }
        public int user_Id { get; set; }
        public int questionnaire_Id { get; set; }
        public int question_Id { get; set; }
        public int answer { get; set; }
    }
}
