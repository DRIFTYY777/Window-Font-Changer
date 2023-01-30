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
using Windows.UI.Xaml.Media;
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

namespace Trans_Back
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

            pathAsync();
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

            replacing(font_Name);
        }
        private async void message(String Title, String Content, String btn_Content)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = Title;
            dialog.Content = Content;
            dialog.PrimaryButtonText = btn_Content;
            dialog.DefaultButton = ContentDialogButton.Primary;
            var result = await dialog.ShowAsync();
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
        String dublicate_file_Path = @"Assets\changer.reg";
        private async Task<string> pathAsync()
        {
            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            String file_Path = @"Assets\original file.bat";
            StorageFile original_File2 = await InstallationFolder.GetFileAsync(file_Path);

            StorageFile dublicate_File2 = await InstallationFolder.GetFileAsync(dublicate_file_Path);


            if (File.Exists(dublicate_File2.ToString()))
            {
                File.Delete(dublicate_File2.ToString());
                File.Copy(file_Path, dublicate_file_Path);
            }
            else
            {
                if (File.Exists(original_File2.ToString()))
                {
                    File.Copy(file_Path, dublicate_file_Path);
                }
                else
                {
                    message("Error", "Reinstall It!", "OK");
                    return null;
                }
            }
            return null;
        }

        private void runner()
        {
            Process p = new Process();                          //creating new process 
            ProcessStartInfo pi = new ProcessStartInfo();       //checkin ProcessStartInfo or i dont know
            pi.UseShellExecute = true;
            pi.FileName = dublicate_file_Path;                             //defining file location 
            p.StartInfo = pi;                                   //geting file info
            try                                                 //try if file exeist run it or not throw error or if user mannuly close the file it also throw an error
            {
                p.Start();                                      //simply file.run
            }
            catch (Exception Ex)                                //capturing unexpected error like file crash or file not found or user mannuly close the file it also throw an error
            {
                message("Error", Ex.ToString(), "ok");                    //displaying error only
            }
        }
        private void replacing(String name)                     //this function replacing "Consolas" text with new font given or selected by user
        {
            String new_Path = dublicate_file_Path;
            try                                                 //try for try if not work go to catch (Exception Ex) function
            {
                string str = File.ReadAllText(new_Path);        //reading all text in dublicate file 
                str = str.Replace("Consolas", name);            //replacing "Consolas font" with user selected font
                File.WriteAllText(new_Path, str);               //replacing "Consolas font" with user selected font
                runner();                                       //calling runner function for run main file (which is important for font changing) or execute/run reg file!
            }
            catch (Exception Ex)                                //Exception for catching unexpected error for printing on display
            {
                //message(Ex.ToString(), "if you are seeing this means you close this or unexpexted error");//just displaying a unknown error 
                message("Error", Ex.ToString(), "ok");                    //displaying error only
            }
        }
    }
}


//I have a problem with the code. I want to change the font of the text in the application. I have a list of fonts and when I click on the font, the font changes. The problem is that the font changes only after the application is restarted. I want to change the font without restarting the application. I tried to use the registry, but it didn't work. I tried to use the code below, but it didn't work either. I don't know what to do. I would be grateful for any help. (USER => ){and this line is written by gitcopilot}