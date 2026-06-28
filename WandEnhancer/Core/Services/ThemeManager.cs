using System;
using System.Linq;
using System.Windows;

namespace WandEnhancer.Core.Services
{
    public enum AppTheme
    {
        Dark,
        Light
    }

    public static class ThemeManager
    {
        private const string DarkSource = "Style/Theme.Dark.xaml";
        private const string LightSource = "Style/Theme.Light.xaml";

        private static AppTheme _currentTheme = AppTheme.Dark;

        public static AppTheme CurrentTheme
        {
            get => _currentTheme;
            set => SetTheme(value);
        }

        public static void Initialize()
        {
            var saved = SettingsManager.LoadSettings()?.Theme;
            var theme = string.Equals(saved, AppTheme.Light.ToString(), StringComparison.OrdinalIgnoreCase)
                ? AppTheme.Light
                : AppTheme.Dark;

            // Dark is already loaded from App.xaml, so only swap when the saved choice differs.
            if (theme == AppTheme.Dark)
            {
                _currentTheme = AppTheme.Dark;
                return;
            }

            SetTheme(theme, saveSettings: false);
        }

        private static void SetTheme(AppTheme theme, bool saveSettings = true)
        {
            _currentTheme = theme;

            var source = theme == AppTheme.Light ? LightSource : DarkSource;
            var newDict = new ResourceDictionary { Source = new Uri(source, UriKind.Relative) };

            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var oldDict = dictionaries.FirstOrDefault(d => d.Source != null &&
                (d.Source.OriginalString.IndexOf("Theme.Dark", StringComparison.OrdinalIgnoreCase) >= 0 ||
                 d.Source.OriginalString.IndexOf("Theme.Light", StringComparison.OrdinalIgnoreCase) >= 0));

            if (oldDict != null)
            {
                var index = dictionaries.IndexOf(oldDict);
                dictionaries[index] = newDict;
            }
            else
            {
                dictionaries.Add(newDict);
            }

            if (saveSettings)
            {
                var settings = SettingsManager.LoadSettings() ?? new AppSettings();
                settings.Theme = theme.ToString();
                SettingsManager.SaveSettings(settings);
            }
        }
    }
}
