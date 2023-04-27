using LearningSystemGUI.MainMenuActions.CourseExplorer;
using LearningSystemGUI.MainMenuActions;
using LearningSystemGUI.MenuActions;
using LearningSystemGUI.UserOperations;
using Library.Danvas3.models;

namespace LearningSystemGUI.UserViews;

public partial class TAMainPage : ContentPage
{
    private DataStorage dataStorage;
    private Person ta;

    public TAMainPage(DataStorage dataStorage, Person ta)
    {
        InitializeComponent();
        this.dataStorage = dataStorage;
        this.ta = ta;

        TaInfoLabel.Text = $"TA: {ta.Name}, EmplID: {ta.ID}";

        DisplayCurrentSemesterCourses();
    }

    private void DisplayCurrentSemesterCourses()
    {
        var currentSemesterCourses = dataStorage.courses
            .Where(c => c.Faculty.Contains(ta) && c.IsCourseInCurrentSemester())
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
            await Navigation.PushAsync(new CourseDetailsPage(selectedCourse, dataStorage));
        }
        CurrentSemesterCoursesListView.SelectedItem = null;
    }

    private async void PreviousSemesterCoursesButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PreviousSemesterCoursesPage(dataStorage, ta));
    }

}