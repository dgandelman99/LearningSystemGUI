using Library.Danvas3.models;

namespace LearningSystemGUI.MainMenuActions.CourseExplorer.AnnouncementManager;

public partial class AnnouncementsPage : ContentPage
{
    private Course _course;
    private Announcement _selectedAnnouncement;

    public AnnouncementsPage(Course course)
    {
        InitializeComponent();
        _course = course;
        LoadAnnouncements();
    }

    private void LoadAnnouncements()
    {
        AnnouncementsListView.ItemsSource = _course.Announcements;
    }

    private void AnnouncementsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _selectedAnnouncement = e.SelectedItem as Announcement;
    }

    private async void AddAnnouncementButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateAnnouncementPage(_course));
    }

    private async void UpdateAnnouncementButton_Clicked(object sender, EventArgs e)
    {
        if (_selectedAnnouncement != null)
        {
            await Navigation.PushAsync(new UpdateAnnouncementPage(_course, _selectedAnnouncement));
        }
        else
        {
            await DisplayAlert("Error", "No announcement selected. Please select an announcement to update.", "OK");
        }
    }

    private async void DeleteAnnouncementButton_Clicked(object sender, EventArgs e)
    {
        if (_selectedAnnouncement != null)
        {
            _course.DeleteAnnouncement(_selectedAnnouncement.ID);
            LoadAnnouncements(); // Refresh the ListView
            await DisplayAlert("Success", "Announcement deleted successfully!", "OK");
        }
        else
        {
            await DisplayAlert("Error", "No announcement selected. Please select an announcement to delete.", "OK");
        }
    }

    private void DisplayAnnouncements()
    {
        var announcementDisplayItems = _course.Announcements.Select(a => new AnnouncementDisplayItem
        {
            AnnouncementId = a.ID,
            Title = a.Title,
            Content = a.Content,
            PublishDate = a.PublishDate.ToString("MM/dd/yyyy")
        }).ToList();

        AnnouncementsListView.ItemsSource = announcementDisplayItems;
    }

    private void RefreshButton_Clicked(object sender, EventArgs e)
    {
        DisplayAnnouncements();
    }

    private async void BackButton_Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void CreateAnnouncementButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateAnnouncementPage(_course));
    }

}

public class AnnouncementDisplayItem
{
    public int AnnouncementId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string PublishDate { get; set; }
}
