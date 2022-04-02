using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using Microsoft.Extensions.Configuration;
using ContosoUniversity.Models;
using System;

namespace ContosoUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(ContosoUniversity.Data.SchoolContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string NameSort { get; set; } 
        public string DateSort {get; set;}
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Student> Students { get;set; }

        public async Task OnGetAsync(string sortString, string currentFilter, string searchString, int? PageIndex)
        {
            CurrentSort = sortString;
            NameSort = (String.IsNullOrEmpty(sortString)) ? "Name_Desc": "";
            DateSort = (sortString == "Date") ? "Date_Desc" : "Date";
            if (searchString != null)
            {
                PageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Student> StudentsIQ = from s in _context.Students select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                StudentsIQ = StudentsIQ.
                    Where(s => 
                    s.FirstMidName.Contains(searchString)
                    ||
                    s.LastName.Contains(searchString));
            }

            switch (sortString)
            {
                case "Name_Desc":
                    StudentsIQ = StudentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date_Desc":
                    StudentsIQ = StudentsIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                case "Date":
                    StudentsIQ = StudentsIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                default:
                    StudentsIQ = StudentsIQ.OrderBy(s => s.LastName);
                    break;
            }

            var pageSize = Configuration.GetValue("PageSize", 4);
            Students = await PaginatedList<Student>.CreateAsync(
                StudentsIQ, 
                PageIndex ?? 1, pageSize);
        }
    }
}
