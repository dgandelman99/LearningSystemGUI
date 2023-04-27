using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer.AnnouncementManager;

public partial class CreateAnnouncementPage : ContentPage
{
    private Course _course;

    public CreateAnnouncementPage(Course course)
    {
        InitializeComponent();
        _course = course;
    }

    private async void CreateAnnouncementButton_Clicked(object sender, EventArgs e)
    {
        string title = TitleEntry.Text;
        string content = ContentEditor.Text;
        Announcement announcement = new Announcement
        {
            Title = title,
            Content = content,
            PublishDate = DateTime.Now
        };

        _course.AddAnnouncement(announcement);
        await DisplayAlert("Success", "Announcement created successfully!", "OK");
        await Navigation.PopAsync();
    }

    private async void BackButton_Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
