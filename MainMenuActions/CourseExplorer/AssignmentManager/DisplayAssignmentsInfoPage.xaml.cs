using Library.Danvas3.models;
namespace LearningSystemGUI.MainMenuActions.CourseExplorer.AssignmentManager;

public partial class DisplayAssignmentsInfoPage : ContentPage
{
    private Course _course;

    public DisplayAssignmentsInfoPage(Course course)
    {
        InitializeComponent();
        _course = course;
        DisplayAssignmentsInfo();
    }

    private void DisplayAssignmentsInfo()
    {
        var assignmentDisplayItems = new List<AssignmentDisplayItem>();

        foreach (var group in _course.AssignmentGroups)
        {
            if (group.Name != null)
            {
                assignmentDisplayItems.Add(new AssignmentDisplayItem
                {
                    GroupHeader = $"---Group [ID: {group.GroupId}]: {group.Name} (Weight: {group.Weight})----",
                    IsGroupHeader = true,
                    GroupId = group.GroupId
                });
            }

            foreach (var assignment in group.Assignments)
            {
                assignmentDisplayItems.Add(new AssignmentDisplayItem
                {
                    AssignmentInfo = $"[{assignment.AssignmentId}] {assignment.Name}: {assignment.Grades.Count} submissions",
                    IsAssignment = true,
                    AssignmentId = assignment.AssignmentId
                });
            }
        }

        AssignmentsInfoListView.ItemsSource = assignmentDisplayItems;
    }


    private async void CreateAssignmentButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateAssignmentPage(_course));
    }

    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        var selectedItem = (AssignmentDisplayItem)AssignmentsInfoListView.SelectedItem;

        if (selectedItem == null)
        {
            await DisplayAlert("Error", "Please select an assignment or group to update.", "OK");
            return;
        }

        if (selectedItem.IsAssignment)
        {
            var selectedAssignment = _course.GetAssignmentById(selectedItem.AssignmentId);
            await Navigation.PushAsync(new UpdateAssignmentPage(_course, selectedAssignment));
        }
        else if (selectedItem.IsGroupHeader)
        {
            var selectedGroup = _course.GetAssignmentGroupById(selectedItem.GroupId);
            await Navigation.PushAsync(new UpdateAssignmentGroupPage(_course, selectedGroup));
        }
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        var selectedItem = (AssignmentDisplayItem)AssignmentsInfoListView.SelectedItem;

        if (selectedItem == null)
        {
            await DisplayAlert("Error", "Please select an assignment or group to delete.", "OK");
            return;
        }

        if (selectedItem.IsAssignment)
        {
            var selectedAssignment = _course.GetAssignmentById(selectedItem.AssignmentId);
            bool confirmed = await DisplayAlert("Confirmation", $"Are you sure you want to delete the assignment '{selectedAssignment.Name}'?", "Yes", "No");

            if (confirmed)
            {
                _course.DeleteAssignment(selectedAssignment);
                DisplayAssignmentsInfo();
            }
        }
        else if (selectedItem.IsGroupHeader)
        {
            var selectedGroup = _course.GetAssignmentGroupById(selectedItem.GroupId);
            bool confirmed = await DisplayAlert("Confirmation", $"Are you sure you want to delete the group '{selectedGroup.Name}'?", "Yes", "No");

            if (confirmed)
            {
                _course.DeleteAssignmentGroup(selectedGroup);
                DisplayAssignmentsInfo();
            }
        }
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private void RefreshButton_Clicked(object sender, EventArgs e)
    {
        DisplayAssignmentsInfo();
    }

    private async void CreateGroupButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateGroupPage(_course));
    }

    private async void GradeAssignmentButton_Clicked(object sender, EventArgs e)
    {
        if (AssignmentsInfoListView.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select an assignment to grade.", "OK");
            return;
        }

        var selectedAssignment = (AssignmentDisplayItem)AssignmentsInfoListView.SelectedItem;
        var assignment = _course.GetAssignmentById(selectedAssignment.AssignmentId);

        int studentId = await GetStudentIdAsync();
        if (studentId == -1)
        {
            await DisplayAlert("Error", "Invalid student ID.", "OK");
            return;
        }

        Person student = _course.Roster.FirstOrDefault(s => s?.ID == studentId);
        if (student == null)
        {
            await DisplayAlert("Error", "Student not found in this course.", "OK");
            return;
        }

        int grade = await GetGradeAsync();
        if (grade == -1)
        {
            await DisplayAlert("Error", "Invalid grade.", "OK");
            return;
        }

        if (assignment.Grades.ContainsKey(student))
        {
            assignment.Grades[student] = grade;
        }
        else
        {
            assignment.Grades.Add(student, grade);
        }

        await DisplayAlert("Success", $"Grade of {grade} assigned to {student.Name} for assignment {assignment.Name} ({assignment.AssignmentId}) in course {_course.Name}", "OK");
    }

    private async Task<int> GetStudentIdAsync()
    {
        string result = await DisplayPromptAsync("Enter Student ID", "Please enter the student ID:");
        if (int.TryParse(result, out int studentId))
        {
            return studentId;
        }
        return -1;
    }

    private async Task<int> GetGradeAsync()
    {
        string result = await DisplayPromptAsync("Enter Grade", "Please enter the grade:");
        if (int.TryParse(result, out int grade))
        {
            return grade;
        }
        return -1;
    }
}


public class AssignmentDisplayItem
{
    public string GroupHeader { get; set; }
    public string AssignmentInfo { get; set; }
    public bool IsGroupHeader { get; set; }
    public bool IsAssignment { get; set; }
    public AssignmentGroup Group { get; set; }
    public Assignment Assignment { get; set; }
    public int AssignmentId { get; set; }
    public int GroupId { get; set; }

}
