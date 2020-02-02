using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Xml;
using System.IO;

namespace HCI_WPF_Email_App
{
    /// <summary>
    /// Interaction logic for NewMail.xaml
    /// </summary>
    public partial class NewMailWindow : Window
    {
        public NewMailWindow(bool option=true)
        {
            InitializeComponent();

            string ImageSourcePath = System.AppDomain.CurrentDomain.BaseDirectory + @"png\";
            ImageSourceDelete.Source = new BitmapImage(new Uri(ImageSourcePath + "009-delete.png"));
            ImageSourceForward.Source = new BitmapImage(new Uri(ImageSourcePath + "008-note-1.png"));
            ImageSourceReplyAll.Source = new BitmapImage(new Uri(ImageSourcePath + "007-note-2.png"));
            ImageSourceReply.Source = new BitmapImage(new Uri(ImageSourcePath + "010-note.png"));
            ImageSourceExit.Source = new BitmapImage(new Uri(ImageSourcePath + "close.png"));
            ImageSourceMinimize.Source = new BitmapImage(new Uri(ImageSourcePath + "size.png"));
            ImageSourceAttachment.Source = new BitmapImage(new Uri(ImageSourcePath + "006-note-3.png"));
            if (!option)
            {
                NewMailSend.Visibility = Visibility.Hidden;
                AddFile.Visibility = Visibility.Hidden;
                MailTextButtonsSeparator.Visibility = Visibility.Hidden;
                toWhoText.IsReadOnly = true;
                SubjectText.IsReadOnly = true;
                MailContentText.ReadOnlyText(true);
                SenderComboBox.IsHitTestVisible = false;
                DateText.IsReadOnly = true;
                MailInputs.Children.Remove(WhenRadioButtons);
                MailInputsText.Children.Remove(WhenTextBlock);
                MailContentText.HideExpander();
                Grid.SetRow(MailContentText, 3);
           
            }
        }

        private void MinimizeButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void ExitButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void AddFile_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png |All files (*.*)|*.*";
            openFileDialog.Title = "Please Select Source File(s) for Conversion";
            string[] filePathArray;
            openFileDialog.ShowDialog();

            if ( openFileDialog.FileName!="")
            {
                AttachlList.Visibility = Visibility.Visible;
                Grid.SetColumn(MailContentText, 2);

                filePathArray = openFileDialog.FileNames;
                ListBoxItemData[] ListBoxDataSource = new ListBoxItemData[filePathArray.Length];

            for(int i=0;i<filePathArray.Length;i++){
                ListBoxDataSource[i] = new ListBoxItemData();
                ListBoxDataSource[i].Title = filePathArray[i];
                ListBoxDataSource[i].Size =  ((new FileInfo(filePathArray[i]).Length)/1024).ToString()+" KB";
                ListBoxDataSource[i].ImageData = LoadImage(filePathArray[i]);   
            }
            AttachlList.ItemsSource = ListBoxDataSource;
            }   

        }

        private BitmapImage LoadImage(string filename)
        {
            return new BitmapImage(new Uri(filename));
        }

        private void AddElement_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void NewMailSend_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (toWhoText.Text == "")
            {
                MessageBox.Show("Please set a recipient!","Warning",MessageBoxButton.OK,MessageBoxImage.Warning);

            }
            else if (SubjectText.Text != "" || (SubjectText.Text == "" && MessageBox.Show("Are you sure that want to send the mail without subject?", "Remainder", MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes))
            {
                XmlDocument emails = new XmlDocument();

                emails.Load(System.AppDomain.CurrentDomain.BaseDirectory+ @"\Emails\Emails.xml");


                XmlNode NodeToCopy = emails.SelectSingleNode("messagelist/message");

          
                XmlNode newNode = emails.CreateElement("message");

                // set the inner xml of a new node to inner xml of original node
                newNode.InnerXml = NodeToCopy.InnerXml;
                XmlAttribute att;
                att=emails.CreateAttribute("folder");
                att.Value = "Sent";
                newNode.Attributes.SetNamedItem(att);
                newNode.SelectSingleNode("subject").InnerText = SubjectText.Text;
                newNode.SelectSingleNode("date").InnerText = DateTime.Now.ToString();
                newNode.SelectSingleNode("sender").InnerText = SenderComboBox.Text;
                newNode.SelectSingleNode("content/text").InnerText = MailContentText.GetTextasXaml();
                newNode.SelectSingleNode("recipients/recipient").InnerText = toWhoText.Text;

                XmlNode messages = emails.SelectSingleNode("messagelist");
                messages.AppendChild(newNode);
                emails.Save(System.AppDomain.CurrentDomain.BaseDirectory+ @"\Emails\Emails.xml");
            }
        }

        private void AttachlList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SendLaterRBtn_Checked(object sender, RoutedEventArgs e)
        {
            GridLength g2 = new GridLength(190, GridUnitType.Pixel);

            MailInfoRow.Height = g2;
        }

        private void SendImmedlyRBtn_Checked(object sender, RoutedEventArgs e)
        {
            GridLength g2 = new GridLength(150, GridUnitType.Pixel);

            MailInfoRow.Height = g2;
        }

        private void DragButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
