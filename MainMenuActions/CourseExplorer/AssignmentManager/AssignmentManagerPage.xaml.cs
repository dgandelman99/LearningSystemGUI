using Library.Danvas3.models;
using LearningSystemGUI.MainMenuActions.CourseExplorer.ModuleManager;
using LearningSystemGUI.MainMenuActions.CourseExplorer.AnnouncementManager;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer.AssignmentManager;

public partial class AssignmentManagerPage : ContentPage
{
    private Course selectedCourse;

    public AssignmentManagerPage(Course course)
    {
        InitializeComponent();
        selectedCourse = course;
    }
    private async void DisplayAssignmentsInfoButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DisplayAssignmentsInfoPage(selectedCourse));
    }

    private async void ModuleManagerButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ModuleManagerPage(selectedCourse));
    }

    private async void ReturnButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void ManageAnnouncementsButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AnnouncementsPage(selectedCourse));
    }

}