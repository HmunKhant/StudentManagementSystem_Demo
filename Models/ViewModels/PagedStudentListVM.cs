using System.Collections.Generic;

namespace StudentManagementSystem.Models.ViewModels
{
    public class PagedStudentListVM
    {
        public List<StudentListVM> Students { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int StartRecord => (CurrentPage - 1) * PageSize + 1;
        public int EndRecord => CurrentPage * PageSize > TotalRecords ? TotalRecords : CurrentPage * PageSize;
    }
}

