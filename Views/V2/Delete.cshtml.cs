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
    public class DeleteModel : PageModel
    {
        private readonly CPSC571Project6.Models.ConnectionDBClass _context;

        public DeleteModel(CPSC571Project6.Models.ConnectionDBClass context)
        {
            _context = context;
        }

        [BindProperty]
        public TopicClass TopicClass { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TopicClass = await _context.Topics.FirstOrDefaultAsync(m => m.rowno == id);

            if (TopicClass == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TopicClass = await _context.Topics.FindAsync(id);

            if (TopicClass != null)
            {
                _context.Topics.Remove(TopicClass);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
