using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPSC571Project6.Models;

namespace CPSC571Project6.Views.V2
{
    public class EditModel : PageModel
    {
        private readonly CPSC571Project6.Models.ConnectionDBClass _context;

        public EditModel(CPSC571Project6.Models.ConnectionDBClass context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TopicClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicClassExists(TopicClass.rowno))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TopicClassExists(int id)
        {
            return _context.Topics.Any(e => e.rowno == id);
        }
    }
}
