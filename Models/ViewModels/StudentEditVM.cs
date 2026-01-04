using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentManagementSystem.Models.ViewModels
{
    public class StudentEditVM
    {
        public int StudentId { get; set; }
        
        [Required(ErrorMessage = "NRIC is required")]
        [Display(Name = "NRIC")]
        public string NRIC { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        
        [Required(ErrorMessage = "Birthday is required")]
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [Display(Name = "Available Date")]
        [DataType(DataType.Date)]
        public DateTime? AvailableDate { get; set; }

        public List<int> SelectedSubjectIds { get; set; }
        public List<SelectListItem> Subjects { get; set; }
        
        public StudentEditVM()
        {
            SelectedSubjectIds = new List<int>();
            Subjects = new List<SelectListItem>();
        }
    }

}