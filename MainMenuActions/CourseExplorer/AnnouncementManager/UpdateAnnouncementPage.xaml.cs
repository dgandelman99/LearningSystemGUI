using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer.AnnouncementManager;

public partial class UpdateAnnouncementPage : ContentPage
{
    private Course _course;
    private Announcement _announcement;

    public UpdateAnnouncementPage(Course course, Announcement announcement)
    {
        InitializeComponent();
        _course = course;
        _announcement = announcement;
        LoadAnnouncement();
    }

    private void LoadAnnouncement()
    {
        TitleEntry.Text = _announcement.Title;
        ContentEditor.Text = _announcement.Content;
    }

    private async void UpdateAnnouncementButton_Clicked(object sender, EventArgs e)
    {
        _announcement.Title = TitleEntry.Text;
        _announcement.Content = ContentEditor.Text;

        await DisplayAlert("Success", "Announcement updated successfully!", "OK");
        await Navigation.PopAsync();
    }

    private async void DeleteAnnouncementButton_Clicked(object sender, EventArgs e)
    {
        _course.DeleteAnnouncement(_announcement.ID);
        await DisplayAlert("Success", "Announcement deleted successfully!", "OK");
        await Navigation.PopAsync();
    }

    private async void BackButton_Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }


}
