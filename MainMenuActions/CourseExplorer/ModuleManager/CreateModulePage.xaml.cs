using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer.ModuleManager;

public partial class CreateModulePage : ContentPage
{
    private Course _course;

    public CreateModulePage(Course course)
    {
        InitializeComponent();
        _course = course;
    }

    private async void CreateModuleButton_Clicked(object sender, EventArgs e)
    {
        Module module = new Module
        {
            Name = NameEntry.Text,
            Description = DescriptionEditor.Text
        };
        _course.Modules.Add(module);
        await DisplayAlert("Success", $"Module '{module.Name}' has been created.", "OK");
        await Navigation.PopAsync();
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}
