using Library.Danvas3.models;
using System.Threading.Tasks;

public class ContentItemDialog
{
    private ContentItem _contentItem;

    public ContentItemDialog(ContentItem contentItem = null)
    {
        _contentItem = contentItem ?? new ContentItem();
    }

    public async Task<ContentItem> Show(Page page)
    {
        ContentItem result = null;

        var nameEntry = new Entry { Placeholder = "Name", Text = _contentItem.Name };
        var descriptionEntry = new Entry { Placeholder = "Description", Text = _contentItem.Description };
        var pathEntry = new Entry { Placeholder = "Path", Text = _contentItem.Path };
        var idEntry = new Entry { Placeholder = "ID", Text = _contentItem.ID.ToString() };

        var itemTypePicker = new Picker { Title = "Item Type" };
        itemTypePicker.ItemsSource = new List<string> { "Assignment", "File", "Page" };

        var saveButton = new Button { Text = "Save" };
        var cancelButton = new Button { Text = "Cancel" };

        var stackLayout = new StackLayout
        {
            Children = { nameEntry, descriptionEntry, pathEntry, idEntry, itemTypePicker, saveButton, cancelButton },
            Padding = new Thickness(20)
        };

        var dialog = new ContentPage { Content = stackLayout };

        saveButton.Clicked += async (s, e) =>
        {
            _contentItem.Name = nameEntry.Text;
            _contentItem.Description = descriptionEntry.Text;
            _contentItem.Path = pathEntry.Text;
            _contentItem.ID = int.TryParse(idEntry.Text, out int id) ? id : 0;

            string selectedItemType = (string)itemTypePicker.SelectedItem;
            switch (selectedItemType)
            {
                case "Assignment":
                    result = new AssignmentItem { Name = _contentItem.Name, Description = _contentItem.Description, Path = _contentItem.Path, ID = _contentItem.ID };
                    break;
                case "File":
                    result = new FileItem { Name = _contentItem.Name, Description = _contentItem.Description, Path = _contentItem.Path, ID = _contentItem.ID };
                    break;
                case "Page":
                    result = new PageItem { Name = _contentItem.Name, Description = _contentItem.Description, Path = _contentItem.Path, ID = _contentItem.ID };
                    break;
                default:
                    result = _contentItem;
                    break;
            }

            await page.Navigation.PopModalAsync();
        };

        cancelButton.Clicked += async (s, e) =>
        {
            await page.Navigation.PopModalAsync();
        };

        await page.Navigation.PushModalAsync(new NavigationPage(dialog));
        return result;
    }
}
