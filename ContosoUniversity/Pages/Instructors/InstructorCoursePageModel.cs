using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace ContosoUniversity.Pages.Instructors
{
    public class InstructorCoursePageModel : PageModel
    {
        public List<AssignedCourseData> AssignedCourseDataList;
        
        public void PopulateAssignedCourseData(SchoolContext context, Instructor instructor)
        {
            var allCourses = context.Courses;
            var instructorCourses = new HashSet<int>(
                instructor.Courses.Select(c => c.ID));
            AssignedCourseDataList = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                AssignedCourseDataList.Add(new AssignedCourseData
                {
                    ID = course.ID, 
                    Title = course.Title, 
                    Assigned = instructorCourses.Contains(course.ID)
                });
            }
        }
    }
}
