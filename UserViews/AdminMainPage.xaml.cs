using LearningSystemGUI.MainMenuActions.CourseExplorer;
using LearningSystemGUI.MainMenuActions;
using LearningSystemGUI.MenuActions;
using LearningSystemGUI.UserOperations;

namespace LearningSystemGUI.UserViews;

public partial class AdminMainPage : ContentPage
{
    private readonly DataStorage dataStorage;

    public AdminMainPage(DataStorage _dataStorage)
    {
        InitializeComponent();

        dataStorage = _dataStorage;
        // Instantiate DataStorage and populate the course list
        //dataStorage = new DataStorage();
    }

    private async void CreateCourse_Clicked(object sender, EventArgs e)
    {
        var createCoursePage = new CreateCoursePage(dataStorage);
        await Navigation.PushModalAsync(createCoursePage);
    }

    private async void CreateStudent_Clicked(object sender, EventArgs e)
    {
        var createStudentPage = new CreateStudentPage(dataStorage);
        await Navigation.PushModalAsync(createStudentPage);
    }
    /*
    private async void AddStudentToCourse_Clicked(object sender, EventArgs e)
    {
        var addStudentToCoursePage = new AddStudentToCoursePage(dataStorage);
        await Navigation.PushModalAsync(addStudentToCoursePage);
    }

    private async void RemoveStudentFromCourse_Clicked(object sender, EventArgs e)
    {
        var removeStudentFromCoursePage = new RemoveStudentFromCoursePage(dataStorage);
        await Navigation.PushModalAsync(removeStudentFromCoursePage);
    } */

    private async void ListAllCoursesButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListAllCoursesPage(dataStorage));
    }

    private async void SearchCoursesButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchCoursesPage(dataStorage));
    }

    private async void ListAllPeopleButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListAllPeoplePage(dataStorage));
    }

    private async void SearchPeopleButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainMenuActions.SearchPeoplePage(dataStorage));
    }

    private async void CreateStaffButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddStaffPage(dataStorage));
    }

    private async void ListAllStaffButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListAllStaffPage(dataStorage));
    }

    private async void SwitchUserButton_Clicked(object sender, EventArgs e)
    {
        // Navigate to the UserSelectionPage
        await Navigation.PushAsync(new UserSelectionPage(dataStorage));
    }


}