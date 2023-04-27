using LearningSystemGUI.MainMenuActions.CourseExplorer;
using Library.Danvas3.models;

namespace LearningSystemGUI.UserViews;

public partial class PreviousSemesterCoursesPage : ContentPage
{
    private DataStorage dataStorage;
    private Person user;

    public PreviousSemesterCoursesPage(DataStorage dataStorage, Person user)
    {
        InitializeComponent();
        this.dataStorage = dataStorage;
        this.user = user;

        DisplayPreviousSemesterCourses();
    }


    private void DisplayPreviousSemesterCourses()
    {
        var previousSemesterCourses = dataStorage.courses
            .Where(c => c.Roster.Contains(user) && !c.IsCourseInCurrentSemester())
            .ToList();

        PreviousSemesterCoursesListView.ItemsSource = previousSemesterCourses;
    }


    private async void PreviousSemesterCoursesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedCourse = e.SelectedItem as Course;
        if (selectedCourse != null)
        {
            if (user.Classification != Classification.TA && user.Classification != Classification.Instructor)
            {
                await Navigation.PushAsync(new CourseStudentEnd(selectedCourse, dataStorage));
            }
            else if (user.Classification == Classification.TA)
            {
                await Navigation.PushAsync(new CourseDetailsPage(selectedCourse, dataStorage));
            }
        }
        PreviousSemesterCoursesListView.SelectedItem = null;
    }

}