using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

using FontFamily = System.Drawing.FontFamily;
using System.Security.AccessControl;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using System.Xml.Linq;
using System.Threading;
using Windows.Storage;

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

        private void comboFonts_SelectionChanged(String font_Name)
        {
            if (font_Name != "Default")
            {
                antyAsync("E:/visual project/C# WUP/Windows_Font_Changer/Trans_Back/Assets/File.bat");
            }
        }
        // after doing that delete the file from temp path
        private async void deleteFile()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/original.reg"));
            await file.DeleteAsync();
        }

        private async Task antyAsync(String path)
        {
            await CopyFileFromAssetsToTempFolderAsync(path);
        }

        public async Task CopyFileFromAssetsToTempFolderAsync(string assetFileName)
        {
            try
            {
                // Get a reference to the temporary folder
                StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;

                // Get a reference to the app's package folder (where assets are located)
                StorageFolder appPackageFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

                // Get a StorageFile reference to the asset file
                StorageFile assetFile = await appPackageFolder.GetFileAsync(assetFileName);

                // Copy the asset file to the temporary folder
                await assetFile.CopyAsync(tempFolder, assetFileName, NameCollisionOption.ReplaceExisting);

                // Handle the copied file as needed
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur
                Console.WriteLine($"Error: {ex.Message}");
                message("tile", $"Error: {ex.Message}", "ok");
            }
        }
    }
}


