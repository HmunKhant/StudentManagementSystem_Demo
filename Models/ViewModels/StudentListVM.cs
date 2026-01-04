using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystem.Models.ViewModels
{
    public class StudentListVM
    {
        public int SerialNo { get; set; }
        public string NRIC { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int NoOfSubjects { get; set; }
    }

}