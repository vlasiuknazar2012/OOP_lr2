using System.Text;
using OOP_lr2.Services;
using OOP_lr2.ViewModels;

namespace OOP_lr2;

/// <summary>
/// Подання (View). Обробники подій лише передають дії в MainViewModel і оновлюють екран,
/// уся логіка винесена в окремі класи (антишаблон «магічна кнопка» не використовується).
/// </summary>
public partial class MainPage : ContentPage
{
    private const string AnyValue = "(будь-яке значення)";

    private static readonly FilePickerFileType XmlFileType = new(new Dictionary<DevicePlatform, IEnumerable<string>>
    {
        { DevicePlatform.WinUI, new[] { ".xml" } },
        { DevicePlatform.Android, new[] { "text/xml", "application/xml" } },
        { DevicePlatform.iOS, new[] { "public.xml" } },
        { DevicePlatform.MacCatalyst, new[] { "public.xml" } }
    });

    private readonly MainViewModel _viewModel;

    public MainPage()
    {
        InitializeComponent();
        _viewModel = new MainViewModel();
        StrategyPicker.ItemsSource = _viewModel.StrategyNames.ToList();
        StrategyPicker.SelectedIndex = 0;
    }

    private async void SelectFileButton_Clicked(object? sender, EventArgs e)
    {
        try
        {
            var file = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Оберіть XML-файл",
                FileTypes = XmlFileType
            });
            if (file == null)
            {
                return;
            }

            _viewModel.SelectFile(file.FullPath);
            FilePathLabel.Text = file.FullPath;
            ResultEditor.Text = string.Empty;
            HtmlWebView.Source = new HtmlWebViewSource { Html = string.Empty };
            BuildAttributePickers();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private void StrategyPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (StrategyPicker.SelectedItem is string name)
        {
            _viewModel.SelectStrategy(name);
        }
    }

    private void AttributePicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (sender is not Picker picker)
        {
            return;
        }

        var value = picker.SelectedIndex <= 0 ? string.Empty : picker.SelectedItem as string ?? string.Empty;
        _viewModel.SetCriterion(picker.StyleId, value);
    }

    private async void SearchButton_Clicked(object? sender, EventArgs e)
    {
        try
        {
            _viewModel.Search();
            ShowResult();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private async void TransformButton_Clicked(object? sender, EventArgs e)
    {
        try
        {
            var html = _viewModel.TransformToHtml();
            var htmlPath = Path.ChangeExtension(_viewModel.XmlPath, ".html");
            await File.WriteAllTextAsync(htmlPath, html, new UTF8Encoding(false));
            HtmlWebView.Source = new HtmlWebViewSource { Html = html };
            await DisplayAlert("Готово", $"HTML-файл збережено:{Environment.NewLine}{htmlPath}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private void ClearButton_Clicked(object? sender, EventArgs e)
    {
        _viewModel.Clear();
        foreach (var picker in AttributesPanel.Children.OfType<Picker>())
        {
            picker.SelectedIndex = 0;
        }

        ShowResult();
        HtmlWebView.Source = new HtmlWebViewSource { Html = string.Empty };
    }

    private async void ExitButton_Clicked(object? sender, EventArgs e)
    {
        if (await ExitConfirmation.AskAsync(this))
        {
            Application.Current?.Quit();
        }
    }

    private void ShowResult()
    {
        ResultEditor.Text = _viewModel.ResultText;
    }

    /// <summary>Для кожного атрибута, що є у файлі, створює список значень, присутніх у цьому файлі.</summary>
    private void BuildAttributePickers()
    {
        AttributesPanel.Children.Clear();
        foreach (var attribute in _viewModel.AttributeNames)
        {
            var values = new List<string> { AnyValue };
            values.AddRange(_viewModel.GetValues(attribute));

            var picker = new Picker { StyleId = attribute, ItemsSource = values, SelectedIndex = 0 };
            picker.SelectedIndexChanged += AttributePicker_SelectedIndexChanged;

            AttributesPanel.Children.Add(new Label { Text = char.ToUpper(attribute[0]) + attribute[1..] });
            AttributesPanel.Children.Add(picker);
        }
    }
}
