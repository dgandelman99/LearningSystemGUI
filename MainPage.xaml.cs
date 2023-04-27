using LearningSystemGUI.MainMenuActions;
using LearningSystemGUI.MainMenuActions.CourseExplorer;
using LearningSystemGUI.MenuActions;
using LearningSystemGUI.UserOperations;

namespace LearningSystemGUI
{

    public partial class MainPage : ContentPage
    {

        private DataStorage dataStorage;
        public MainPage(DataStorage _dataStorage)
        {
            InitializeComponent();
            dataStorage = _dataStorage;
        }

        private async void SwitchUserButton_Clicked(object sender, EventArgs e)
        {
            // Navigate to the UserSelectionPage
            await Navigation.PushAsync(new UserSelectionPage(dataStorage));
        }
    }

}