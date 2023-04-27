using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer;

public partial class UpdateCoursePage : ContentPage
{
    private readonly Course course;
    private readonly DataStorage dataStorage;

    public UpdateCoursePage(Course course, DataStorage dataStorage)
    {
        InitializeComponent();
        this.course = course;
        this.dataStorage = dataStorage;
        PreFillCourseDetails();
    }

    private void PreFillCourseDetails()
    {
        codeEntry.Text = course.Code;
        nameEntry.Text = course.Name;
        descriptionEditor.Text = course.Description;
        creditHoursEntry.Text = course.CreditHours.ToString();
        roomLocationEntry.Text = course.RoomLocation;
        startDatePicker.Date = course.StartDate;
    }

    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        course.Name = nameEntry.Text ?? string.Empty;
        course.Description = descriptionEditor.Text ?? string.Empty;
        course.CreditHours = int.Parse(creditHoursEntry.Text);
        course.RoomLocation = roomLocationEntry.Text ?? string.Empty;
        course.StartDate = startDatePicker.Date;

        dataStorage.UpdateCourse(course);

        await DisplayAlert("Success", "Course updated successfully.", "OK");
        await Navigation.PopAsync();
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}