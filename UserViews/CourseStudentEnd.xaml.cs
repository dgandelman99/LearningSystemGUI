using Library.Danvas3.models;

namespace LearningSystemGUI.UserViews;

public partial class CourseStudentEnd : ContentPage
{
    private Course course;
    private DataStorage dataStorage;

    public CourseStudentEnd(Course course, DataStorage dataStorage)
    {
        InitializeComponent();
        this.course = course;
        this.dataStorage = dataStorage;

        DisplayCourseInformation();
        DisplayAnnouncements();
        DisplayModules();
        DisplayAssignments();
    }

    private void DisplayCourseInformation()
    {
        CourseName.Text = course.Name;
        CourseCode.Text = course.Code;
        SemesterLabel.Text = $"Semester: {course.GetSemester()}";
        RoomLocation.Text = course.RoomLocation;
        CourseDescription.Text = course.Description;
    }

    private void DisplayAnnouncements()
    {
        // Assuming you have a course.Announcements list
        AnnouncementsListView.ItemsSource = course.Announcements;
    }

    private void DisplayModules()
    {
        // Assuming you have a course.Modules list
        ModulesListView.ItemsSource = course.Modules;
    }

    private void DisplayAssignments()
    {
        var assignments = course.AssignmentGroups.SelectMany(ag => ag.Assignments).ToList();
        AssignmentsListView.ItemsSource = assignments;
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }


}