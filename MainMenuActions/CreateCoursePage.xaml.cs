using Library.Danvas3.models;

namespace LearningSystemGUI.MenuActions;

public partial class CreateCoursePage : ContentPage
{
    private readonly DataStorage dataStorage;
    public CreateCoursePage(DataStorage dataStorage)
	{
		InitializeComponent();
        this.dataStorage = dataStorage;
    }

    private async void CreateCourseButton_Clicked(object sender, EventArgs e)
    {
        string code = courseCodeEntry.Text?.ToUpper().Replace(" ", "") ?? string.Empty;
        if (!dataStorage.IsCodeUnique(code))
        {
            await DisplayAlert("Error", "Course code already in use. Try again.", "OK");
            return;
        }
        string name = courseNameEntry.Text ?? string.Empty;
        int credits = int.Parse(courseCreditHoursEntry.Text ?? "0");
        string description = courseDescriptionEntry.Text ?? string.Empty;

        Course course = new Course
        {
            Code = code,
            Name = name,
            Description = description,
            CreditHours = credits
        };

        dataStorage.courses.Add(course);

        string defname = "Unweighted";
        int defweight = 1;
        AssignmentGroup unweightedGroup = new AssignmentGroup(defname, defweight);
        course.AddAssignmentGroup(unweightedGroup);

        await DisplayAlert("Success", "Course created successfully!", "OK");
        await Navigation.PopModalAsync();
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }


}