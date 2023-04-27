using System;
using Library.Danvas3.models;
namespace LearningSystemGUI.MainMenuActions;

public partial class AddStaffPage : ContentPage
{
    private DataStorage dataStorage;

    public AddStaffPage(DataStorage dataStorage)
    {
        InitializeComponent();
        this.dataStorage = dataStorage;
    }

    private async void AddStaffButton_Clicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text ?? string.Empty;
        string classificationString = ClassificationPicker.SelectedItem?.ToString() ?? string.Empty;
        if (classificationString == "TA")
        {
            classificationString = "T";
        }
        else classificationString = "I";


        try
        {
            Classification classification = Person.ConvertStringToClassification(classificationString);
            Person faculty = new Person(name, classification);
            dataStorage.AddPerson(faculty);
            await DisplayAlert("Success", "Employee created successfully!", "OK");
        }
        catch (ArgumentException ex)
        {
            await DisplayAlert("Error", "Invalid classification string: " + ex.Message, "OK");
        }
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}