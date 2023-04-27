using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer.AssignmentManager;

public partial class CreateGroupPage : ContentPage
{
    private Course _course;

    public CreateGroupPage(Course course)
    {
        InitializeComponent();
        _course = course;
    }

    private async void CreateGroupButton_Clicked(object sender, EventArgs e)
    {
        string name = GroupNameEntry.Text;
        double weight = double.Parse(GroupWeightEntry.Text);

        AssignmentGroup newGroup = new AssignmentGroup(name, weight);
        _course.AddAssignmentGroup(newGroup);

        await DisplayAlert("Success", $"Group [{newGroup.GroupId}] {newGroup.Name} has been added to {_course.Name}.", "OK");
        await Navigation.PopAsync();
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
