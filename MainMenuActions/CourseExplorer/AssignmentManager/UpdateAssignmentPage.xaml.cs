using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer.AssignmentManager;

public partial class UpdateAssignmentPage : ContentPage
{
    private Course _course;
    private Assignment _assignment;
    public List<AssignmentGroup> Groups => _course.AssignmentGroups;


    public UpdateAssignmentPage(Course course, Assignment assignment)
    {
        InitializeComponent();
        _course = course;
        _assignment = assignment;

        // Populate the GroupPicker with available assignment groups
        GroupPicker.ItemsSource = _course.AssignmentGroups;

        // Set the initial values of the Entry fields and DatePicker
        NameEntry.Text = _assignment.Name;
        DescriptionEntry.Text = _assignment.Description;
        TotalPointsEntry.Text = _assignment.TotalAvailablePoints.ToString();
        DueDatePicker.Date = _assignment.DueDate;

        // Set the initial value of the GroupPicker
        AssignmentGroup currentGroup = _course.GetGroupOfAssignment(_assignment);
        if (currentGroup != null)
        {
            GroupPicker.SelectedItem = currentGroup;
        }
    }

    private async void UpdateAssignmentButton_Clicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text;
        string description = DescriptionEntry.Text;
        int totalAvailablePoints = int.Parse(TotalPointsEntry.Text);
        DateTime dueDate = DueDatePicker.Date;

        _assignment.Name = name;
        _assignment.Description = description;
        _assignment.TotalAvailablePoints = totalAvailablePoints;
        _assignment.DueDate = dueDate;

        AssignmentGroup newGroup = (AssignmentGroup)GroupPicker.SelectedItem;
        AssignmentGroup oldGroup = _course.GetGroupOfAssignment(_assignment);

        if (oldGroup != newGroup)
        {
            oldGroup.RemoveAssignment(_assignment);
            newGroup.AddAssignment(_assignment);
        }

        await DisplayAlert("Success", $"{_assignment.Name} has been updated.", "OK");
        await Navigation.PopAsync();
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}
