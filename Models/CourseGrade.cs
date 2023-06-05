using AcademicManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Linq;
namespace Lab_2.Models
{
    public class CourseGrade:AcademicRecord
    {
        public string CourseTitle { get; set; }
    }
}
