using Danvas3;
using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions;

public partial class ListAllStaffPage : ContentPage
{
    private DataStorage dataStorage;
    private ListNavigator<Person> navigator;

    public ListAllStaffPage(DataStorage dataStorage)
    {
        InitializeComponent();
        this.dataStorage = dataStorage;

        // Filter the list of people to only include staff (Instructors and TAs)
        var staff = dataStorage.people.Where(p => p?.Classification == Classification.Instructor || p?.Classification == Classification.TA)?.ToList();

        navigator = new ListNavigator<Person>(staff);
        if (staff.Count > 0)
        {
            peopleListView.ItemsSource = navigator.GetCurrentPage();
        }
        else
        {
            peopleListView.ItemsSource = new List<Person>(); // Empty list
        }
        UpdatePeopleList();
    }

    private void UpdatePeopleList()
    {
        peopleListView.ItemsSource = navigator.GetCurrentPage().Values;
        pageNumberLabel.Text = $"Page {navigator.CurrentPage} of {navigator.LastPage}";
        previousButton.IsEnabled = navigator.HasPreviousPage;
        nextButton.IsEnabled = navigator.HasNextPage;
    }

    private void PreviousButton_Clicked(object sender, EventArgs e)
    {
        navigator.GoBackward();
        UpdatePeopleList();
    }

    private void NextButton_Clicked(object sender, EventArgs e)
    {
        navigator.GoForward();
        UpdatePeopleList();
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void ManageCoursesButton_Clicked(object sender, EventArgs e)
    {
        var selectedPerson = (Person)peopleListView.SelectedItem;
        if (selectedPerson != null)
        {
            await Navigation.PushAsync(new ManagePersonCoursesPage(dataStorage, selectedPerson));
        }
        else
        {
            await DisplayAlert("Error", "Please select a valid person to manage their courses.", "OK");
        }
    }

    private async void UpdatePersonButton_Clicked(object sender, EventArgs e)
    {
        var selectedPerson = (Person)peopleListView.SelectedItem;
        if (selectedPerson != null)
        {
            await Navigation.PushAsync(new UpdateStaffPage(selectedPerson, dataStorage));
        }
        else
        {
            await DisplayAlert("Error", "Please select a valid person to update.", "OK");
        }
    }

    private async void DeletePersonButton_Clicked(object sender, EventArgs e)
    {
        var selectedPerson = (Person)peopleListView.SelectedItem;
        if (selectedPerson != null)
        {
            bool confirm = await DisplayAlert("Confirm", $"Are you sure you want to delete {selectedPerson.Name}?", "Yes", "No");
            if (confirm)
            {
                dataStorage.DeletePerson(selectedPerson);
                peopleListView.ItemsSource = dataStorage.people;
            }
        }
        else
        {
            await DisplayAlert("Error", "Please select a person to delete.", "OK");
        }
    }



}