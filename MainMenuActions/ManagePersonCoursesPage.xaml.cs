using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions;

public partial class ManagePersonCoursesPage : ContentPage
{
    private DataStorage dataStorage;
    private Person selectedPerson;

    public ManagePersonCoursesPage(DataStorage dataStorage, Person selectedPerson)
    {
        InitializeComponent();
        this.dataStorage = dataStorage;
        this.selectedPerson = selectedPerson;
        BindingContext = this;
        enrolledCoursesListView.ItemsSource = GetEnrolledCourses(selectedPerson);
    }

    private async void AddToCourseButton_Clicked(object sender, EventArgs e)
    {
        string courseCode = courseCodeEntry.Text;
        var selectedCourse = dataStorage.courses.FirstOrDefault(c => c?.Code == courseCode);
        if (selectedCourse != null)
        {
            if (!selectedCourse.Roster.Contains(selectedPerson))
            {
                if (selectedPerson.Classification == Classification.Instructor || selectedPerson.Classification == Classification.TA)
                {
                    selectedCourse.Faculty.Add(selectedPerson);
                    await DisplayAlert("Success", $"{selectedPerson.Name} added to {selectedCourse.Name} faculty.", "OK");
                }
                else
                {
                    selectedCourse.Roster.Add(selectedPerson);
                    await DisplayAlert("Success", $"{selectedPerson.Name} added to {selectedCourse.Name} roster.", "OK");
                }
            }
            else
            {
                DisplayAlert("Error", "The selected person is already enrolled in this course.", "OK");
            }
        }
        else
        {
            DisplayAlert("Error", "Course not found.", "OK");
        }
    }

    private void RemoveButton_Clicked(object sender, EventArgs e)
    {
        string courseCode = courseCodeEntry.Text;
        var selectedCourse = dataStorage.courses.FirstOrDefault(c => c?.Code == courseCode);
        if (selectedCourse != null)
        {
            if (selectedCourse.Roster.Contains(selectedPerson))
            {
                selectedCourse.Roster.Remove(selectedPerson);
                DisplayAlert("Success", $"{selectedPerson.Name} removed from {selectedCourse.Name} roster.", "OK");
            }
            else
            {
                DisplayAlert("Error", "The selected person is not enrolled in this course.", "OK");
            }
        }
        else
        {
            DisplayAlert("Error", "Course not found.", "OK");
        }
    }

    private async void RemoveFromCourseButton_Clicked(object sender, EventArgs e)
    {
        var selectedCourse = (Course)enrolledCoursesListView.SelectedItem;

        if (selectedCourse == null)
        {
            await DisplayAlert("Error", "No course selected", "OK");
            return;
        }

        if (selectedCourse.Roster.Contains(selectedPerson))
        {
            selectedCourse.Roster.Remove(selectedPerson);
            await DisplayAlert("Success", $"{selectedPerson.Name} has been removed from {selectedCourse.Name}", "OK");
            enrolledCoursesListView.ItemsSource = null;
            enrolledCoursesListView.ItemsSource = GetEnrolledCourses(selectedPerson);
        }
        else
        {
            await DisplayAlert("Error", $"{selectedPerson.Name} is not enrolled in {selectedCourse.Name}", "OK");
        }
    }

    private List<Course> GetEnrolledCourses(Person person)
    {
        return dataStorage.courses.FindAll(course => course.Roster.Contains(person));
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}