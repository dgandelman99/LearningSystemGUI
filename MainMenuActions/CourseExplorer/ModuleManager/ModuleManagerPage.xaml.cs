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
        LoadContentItems();
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

    private void LoadContentItems()
    {
        var selectedModule = (Module)ModulesListView.SelectedItem;
        if (selectedModule != null)
        {
            ContentItemsListView.ItemsSource = selectedModule.ContentItems;
        }
    }

    private async void AddContentItemButton_Clicked(object sender, EventArgs e)
    {
        var selectedModule = (Module)ModulesListView.SelectedItem;
        if (selectedModule != null)
        {
            var contentItem = await DisplayAddContentItemDialog();
            if (contentItem != null)
            {
                selectedModule.ContentItems.Add(contentItem);
                RefreshContentItemsList(selectedModule);
            }
        }
    }

    private async Task<ContentItem> DisplayAddContentItemDialog()
    {
        // Display dialog to get the content item type
        string contentType = await DisplayActionSheet("Select content item type", "Cancel", null, "AssignmentItem", "FileItem", "PageItem");

        if (contentType == "Cancel")
        {
            return null;
        }

        // Display dialog to get the content item name
        string name = await DisplayPromptAsync("Content Item Name", "Please enter the content item name:");

        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        // Display dialog to get the content item description
        string description = await DisplayPromptAsync("Content Item Description", "Please enter the content item description:");

        if (string.IsNullOrEmpty(description))
        {
            return null;
        }

        // Create a new ContentItem based on the user input and assign a unique ID
        ContentItem newItem;
        int newID = GetNextContentItemId();

        switch (contentType)
        {
            case "AssignmentItem":
                newItem = new AssignmentItem { Name = name, Description = description, ID = newID };
                break;
            case "FileItem":
                newItem = new FileItem { Name = name, Description = description, ID = newID };
                break;
            case "PageItem":
                newItem = new PageItem { Name = name, Description = description, ID = newID };
                break;
            default:
                newItem = null;
                break;
        }

        return newItem;
    }

    private int GetNextContentItemId()
    {
        int highestId = 0;

        foreach (var module in _course.Modules)
        {
            foreach (var contentItem in module.ContentItems)
            {
                if (contentItem.ID > highestId)
                {
                    highestId = contentItem.ID;
                }
            }
        }

        return highestId + 1;
    }


    private void RefreshContentItemsList(Module selectedModule)
    {
        ContentItemsListView.ItemsSource = null;
        ContentItemsListView.ItemsSource = selectedModule.ContentItems;
    }


    private async void UpdateContentItemButton_Clicked(object sender, EventArgs e)
    {
        var selectedModule = (Module)ModulesListView.SelectedItem;
        var selectedContentItem = (ContentItem)ContentItemsListView.SelectedItem;
        if (selectedModule != null && selectedContentItem != null)
        {
            var contentItemDialog = new ContentItemDialog(selectedContentItem);
            var result = await contentItemDialog.Show(this);

            if (result != null)
            {
                int index = selectedModule.ContentItems.IndexOf(selectedContentItem);
                selectedModule.ContentItems[index] = result;
                LoadContentItems();
            }
        }
    }

    private void DeleteContentItemButton_Clicked(object sender, EventArgs e)
    {
        var selectedModule = (Module)ModulesListView.SelectedItem;
        var selectedContentItem = (ContentItem)ContentItemsListView.SelectedItem;
        if (selectedModule != null && selectedContentItem != null)
        {
            selectedModule.ContentItems.Remove(selectedContentItem);
            LoadContentItems();
        }
    }

    private void ModulesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedModule = e.SelectedItem as Module;
        if (selectedModule != null)
        {
            ContentItemsListView.ItemsSource = selectedModule.ContentItems;
        }
    }

    private void ModulesListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var selectedModule = e.Item as Module;
        if (selectedModule != null)
        {
            LoadContentItems(selectedModule);
        }
    }

    private void LoadContentItems(Module selectedModule)
    {
        ContentItemsListView.ItemsSource = null;
        ContentItemsListView.ItemsSource = selectedModule.ContentItems;
    }



}