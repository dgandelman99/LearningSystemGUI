using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions;

public partial class UpdateStaffPage : ContentPage
{
    private readonly Person person;
    private readonly DataStorage dataStorage;

    public UpdateStaffPage(Person person, DataStorage dataStorage)
    {
        InitializeComponent();
        this.person = person;
        this.dataStorage = dataStorage;
        BindingContext = person;
        classificationPicker.SelectedIndex = (int)person.Classification;
    }

    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        person.Name = nameEntry.Text;
        person.Classification = (Classification)classificationPicker.SelectedIndex;
        dataStorage.UpdatePerson(person);
        await Navigation.PopAsync();
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
