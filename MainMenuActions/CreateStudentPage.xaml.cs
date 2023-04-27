using Library.Danvas3.models;

namespace LearningSystemGUI.MenuActions;

public partial class CreateStudentPage : ContentPage
{
    private readonly DataStorage dataStorage;
    public CreateStudentPage(DataStorage dataStorage)
	{
		InitializeComponent();
        this.dataStorage = dataStorage;
    }

    private async void CreateStudentButton_Clicked(object sender, EventArgs e)
    {
        string name = studentNameEntry.Text ?? string.Empty;
        string classificationString = classificationPicker.SelectedItem?.ToString() ?? string.Empty;

        try
        {
            Classification classification = Person.ConvertStringToClassification(classificationString);
            Person student = new Person(name, classification);
            dataStorage.people.Add(student);
            await DisplayAlert("Success", "Student created successfully!", "OK");
            await Navigation.PopModalAsync();
        }
        catch (ArgumentException ex)
        {
            await DisplayAlert("Error", "Invalid classification string: " + ex.Message, "OK");
        }
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }



}