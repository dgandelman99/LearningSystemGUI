using LearningSystemGUI.MainMenuActions.CourseExplorer;
using LearningSystemGUI.MainMenuActions;
using LearningSystemGUI.MenuActions;
using LearningSystemGUI.UserOperations;
using Library.Danvas3.models;

namespace LearningSystemGUI.UserViews;

public partial class InstructorMainPage : ContentPage
{
    private DataStorage dataStorage;
    private Person instructor;

    public InstructorMainPage(DataStorage dataStorage, Person instructor)
    {
        InitializeComponent();
        this.dataStorage = dataStorage;
        this.instructor = instructor;
        InstructorInfoLabel.Text = $"{instructor.Name} (EmplID: {instructor.ID})";
        DisplayInstructorCourses();
    }

    private void DisplayInstructorCourses()
    {
        var instructorCourses = dataStorage.courses
            .Where(c => c.Faculty.Contains(instructor))
            .ToList();

        InstructorCoursesListView.ItemsSource = instructorCourses;
    }

    private async void SwitchUserButton_Clicked(object sender, EventArgs e)
    {
        // Navigate to the UserSelectionPage
        await Navigation.PushAsync(new UserSelectionPage(dataStorage));
    }

    private async void InstructorCoursesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedCourse = e.SelectedItem as Course;
        if (selectedCourse != null)
        {
            await Navigation.PushAsync(new CourseDetailsPage(selectedCourse, dataStorage));
        }
        InstructorCoursesListView.SelectedItem = null;
    }
}