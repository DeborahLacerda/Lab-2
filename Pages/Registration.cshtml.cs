using AcademicManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab_2.Pages
{
    public class RegistrationModel : PageModel
    {
        private const string NoStudentSelectedError = "You must select a Student!";
        private const string StudentSelectedHasnotCourse = "The Student has not registered for any course. Select courses to register";
        private const string StudentSelectedRegistered = "The Student has been registered for the following courses";
        private const string NoCoursSelectedError = "You must select at least one Course!";

       [BindProperty]
        public string SelectedStudentId { get; set; } = string.Empty;

        public List<AcademicRecord> RegisteredCourses { get; set; } = new List<AcademicRecord>();

        public string ErrorMessage
        {
            set
            {
                TempData["ErrorMessage"] = value;
            }
        }

        public string InfoMessage
        {
            set
            {
                TempData["InfoMessage"] = value;
            }
        }

        
        public IActionResult OnPostStudentSelected()
        {
            if (string.IsNullOrEmpty(SelectedStudentId))
            {
                ErrorMessage = NoStudentSelectedError;
                return Page();
            }

            LoadSelectedStudentCourses();

            if (RegisteredCourses.Count == 0)
            {
                InfoMessage = StudentSelectedHasnotCourse;
            }
            else
            {
                InfoMessage = StudentSelectedRegistered;
            }

            return Page();
        }

        public IActionResult OnPostRegister(List<string> selectedCourses)
        {
            if (selectedCourses == null || selectedCourses.Count == 0)
            {
                ErrorMessage = NoCoursSelectedError;
                return Page();
            }

            foreach (var courseCode in selectedCourses)
            {
                var academicRecord = new AcademicRecord(SelectedStudentId, courseCode);
                DataAccess.AddAcademicRecord(academicRecord);
            }

            LoadSelectedStudentCourses();

            return Page();
        }

        private void LoadSelectedStudentCourses()
        {
            RegisteredCourses = DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId);
        }
    }
}
