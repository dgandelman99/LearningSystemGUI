using Danvas3;
using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer;


public partial class ListAllCoursesPage : ContentPage
{

    DataStorage dataStorage;
    ListNavigator<Course> courseNavigator;
    public ListAllCoursesPage(DataStorage dataStorage)
    {
        this.dataStorage = dataStorage;
        courseNavigator = new ListNavigator<Course>(dataStorage.courses);
        InitializeComponent();
        BindingContext = new ListAllCoursesViewModel(dataStorage);
    }


    private void PreviousButton_Clicked(object sender, EventArgs e)
    {
        var viewModel = (ListAllCoursesViewModel)BindingContext;
        if (viewModel.HasPreviousPage)
        {
            viewModel.LoadPage(viewModel.CurrentPage - 1);
            coursesListView.ItemsSource = null;
            coursesListView.ItemsSource = viewModel.Courses;
        }
    }

    private void NextButton_Clicked(object sender, EventArgs e)
    {
        var viewModel = (ListAllCoursesViewModel)BindingContext;
        if (viewModel.HasNextPage)
        {
            viewModel.LoadPage(viewModel.CurrentPage + 1);
            coursesListView.ItemsSource = null;
            coursesListView.ItemsSource = viewModel.Courses;
        }
    }


    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void CoursesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedCourse = (Course)e.SelectedItem;
        if (selectedCourse != null)
        {
            await Navigation.PushAsync(new CourseDetailsPage(selectedCourse, dataStorage));
            ((ListView)sender).SelectedItem = null;
        }
    }


}
