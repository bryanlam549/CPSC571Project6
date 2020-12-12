using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CPSC571Project6.Models;

namespace CPSC571Project6.Views.V2
{
    public class IndexModel : PageModel
    {
        private readonly CPSC571Project6.Models.ConnectionDBClass _context;

        public IndexModel(CPSC571Project6.Models.ConnectionDBClass context)
        {
            _context = context;
        }

        public IList<TopicClass> TopicClass { get;set; }

        public async Task OnGetAsync()
        {
            TopicClass = await _context.Topics.ToListAsync();
        }
    }
}
