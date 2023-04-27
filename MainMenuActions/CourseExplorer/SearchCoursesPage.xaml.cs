using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer;

public partial class SearchCoursesPage : ContentPage
{
    private DataStorage dataStorage;

    public SearchCoursesPage(DataStorage dataStorage)
    {
        InitializeComponent();
        this.dataStorage = dataStorage;
    }

    private void SearchButton_Clicked(object sender, System.EventArgs e)
    {
        string query = searchQueryEntry.Text.ToLower() ?? string.Empty;
        List<Course> results = dataStorage.courses.FindAll(c => c.Name.ToLower().Contains(query) || c.Description.ToLower().Contains(query));
        searchResultsListView.ItemsSource = results;
    }

    private async void BackButton_Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void CoursesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            return;
        }

        var selectedCourse = e.SelectedItem as Course;
        await Navigation.PushAsync(new CourseDetailsPage(selectedCourse, dataStorage));
        ((ListView)sender).SelectedItem = null;
    }

}