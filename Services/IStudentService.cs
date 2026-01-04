using StudentManagementSystem.Models.ViewModels;
using System.Collections.Generic;

namespace StudentManagementSystem.Services
{
    public interface IStudentService
    {
        IEnumerable<StudentListVM> GetStudents(string search = null);
        PagedStudentListVM GetStudentsPaged(string search = null, int pageNumber = 1, int pageSize = 10);
        StudentCreateVM GetCreateViewModel();
        StudentEditVM GetEditViewModel(string nric);
        bool CreateStudent(StudentCreateVM model, out string errorMessage);
        bool UpdateStudent(StudentEditVM model, out string errorMessage);
        int CalculateAge(System.DateTime dateOfBirth);
    }
}

