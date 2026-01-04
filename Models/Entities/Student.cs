using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManagementSystem.Models.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string NRIC { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(1)]
        public string Gender { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        public DateTime? AvailableDate { get; set; }

        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }

}