//using Android.Telecom;
using LearningSystemGUI.MainMenuActions.CourseExplorer.AssignmentManager;
using Library.Danvas3.models;
using System.Collections.ObjectModel;
//using static Android.Graphics.ImageDecoder;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CourseDetailsPage : ContentPage
{
    Course course;
    DataStorage dataStorage;
    public CourseDetailsPage(Course selectedCourse, DataStorage dataStorage)
    {
        InitializeComponent();

        course = selectedCourse;
        this.dataStorage = dataStorage;
        BindingContext = course;

        DisplayCourseDetails();
    }


    private void DisplayCourseDetails()
    {
        CourseName.Text = course.Name;
        CourseCode.Text = course.Code;
        SemesterLabel.Text = $"Semester: {course.GetSemester()}";
        RoomLocation.Text = course.RoomLocation;
        CourseDescription.Text = course.Description;

        EnrolledStaffListView.ItemsSource = course.Faculty.ToList();
        EnrolledStudentsListView.ItemsSource = course.Roster.ToList();
    }

    private async void UpdateCourseButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UpdateCoursePage(course, dataStorage));
    }

    private async void BackButton_Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void RefreshButton_Clicked(object sender, EventArgs e)
    {
        DisplayCourseDetails();
    }

    private async void DeleteCourseButton_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm", $"Are you sure you want to delete {course.Name}?", "Yes", "No");
        if (confirm)
        {
            dataStorage.DeleteCourse(course);
            await Navigation.PopAsync(); // Go back to the previous page
        }
    }

    private async void AssignmentManagerButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AssignmentManagerPage(course));
    }


}
