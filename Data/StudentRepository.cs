using StudentManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace StudentManagementSystem.Data
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        public IEnumerable<Student> GetAllWithSubjects()
        {
            return _context.Students
                .Include(s => s.StudentSubjects)
                .ToList();
        }

        public IEnumerable<Student> Find(Expression<Func<Student, bool>> predicate)
        {
            return _context.Students.Where(predicate).ToList();
        }

        public IEnumerable<Student> FindWithSubjects(Expression<Func<Student, bool>> predicate)
        {
            return _context.Students
                .Include(s => s.StudentSubjects)
                .Where(predicate)
                .ToList();
        }

        public IEnumerable<Student> GetAllWithSubjectsPaged(int pageNumber, int pageSize)
        {
            return _context.Students
                .Include(s => s.StudentSubjects)
                .OrderBy(s => s.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<Student> FindWithSubjectsPaged(Expression<Func<Student, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.Students
                .Include(s => s.StudentSubjects)
                .Where(predicate)
                .OrderBy(s => s.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int Count()
        {
            return _context.Students.Count();
        }

        public int Count(Expression<Func<Student, bool>> predicate)
        {
            return _context.Students.Count(predicate);
        }

        public Student GetById(int id)
        {
            return _context.Students.Find(id);
        }

        public Student GetByNRIC(string nric)
        {
            return _context.Students.FirstOrDefault(s => s.NRIC == nric);
        }

        public Student GetByIdWithSubjects(int id)
        {
            return _context.Students
                .Include(s => s.StudentSubjects)
                .FirstOrDefault(s => s.StudentId == id);
        }

        public Student GetByNRICWithSubjects(string nric)
        {
            return _context.Students
                .Include(s => s.StudentSubjects)
                .FirstOrDefault(s => s.NRIC == nric);
        }

        public bool Exists(string nric)
        {
            return _context.Students.Any(s => s.NRIC == nric);
        }

        public bool Exists(int id)
        {
            return _context.Students.Any(s => s.StudentId == id);
        }

        public void Add(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            _context.Students.Add(student);
        }

        public void Update(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            _context.Entry(student).State = EntityState.Modified;
        }

        public void Delete(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            _context.Students.Remove(student);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Subject> GetAllSubjects()
        {
            return _context.Subjects.ToList();
        }

        public Subject GetSubjectById(int id)
        {
            return _context.Subjects.Find(id);
        }

        public void AddStudentSubject(StudentSubject studentSubject)
        {
            if (studentSubject == null)
                throw new ArgumentNullException(nameof(studentSubject));

            _context.StudentSubjects.Add(studentSubject);
        }

        public void RemoveStudentSubjects(int studentId)
        {
            var studentSubjects = _context.StudentSubjects
                .Where(ss => ss.StudentId == studentId)
                .ToList();

            _context.StudentSubjects.RemoveRange(studentSubjects);
        }

        public IEnumerable<StudentSubject> GetStudentSubjects(int studentId)
        {
            return _context.StudentSubjects
                .Where(ss => ss.StudentId == studentId)
                .ToList();
        }
    }
}

