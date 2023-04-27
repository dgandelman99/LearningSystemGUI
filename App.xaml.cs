
using LearningSystemGUI.UserViews;

namespace LearningSystemGUI
{
    public partial class App : Application
    {
        private DataStorage dataStorage;
        public App()
        {
            InitializeComponent();
            dataStorage = new DataStorage();

            MainPage = new NavigationPage(new MainPage(dataStorage));
        }
    }
}
