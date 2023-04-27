

using LearningSystemGUI.UserViews;
using Library.Danvas3.models;

namespace LearningSystemGUI.UserOperations;

public partial class UserSelectionPage : ContentPage
{
    private DataStorage _dataStorage;

    public UserSelectionPage(DataStorage dataStorage)
    {
        InitializeComponent();
        _dataStorage = dataStorage;
    }

    private async void StudentButton_Clicked(object sender, EventArgs e)
    {
        string studentIdInput = await DisplayPromptAsync("Student ID", "Please enter your student ID:");

        if (!string.IsNullOrEmpty(studentIdInput))
        {
            if (int.TryParse(studentIdInput, out int parsedStudentId))
            {
                var selectedStudent = _dataStorage.people.FirstOrDefault(p => p.Classification != Classification.Instructor && p.Classification != Classification.TA && p.ID == parsedStudentId);

                if (selectedStudent != null)
                {
                    await Navigation.PushAsync(new StudentMainPage(_dataStorage, selectedStudent));
                }
                else
                {
                    await DisplayAlert("Error", "Invalid student ID. Please try again.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid student ID.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Please enter a valid student ID.", "OK");
        }
    }


    private async void TAButton_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("TA EmplID", "Please enter the TA's EmplID:");

        if (!string.IsNullOrEmpty(result))
        {
            int.TryParse(result, out int emplIdInput);
            var ta = _dataStorage.people.FirstOrDefault(p => p.emplID == emplIdInput && p.Classification == Classification.TA);

            if (ta != null)
            {
                await Navigation.PushAsync(new TAMainPage(_dataStorage, ta));
            }
            else
            {
                await DisplayAlert("Error", "Invalid EmplID or the person is not a TA.", "OK");
            }
        }
    }

    private async void InstructorButton_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Instructor EmplID", "Please enter the Instructor's EmplID:");

        if (!string.IsNullOrEmpty(result))
        {
            int.TryParse(result, out int emplIdInput);
            var instructor = _dataStorage.people.FirstOrDefault(p => p.emplID == emplIdInput && p.Classification == Classification.Instructor);

            if (instructor != null)
            {
                await Navigation.PushAsync(new InstructorMainPage(_dataStorage, instructor));
            }
            else
            {
                await DisplayAlert("Error", "Invalid EmplID or the person is not an instructor.", "OK");
            }
        }
    }

    private async void AdminButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminMainPage(_dataStorage));
    }
}
