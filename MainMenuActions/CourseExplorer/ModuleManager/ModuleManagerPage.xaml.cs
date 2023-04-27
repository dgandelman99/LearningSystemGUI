using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer.ModuleManager;

public partial class ModuleManagerPage : ContentPage
{
    private Course _course;

    public ModuleManagerPage(Course course)
    {
        InitializeComponent();
        _course = course;
        LoadModules();
    }

    private async void CreateModuleButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateModulePage(_course));
    }

    private async void UpdateModuleButton_Clicked(object sender, EventArgs e)
    {
        var selectedModule = (Module)ModulesListView.SelectedItem;
        if (selectedModule != null)
        {
            await Navigation.PushAsync(new UpdateModulePage(_course, selectedModule));
        }
    }

    private async void DeleteModuleButton_Clicked(object sender, EventArgs e)
    {
        var selectedModule = (Module)ModulesListView.SelectedItem;
        if (selectedModule != null)
        {
            _course.Modules.Remove(selectedModule);
            ModulesListView.ItemsSource = null;
            ModulesListView.ItemsSource = _course.Modules;
        }
    }

    private async void BackButton_Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void RefreshButton_Clicked(object sender, EventArgs e)
    {
        LoadModules();
    }
    private void LoadModules()
    {
        ModulesListView.ItemsSource = null;
        ModulesListView.ItemsSource = _course.Modules;
    }
}