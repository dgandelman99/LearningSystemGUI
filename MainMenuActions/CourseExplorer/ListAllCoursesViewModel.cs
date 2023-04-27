using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
//using Xamarin.Forms;



namespace LearningSystemGUI.MainMenuActions.CourseExplorer
{
    using Danvas3;
    using Library.Danvas3.models;
    using System.Collections.ObjectModel;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ListAllCoursesViewModel
    {
        public List<Course> Courses { get; set; }
        private ListNavigator<Course> navigator;
        public int CurrentPage => navigator.CurrentPage;
        public bool HasPreviousPage => navigator.HasPreviousPage;
        public bool HasNextPage => navigator.HasNextPage;

        public ListAllCoursesViewModel(DataStorage dataStorage)
        {
            navigator = new ListNavigator<Course>(dataStorage.courses);
            Courses = new List<Course>(navigator.GetCurrentPage().Values);

            if (dataStorage.TotalCourses > 0)
            {
                LoadPage(1);
            }
        }

        public void LoadPage(int pageNumber)
        {
            Dictionary<int, Course> page = navigator.GoToPage(pageNumber);
            Courses.Clear();

            foreach (var course in page.Values)
            {
                Courses.Add(course);
            }
        }
    }



}