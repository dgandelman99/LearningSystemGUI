using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer.AssignmentManager;

public partial class UpdateAssignmentGroupPage : ContentPage
{
    private Course _course;
    private AssignmentGroup _assignmentGroup;

    public UpdateAssignmentGroupPage(Course course, AssignmentGroup assignmentGroup)
    {
        InitializeComponent();
        _course = course;
        _assignmentGroup = assignmentGroup;
        PopulateFields();
    }

    private void PopulateFields()
    {
        NameEntry.Text = _assignmentGroup.Name;
        WeightEntry.Text = _assignmentGroup.Weight.ToString();
    }

    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        string newName = NameEntry.Text;
        double newWeight = double.Parse(WeightEntry.Text);

        _assignmentGroup.Name = newName;
        _assignmentGroup.Weight = newWeight;

        await DisplayAlert("Success", "Assignment group updated.", "OK");
        await Navigation.PopAsync();
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}