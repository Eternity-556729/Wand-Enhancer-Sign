using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WandEnhancer.Core;
using WandEnhancer.Core.Services;
using WandEnhancer.View.MainWindow;

namespace WandEnhancer.View.Popups
{
    public partial class SettingsPopup : UserControl
    {
        private CultureInfo _selectedLanguage;

        public SettingsPopup()
        {
            InitializeComponent();
            LoadLanguages();
            LoadTheme();
        }

        private void LoadTheme()
        {
            if (ThemeManager.CurrentTheme == AppTheme.Light)
            {
                LightThemeRadio.IsChecked = true;
            }
            else
            {
                DarkThemeRadio.IsChecked = true;
            }
        }

        private void LoadLanguages()
        {
            var items = LocalizationManager.SupportedLanguages
                .Select(c => new LanguageItem
                {
                    Culture = c,
                    DisplayName = LocalizationManager.GetLanguageDisplayName(c)
                })
                .ToList();

            LanguageComboBox.ItemsSource = items;
            
            var currentItem = items.FirstOrDefault(i => i.Culture.Name == LocalizationManager.CurrentLanguage?.Name);
            if (currentItem != null)
            {
                LanguageComboBox.SelectedItem = currentItem;
            }
            
            _selectedLanguage = LocalizationManager.CurrentLanguage;
        }

        private void OnLanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem is LanguageItem item)
            {
                _selectedLanguage = item.Culture;
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (_selectedLanguage != null &&
                (LocalizationManager.CurrentLanguage == null ||
                 _selectedLanguage.Name != LocalizationManager.CurrentLanguage.Name))
            {
                LocalizationManager.CurrentLanguage = _selectedLanguage;
            }

            var selectedTheme = LightThemeRadio.IsChecked == true ? AppTheme.Light : AppTheme.Dark;
            if (selectedTheme != ThemeManager.CurrentTheme)
            {
                ThemeManager.CurrentTheme = selectedTheme;
            }

            MainWindow.MainWindow.Instance.ClosePopup();
        }

        private class LanguageItem
        {
            public CultureInfo Culture { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
