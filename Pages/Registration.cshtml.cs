using AcademicManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Linq;
using Lab_2.Models;





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

        public List<CourseGrade> StudentRecords { get; set; } = new List<CourseGrade>();

        [BindProperty]
        public string SortOrder { get; set; }
        public string SortProperty {  get; set; }
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

            if (SelectedStudentId != null) 
            { 
                HttpContext.Session.SetString("SelectedStudentId", SelectedStudentId); 
            }
           

            // Save Selected StudentId in Session

            LoadSelectedStudentCourses();

            if (StudentRecords.Count == 0)
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
            var studentRecords = DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId);
            var Courses = DataAccess.GetAllCourses();

            foreach (var record in studentRecords)
            {
                var targetCourse = Courses.First(course => course.CourseCode == record.CourseCode);
                var courseGrade = new Models.CourseGrade
                {
                    StudentId = record.StudentId,
                    CourseCode = record.CourseCode,
                    Grade = record.Grade,
                    CourseTitle = targetCourse.CourseTitle
                };

                StudentRecords.Add(courseGrade);
            }

            // Sort the records
            switch (SortProperty)
            {
                case "CourseCode":
                    StudentRecords = SortOrder == "Asc" ? StudentRecords.OrderBy(r => r.CourseCode).ToList()
                                                         : StudentRecords.OrderByDescending(r => r.CourseCode).ToList();
                    break;
                case "CourseTitle":
                    StudentRecords = SortOrder == "Asc" ? StudentRecords.OrderBy(r => r.CourseTitle).ToList()
                                                         : StudentRecords.OrderByDescending(r => r.CourseTitle).ToList();
                    break;
                case "Grade":
                    StudentRecords = SortOrder == "Asc" ? StudentRecords.OrderBy(r => r.Grade).ToList()
                                                         : StudentRecords.OrderByDescending(r => r.Grade).ToList();
                    break;
                default:
                    break;
            }
        }

        public IActionResult OnPostSubmitGrades(List<AcademicRecord> gradeRecords)
        {
            var studentRecord = DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId);

            foreach (var gradeRecord in gradeRecords)
            {
                var record = studentRecord.FirstOrDefault(r => r.CourseCode == gradeRecord.CourseCode);
                if (record == null)
                {
                    continue;
                }
                else
                {
                    record.Grade = gradeRecord.Grade;
                }

            }

            LoadSelectedStudentCourses();

            return Page();
        }
       

        public void OnGet(string sortOrder,string sortProperty)
        {
            SortProperty = sortProperty;
            SortOrder = sortOrder;

            // Load SelectedStudentId from session if it's not set
           if ( HttpContext.Session.GetString("SelectedStudentId") != null) { 
            SelectedStudentId = HttpContext.Session.GetString("SelectedStudentId");

                LoadSelectedStudentCourses();

            }
          
        }
    }
}
