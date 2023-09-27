using System;

using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


using Microsoft.UI.Xaml.Controls;
using System.Threading;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Windows_Font_Changer
{
    public sealed partial class MainPage : Page
    {
        int milliseconds = 15;
        Boolean Hidden_Search = false;
        String[] Fonts;
        String[] display_Fonts;
        public MainPage()
        {
            this.InitializeComponent();
            Search.Visibility = Visibility.Collapsed;

            var local_Fonts = Microsoft.Graphics.Canvas.Text.CanvasTextFormat.GetSystemFontFamilies();
            Fonts = new String[local_Fonts.Length + 1];
            Fonts[0] = "Default";
            for (int i = 0; i < local_Fonts.Length; i++)
            {
                Fonts[i + 1] = local_Fonts[i].ToString();
            }
            display_Fonts = (string[])Fonts.Clone(); 
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            comboFonts.ItemsSource = display_Fonts;
            comboFonts.SelectedIndex = 0;
        }
        private void ListView1_ItemClick(object sender, ItemClickEventArgs e)
        {
            var font_Name = e.ClickedItem.ToString();
            Thread.Sleep(milliseconds);

            //replacing(font_Name);
            comboFonts_SelectionChanged(font_Name);
            // providing font name to user
        }
        private async void message(String Title, String Content, String btn_Content)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = Title;
            dialog.Content = Content;
            dialog.PrimaryButtonText = btn_Content;
            dialog.DefaultButton = ContentDialogButton.Primary;
            await dialog.ShowAsync();
        }
        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState(this.SearchAnimatedIcon, "PointerOver");
        }
        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState(this.SearchAnimatedIcon, "Normal");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String Searched_text = Search.Text;
            if (!Hidden_Search)
            {
                Search.Visibility = Visibility.Visible;
                Hidden_Search = true;
            }
            else if (Hidden_Search && Searched_text == "")
            {
                Search.Visibility = Visibility.Collapsed;
                Search.Text = "";
                Hidden_Search = false;
            }
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            String Searched_text = Search.Text;
            if (Searched_text != "")
            {
                String data = Searched_text.ToString();
                display_Fonts = Fonts.Where(F => F.ToLower().Replace(" ","").StartsWith(data.ToLower().Replace(" ",""))).ToArray();
                comboFonts.ItemsSource = display_Fonts;
            }
            else
            {
                comboFonts.ItemsSource = Fonts;
            }
        }

        [DllImport("test.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern int sssss(string regFilePath);


        private void comboFonts_SelectionChanged(String font_Name)
        {
            if (font_Name == "Default")
            {
                font_Name = "Segoe UI";
            }
            try
            {
                string regFilePath = @"C:\Users\dhima\AppData\Local\Packages\9df81f4c-7e10-440d-99b8-f4b1770bf348_844j25bcfdrry\AC\Temp\test.reg";
                int result = sssss(regFilePath);
                if (result == 0)
                {
                    Console.WriteLine("Success: The .reg file has been executed.");
                }
                else
                {
                    Console.WriteLine("An error occurred.");
                }
            }
            catch(Exception e)
            {
                message("Error", e.Message, "OK");
            }
        }
    }
}


