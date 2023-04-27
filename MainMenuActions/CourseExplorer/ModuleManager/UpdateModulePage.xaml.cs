using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer.ModuleManager;

public partial class UpdateModulePage : ContentPage
{
    private Course _course;
    private Module _module;

    public UpdateModulePage(Course course, Module module)
    {
        InitializeComponent();
        _course = course;
        _module = module;

        NameEntry.Text = _module.Name;
        DescriptionEditor.Text = _module.Description;
    }

    private async void UpdateModuleButton_Clicked(object sender, EventArgs e)
    {
        _module.Name = NameEntry.Text;
        _module.Description = DescriptionEditor.Text;

        await DisplayAlert("Success", $"Module '{_module.Name}' has been updated.", "OK");
        await Navigation.PopAsync();
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}
