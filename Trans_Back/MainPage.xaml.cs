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

        private async Task<String> copyFile()
        {
            StorageFolder localFolder = null;
            try
            {
                localFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/original.reg"));

                if (file == null)
                {
                    message("Error", "File not found", "Ok");
                    return null;
                }

                // Check if file already exists in local folder and delete it
                StorageFile existingFile = await localFolder.TryGetItemAsync("original.reg") as StorageFile;
                if (existingFile != null)
                {
                    await existingFile.DeleteAsync();
                }

                // Copy the file to the local folder
                await file.CopyAsync(localFolder, "original.reg", NameCollisionOption.ReplaceExisting);

                // Return the copied file
                return localFolder.Path + "\\original.reg";
            }
            catch (Exception ex)
            {
                // Handle the exception
                if (ex.HResult == unchecked((int)0x800C000E))
                {
                    // A security problem occurred
                    message("Error", "A security problem occurred", "Ok");
                }
                else
                {
                    // Other exception occurred
                    message("Error", ex.Message, "Ok");
                }
                return null;
            }
        }
        
        // take path from copyFile() method and replace font name in the file with the font name that user selected
        private async Task replacing(String font_Name)
        {
            try
            {
                String path = await copyFile();
                //String text = File.ReadAllText(path);

                //text = text.Replace("Segoe UI", font_Name);
                //File.WriteAllText(path, text);

                //Process process = new Process();
                //process.StartInfo.FileName = "cmd.exe";
                //process.StartInfo.UseShellExecute = true;
                //process.StartInfo.Arguments = "/c regedit.exe /s " + path;
                //process.StartInfo.Verb = "runas";
                //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //process.Start();
            }
            catch (Exception ex)
            {
                message("Message", ex.Message , "ok");
            }

        }

        // after doing that run the file and restart the application
        private void comboFonts_SelectionChanged(String font_Name)
        {
            if (font_Name != "Default")
            {
                replacing(font_Name);
            }
        }


        // after doing that delete the file from temp path
        private async void deleteFile()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/original.reg"));
            await file.DeleteAsync();
        }
    }
}


//I have a problem with the code. I want to change the font of the text in the application. I have a list of fonts and when I click on the font, the font changes. The problem is that the font changes only after the application is restarted. I want to change the font without restarting the application. I tried to use the registry, but it didn't work. I tried to use the code below, but it didn't work either. I don't know what to do. I would be grateful for any help. (USER => ){and this line is written by gitcopilot}