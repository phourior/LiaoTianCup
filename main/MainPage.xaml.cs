using main.Dictionary.I18n;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace main
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // i18n
        /// <summary>
        /// 点击按钮赋值语言的类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void I18nBtn(object sender, RoutedEventArgs e)
        {
            LanguageManager.Instance.ChangeLanguage((sender as Button).Content.ToString().Equals("English")
                ? new CultureInfo("en-US")
                : new CultureInfo("zh-CN"));
        }

        private void Button_RandomMutation_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/main;component/Mode/RandomMutationWindow.xaml", UriKind.Relative));
        }

        private void Button_Negative_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/main;component/Mode/NegativeFactorWindow.xaml", UriKind.Relative));
        }

        private void Button_Doubles_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/main;component/Mode/DoublesModeWindow.xaml", UriKind.Relative));
        }

        private void Button_Single_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/main;component/Mode/SingleModeWindow.xaml", UriKind.Relative));
        }

        private void Button_USuck_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/main;component/Mode/USuckModeWindow.xaml", UriKind.Relative));
        }

        private void Button_Hub_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/main;component/Mode/HubModeWindow.xaml", UriKind.Relative));
        }

        private void Button_SoloHub_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/main;component/Mode/HubSoloModeWindow.xaml", UriKind.Relative));
        }


    }
}
