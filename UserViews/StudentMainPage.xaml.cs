using LearningSystemGUI.MainMenuActions.CourseExplorer;
using LearningSystemGUI.MainMenuActions;
using LearningSystemGUI.MenuActions;
using LearningSystemGUI.UserOperations;
using Library.Danvas3.models;

namespace LearningSystemGUI.UserViews;

public partial class StudentMainPage : ContentPage
{

    private DataStorage dataStorage;
    private Person student;

    public StudentMainPage(DataStorage dataStorage, Person student)
    {
        InitializeComponent();
        this.dataStorage = dataStorage;
        this.student = student;
        StudentInfoLabel.Text = $"{student.Name} (ID: {student.ID})";
        DisplayCurrentSemesterCourses();
    }

    private void DisplayCurrentSemesterCourses()
    {
        var currentSemesterCourses = dataStorage.courses
            .Where(c => c.Roster.Contains(student) && c.IsCourseInCurrentSemester())
            .ToList();

        CurrentSemesterCoursesListView.ItemsSource = currentSemesterCourses;
    }
    private async void SwitchUserButton_Clicked(object sender, EventArgs e)
    {
        // Navigate to the UserSelectionPage
        await Navigation.PushAsync(new UserSelectionPage(dataStorage));
    }

    private async void CurrentSemesterCoursesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedCourse = e.SelectedItem as Course;
        if (selectedCourse != null)
        {
            await Navigation.PushModalAsync(new CourseStudentEnd(selectedCourse, dataStorage));
        }
        CurrentSemesterCoursesListView.SelectedItem = null;
    }

    private async void ViewPreviousSemesterCoursesButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PreviousSemesterCoursesPage(dataStorage, student));
    }


}