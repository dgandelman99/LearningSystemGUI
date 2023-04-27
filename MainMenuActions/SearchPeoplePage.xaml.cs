using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions
{
    public partial class SearchPeoplePage : ContentPage
    {
        private DataStorage dataStorage;

        public SearchPeoplePage(DataStorage dataStorage)
        {
            InitializeComponent();
            this.dataStorage = dataStorage;
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            string query = searchQueryEntry.Text.ToLower() ?? string.Empty;
            List<Person> results = dataStorage.people.FindAll(p => p.Name.ToLower().Contains(query));
            searchResultsListView.ItemsSource = results;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ManageCoursesButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedPerson = button.CommandParameter as Person;
            if (selectedPerson != null)
            {
                await Navigation.PushAsync(new ManagePersonCoursesPage(dataStorage, selectedPerson));
            }
        }
    }
}
