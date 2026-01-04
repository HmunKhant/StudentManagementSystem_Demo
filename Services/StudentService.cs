using StudentManagementSystem.Data;
using StudentManagementSystem.Models.Entities;
using StudentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StudentManagementSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly ILoggingService _loggingService;

        public StudentService(IStudentRepository repository, ILoggingService loggingService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        }

        public IEnumerable<StudentListVM> GetStudents(string search = null)
        {
            IEnumerable<Student> students;

            if (!string.IsNullOrWhiteSpace(search))
            {
                students = _repository.FindWithSubjects(s => s.NRIC.Contains(search) || s.Name.Contains(search));
            }
            else
            {
                students = _repository.GetAllWithSubjects();
            }

            return students
                .Select((x, i) => new StudentListVM
                {
                    SerialNo = i + 1,
                    NRIC = x.NRIC,
                    Name = x.Name,
                    Gender = x.Gender,
                    Age = CalculateAge(x.DateOfBirth),
                    NoOfSubjects = x.StudentSubjects != null ? x.StudentSubjects.Count() : 0
                })
                .ToList();
        }

        public PagedStudentListVM GetStudentsPaged(string search = null, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            IEnumerable<Student> students;
            int totalRecords;

            if (!string.IsNullOrWhiteSpace(search))
            {
                System.Linq.Expressions.Expression<System.Func<Student, bool>> predicate = 
                    s => s.NRIC.Contains(search) || s.Name.Contains(search);
                students = _repository.FindWithSubjectsPaged(predicate, pageNumber, pageSize);
                totalRecords = _repository.Count(predicate);
            }
            else
            {
                students = _repository.GetAllWithSubjectsPaged(pageNumber, pageSize);
                totalRecords = _repository.Count();
            }

            var studentList = students
                .Select((x, i) => new StudentListVM
                {
                    SerialNo = (pageNumber - 1) * pageSize + i + 1,
                    NRIC = x.NRIC,
                    Name = x.Name,
                    Gender = x.Gender,
                    Age = CalculateAge(x.DateOfBirth),
                    NoOfSubjects = x.StudentSubjects != null ? x.StudentSubjects.Count() : 0
                })
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return new PagedStudentListVM
            {
                Students = studentList,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                Search = search
            };
        }

        public StudentCreateVM GetCreateViewModel()
        {
            var subjects = _repository.GetAllSubjects();
            
            return new StudentCreateVM
            {
                Subjects = subjects.Select(x => new SelectListItem
                {
                    Text = x.SubjectName,
                    Value = x.SubjectId.ToString()
                }).ToList()
            };
        }

        public StudentEditVM GetEditViewModel(string nric)
        {
            if (string.IsNullOrWhiteSpace(nric))
                return null;

            var student = _repository.GetByNRICWithSubjects(nric);
            
            if (student == null)
                return null;

            var subjects = _repository.GetAllSubjects();

            return new StudentEditVM
            {
                StudentId = student.StudentId,
                NRIC = student.NRIC,
                Name = student.Name,
                Gender = student.Gender,
                DateOfBirth = student.DateOfBirth,
                AvailableDate = student.AvailableDate,
                SelectedSubjectIds = student.StudentSubjects.Select(x => x.SubjectId).ToList(),
                Subjects = subjects.Select(s => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = s.SubjectName
                }).ToList()
            };
        }

        public bool CreateStudent(StudentCreateVM model, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                // Check if NRIC already exists
                if (_repository.Exists(model.NRIC))
                {
                    errorMessage = "This NRIC is already registered.";
                    return false;
                }

                var student = new Student
                {
                    NRIC = model.NRIC,
                    Name = model.Name,
                    Gender = model.Gender,
                    DateOfBirth = model.DateOfBirth,
                    AvailableDate = model.AvailableDate
                };

                _repository.Add(student);
                _repository.SaveChanges();

                // Add selected subjects if any
                if (model.SelectedSubjectIds != null && model.SelectedSubjectIds.Any())
                {
                    foreach (var subjectId in model.SelectedSubjectIds)
                    {
                        _repository.AddStudentSubject(new StudentSubject
                        {
                            StudentId = student.StudentId,
                            SubjectId = subjectId
                        });
                    }
                    _repository.SaveChanges();
                }

                _loggingService.Log("Create Student", $"Student created: {student.NRIC} - {student.Name}");
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.Log("Error - Create Student", ex.Message, true);
                errorMessage = "An error occurred while saving. Please try again.";
                return false;
            }
        }

        public bool UpdateStudent(StudentEditVM model, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var student = _repository.GetByIdWithSubjects(model.StudentId);

                if (student == null)
                {
                    errorMessage = "Student not found.";
                    return false;
                }

                // Check if NRIC is being changed and if new NRIC already exists
                if (student.NRIC != model.NRIC && _repository.Exists(model.NRIC))
                {
                    errorMessage = "This NRIC is already registered.";
                    return false;
                }

                student.NRIC = model.NRIC;
                student.Name = model.Name;
                student.Gender = model.Gender;
                student.DateOfBirth = model.DateOfBirth;
                student.AvailableDate = model.AvailableDate;

                // Remove existing subjects
                _repository.RemoveStudentSubjects(student.StudentId);

                // Add selected subjects if any
                if (model.SelectedSubjectIds != null && model.SelectedSubjectIds.Any())
                {
                    foreach (var subjectId in model.SelectedSubjectIds)
                    {
                        _repository.AddStudentSubject(new StudentSubject
                        {
                            StudentId = student.StudentId,
                            SubjectId = subjectId
                        });
                    }
                }

                _repository.Update(student);
                _repository.SaveChanges();

                _loggingService.Log("Edit Student", $"Student updated: {student.NRIC} - {student.Name}");
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.Log("Error - Edit Student", ex.Message, true);
                errorMessage = "An error occurred while saving. Please try again.";
                return false;
            }
        }

        public int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}

