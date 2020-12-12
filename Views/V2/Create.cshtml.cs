using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CPSC571Project6.Models;

namespace CPSC571Project6.Views.V2
{
    public class CreateModel : PageModel
    {
        private readonly CPSC571Project6.Models.ConnectionDBClass _context;

        public CreateModel(CPSC571Project6.Models.ConnectionDBClass context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TopicClass TopicClass { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Topics.Add(TopicClass);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
