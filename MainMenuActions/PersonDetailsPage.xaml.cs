using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions;

public partial class PersonDetailsPage : ContentPage
{
    private Person _person;
    private List<Course> _courses;

    public PersonDetailsPage(Person person, DataStorage dataStorage)
    {
        InitializeComponent();
        _person = person;
        _courses = dataStorage.courses;
        DisplayPersonDetails();
    }

    private void DisplayPersonDetails()
    {
        var enrolledCourses = _courses.Where(c => c.Roster.Contains(_person)).ToList();
        EnrolledClassesListView.ItemsSource = enrolledCourses;

        double totalGradePoints = 0;
        int totalCreditHours = 0;
        double gpa = 0;

        foreach (Course course in enrolledCourses)
        {
            totalCreditHours += course.CreditHours;
            totalGradePoints += course.GetWeightedAverage(_person);
        }

        gpa = totalGradePoints / totalCreditHours;
        GpaLabel.Text = $"GPA for Student [{_person.ID}] {_person.Name}: {gpa}";
    }

    private async void BackButton_Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }
}