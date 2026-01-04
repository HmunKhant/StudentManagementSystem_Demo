using StudentManagementSystem.Models.Entities;
using StudentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StudentManagementSystem.Data
{
    public interface IStudentRepository
    {
        // Student operations
        IEnumerable<Student> GetAll();
        IEnumerable<Student> GetAllWithSubjects();
        IEnumerable<Student> Find(Expression<Func<Student, bool>> predicate);
        IEnumerable<Student> FindWithSubjects(Expression<Func<Student, bool>> predicate);
        IEnumerable<Student> GetAllWithSubjectsPaged(int pageNumber, int pageSize);
        IEnumerable<Student> FindWithSubjectsPaged(Expression<Func<Student, bool>> predicate, int pageNumber, int pageSize);
        int Count();
        int Count(Expression<Func<Student, bool>> predicate);
        Student GetById(int id);
        Student GetByNRIC(string nric);
        Student GetByIdWithSubjects(int id);
        Student GetByNRICWithSubjects(string nric);
        bool Exists(string nric);
        bool Exists(int id);
        void Add(Student student);
        void Update(Student student);
        void Delete(Student student);
        void SaveChanges();

        // Subject operations
        IEnumerable<Subject> GetAllSubjects();
        Subject GetSubjectById(int id);

        // StudentSubject operations
        void AddStudentSubject(StudentSubject studentSubject);
        void RemoveStudentSubjects(int studentId);
        IEnumerable<StudentSubject> GetStudentSubjects(int studentId);
    }
}

