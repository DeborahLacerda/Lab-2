using AcademicManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab_2.Pages
{
    public class RegistrationModel : PageModel
    {
        private readonly ILogger<RegistrationModel> _logger;

        public RegistrationModel(ILogger<RegistrationModel> logger)
        {
            _logger = logger;

        }
        [BindProperty]
        public string SelectedStudent { get; set; }
        [BindProperty]
        public Course SelectedCourse { get; set; }
        public List<Course> SelectedCourses { get; set; }

        [BindProperty]
        public bool Selected { get; set; }

        public void OnGet()
        {
        }
        public void OnPost()
        {
        }


    }

}