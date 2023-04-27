using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer.AssignmentManager;

public partial class CreateAssignmentPage : ContentPage
{
    private Course _course;

    public CreateAssignmentPage(Course course)
    {
        InitializeComponent();
        _course = course;
    }

    private async void CreateAssignmentButton_Clicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text;
        string description = DescriptionEntry.Text;
        int totalPoints = int.Parse(TotalPointsEntry.Text);
        DateTime dueDate = DueDatePicker.Date;

        Assignment assignment = new Assignment(name, description, totalPoints, dueDate);

        // automatically assign to default unweighted group
        string defaultGroupName = "Unweighted";
        AssignmentGroup defaultGroup = _course.AssignmentGroups.Find(g => g?.Name == defaultGroupName);

        if (defaultGroup != null)
        {
            defaultGroup.Assignments.Add(assignment);
        }
        else
        {
            defaultGroup = new AssignmentGroup(defaultGroupName, 0);
            defaultGroup.Assignments.Add(assignment);
            _course.AssignmentGroups.Add(defaultGroup);
        }

        await DisplayAlert("Success", $"{assignment.Name} has been added to {_course.Name}.", "OK");
        await Navigation.PopAsync();
    }
    private async void BackButton_Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }

}