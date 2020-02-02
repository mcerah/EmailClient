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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.IO;
using Microsoft.Win32;

namespace HCI_WPF_Email_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool DefaultView = true;
        string selectedMail = null;
        XmlDocument emails = new XmlDocument();
        XmlNode messages;
        int selectedFolder = 0;
        public MainWindow()
        {
            InitializeComponent();
            string ImageSourcePath = System.AppDomain.CurrentDomain.BaseDirectory + @"png\";
            ImageSourceCompose.Source = new BitmapImage(new Uri(ImageSourcePath + @"005-email.png"));
            ImageSourceAlternate.Source = new BitmapImage(new Uri(ImageSourcePath + "alternate3.png"));
            ImageSourceDefault.Source = new BitmapImage(new Uri(ImageSourcePath + "defaultv3.png"));
            ImageSourceDelete.Source = new BitmapImage(new Uri(ImageSourcePath + "009-delete.png"));
            ImageSourceDraft.Source = new BitmapImage(new Uri(ImageSourcePath + "text.png"));
            ImageSourceForward.Source = new BitmapImage(new Uri(ImageSourcePath + "008-note-1.png"));
            ImageSourceReplyAll.Source = new BitmapImage(new Uri(ImageSourcePath + "007-note-2.png"));
            ImageSourceReply.Source = new BitmapImage(new Uri(ImageSourcePath + "010-note.png"));
            ImageSourceInbox.Source = new BitmapImage(new Uri(ImageSourcePath + "inbox.png"));
            ImageSourceTrash.Source = new BitmapImage(new Uri(ImageSourcePath + "trash-can-outline.png"));
            ImageSourceSent.Source = new BitmapImage(new Uri(ImageSourcePath + "email-verified.png"));
            ImageSourceMenu.Source = new BitmapImage(new Uri(ImageSourcePath + "menu.png"));
            ImageSourceM2Inbox.Source = new BitmapImage(new Uri(ImageSourcePath + "inbox.png"));
            ImageSourceExit.Source = new BitmapImage(new Uri(ImageSourcePath + "close.png"));
            ImageSourceMinimize.Source = new BitmapImage(new Uri(ImageSourcePath + "size.png"));
        }
  
        //Resim Yolları
        //Send Buton Bildirim
        //Attachment Gönderim ve Gösterim
        class mailListViewitem {

            public string MailListImgSrc { get; set; }
            public string MailListSenderText { get; set; }
            public string MailListSubjectText { get; set; }
            public string MailListDateText { get; set; }
        }

        private void MainMailWindow_Loaded(object sender, RoutedEventArgs e)
        {
            emails.Load(System.AppDomain.CurrentDomain.BaseDirectory+ @"\Emails\Emails.xml");
            messages = emails.SelectSingleNode("messagelist");
           
        }

        private void EmailFolders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedItem = (TreeViewItem)EmailFolders.SelectedItem;
            SetEmalistItemsSource(selectedItem.Name);
        }

        private void SetEmalistItemsSource(string selectedItem)
        {
            EmailList.ItemsSource = null;

            List<mailListViewitem> MailList = new List<mailListViewitem>();

            if (selectedItem== "InboxFolder")
            {
                selectedFolder = 1;

                foreach (XmlNode message in messages.SelectNodes("message"))
                {
                    if (message.Attributes[0].Value.ToString() == "Inbox")
                        MailList.Add(new mailListViewitem { MailListImgSrc = System.AppDomain.CurrentDomain.BaseDirectory + @"\png\user.png", MailListDateText = message.SelectSingleNode("date").InnerText, MailListSenderText = message.SelectSingleNode("sender").InnerText, MailListSubjectText = message.SelectSingleNode("subject").InnerText });
                }


            }

            else if (selectedItem== "SentFolder")
            {
                selectedFolder = 2;
                foreach (XmlNode message in messages.SelectNodes("message"))
                {
                    if (message.Attributes[0].Value.ToString() == "Sent")
                        MailList.Add(new mailListViewitem { MailListImgSrc =System.AppDomain.CurrentDomain.BaseDirectory+ @"\png\user.png", MailListDateText = message.SelectSingleNode("date").InnerText, MailListSenderText = message.SelectSingleNode("sender").InnerText, MailListSubjectText = message.SelectSingleNode("subject").InnerText });
                }

            }
            else if (selectedItem== "DraftFolder")
            {
                selectedFolder = 3;
                foreach (XmlNode message in messages.SelectNodes("message"))
                {
                    if (message.Attributes[0].Value.ToString() == "Draft")
                        MailList.Add(new mailListViewitem { MailListImgSrc =System.AppDomain.CurrentDomain.BaseDirectory+ @"\png\user.png", MailListDateText = message.SelectSingleNode("date").InnerText, MailListSenderText = message.SelectSingleNode("sender").InnerText, MailListSubjectText = message.SelectSingleNode("subject").InnerText });
                }

            }
            else if (selectedItem== "TrashFolder")
            {
                selectedFolder = 4;
                foreach (XmlNode message in messages.SelectNodes("message"))
                {
                    if (message.Attributes[0].Value.ToString() == "Trash")
                        MailList.Add(new mailListViewitem { MailListImgSrc =System.AppDomain.CurrentDomain.BaseDirectory+ @"\png\user.png", MailListDateText = message.SelectSingleNode("date").InnerText, MailListSenderText = message.SelectSingleNode("sender").InnerText, MailListSubjectText = message.SelectSingleNode("subject").InnerText });
                }

            }
            EmailList.ItemsSource = MailList;
        }

        private void EmailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mailListViewitem temp = new mailListViewitem();
            if (EmailList.SelectedValue != null) temp = (mailListViewitem)EmailList.SelectedItem;
            selectedMail = temp.MailListSubjectText;
            XmlDocument emails = new XmlDocument();

            emails.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"\Emails\Emails.xml");

            XmlNode messages = emails.SelectSingleNode("messagelist");
            XmlNode messageTemp;
            foreach (XmlNode message in messages.SelectNodes("message"))
            {
                if (selectedMail != null && ((message.SelectSingleNode("subject").InnerText) == selectedMail.ToString()))
                {
                    messageTemp = message.SelectSingleNode("content");
                  SetXaml(messageTemp.SelectSingleNode("text").InnerText);

                    EmailHeader.Text = message.SelectSingleNode("subject").InnerText;
                    EmailHeaderDate.Text = message.SelectSingleNode("date").InnerText;
                }
            }
        }

        public void SetXaml(string xamlString)
        {
            StringReader stringReader = new StringReader(xamlString);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            Section sec = System.Windows.Markup.XamlReader.Load(xmlReader) as Section;
            FlowDocument doc = new FlowDocument();

            while (sec.Blocks.Count > 0)
                doc.Blocks.Add(sec.Blocks.FirstBlock);

            this.EmailContent.Document = doc;
        }

        private void MenuImport_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true) {
                emails.Load(openFileDialog.FileName);
                messages = emails.SelectSingleNode("messagelist");
            }

        }

        private void MenuExport_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml";

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var stringWriter = new StringWriter())
                using (XmlWriter writer = XmlWriter.Create(stringWriter))
                {
                    emails.WriteTo(writer);
                    writer.Flush();
                    File.WriteAllText(saveFileDialog.FileName, stringWriter.GetStringBuilder().ToString());
                }
            }
            
        }

        private void ExitButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ExitButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;

        }

        private void DefaultLayout_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!DefaultView)
            {

                EmailContainer.Children.Remove(EmailList);
                BaseGrid.Children.Add(EmailList);
                EmailContainer.Children.Add(QuickButtonsSeparator);

                EmailContainer.RowDefinitions[0].Height = new GridLength(32, GridUnitType.Pixel);
                EmailContainer.RowDefinitions[1].Height = new GridLength(80, GridUnitType.Pixel);
                EmailContainer.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                EmailContainer.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);

                BaseGrid.RowDefinitions[2].Height = new GridLength(30, GridUnitType.Pixel);

                Grid.SetColumn(RibbonPanel, 2);
                Grid.SetRow(FindWMTextBox, 1);
                Grid.SetColumnSpan(FindWMTextBox, 1);
                Grid.SetColumn(EmailContainer, 2);
                Grid.SetRow(EmailContainer, 2);
                Grid.SetColumnSpan(EmailContainer, 1);
                Grid.SetRow(HeaderContentSeperator, 2);
                Grid.SetRow(EmailList, 2);
                Grid.SetRowSpan(EmailList, 3);
                Grid.SetRow(EmailContentButtons, 0);
                Grid.SetRow(EmailContentHeader, 1);
                Grid.SetRow(EmailContentScroll, 2);
                DefaultView = true;
            }
        }

        private void AlternativeLayout_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DefaultView)
            {
                BaseGrid.Children.Remove(EmailList);

                EmailContainer.Children.Add(EmailList);

                EmailContainer.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                EmailContainer.RowDefinitions[1].Height = new GridLength(32, GridUnitType.Pixel);
                EmailContainer.RowDefinitions[2].Height = new GridLength(80, GridUnitType.Pixel);
                EmailContainer.RowDefinitions[3].Height = new GridLength(2, GridUnitType.Star);

                BaseGrid.RowDefinitions[2].Height = new GridLength(40, GridUnitType.Pixel);
               
                EmailContainer.Children.Remove(QuickButtonsSeparator);


                Grid.SetColumn(RibbonPanel, 1);
                Grid.SetColumnSpan(FindWMTextBox, 2);
                Grid.SetRow(FindWMTextBox, 2);
                Grid.SetColumn(EmailContainer, 1);
                Grid.SetRow(EmailContainer, 3);
                Grid.SetRow(HeaderContentSeperator, 3);
                Grid.SetColumnSpan(EmailContainer, 2);
                Grid.SetRow(EmailList, 0);
                Grid.SetRowSpan(EmailList, 1);
                Grid.SetRow(EmailContentButtons, 1);
                Grid.SetRow(EmailContentHeader, 2);
                Grid.SetRow(EmailContentScroll, 3);
           
                DefaultView = false;
            }
        }

        private void DeleteMailButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
          
            if (selectedFolder != 4)
            {
                foreach (XmlNode message in messages.SelectNodes("message"))
                {
                    if (message.SelectSingleNode("subject").InnerText == selectedMail)
                    {
                        message.Attributes[0].Value = "Trash";
                        SetEmalistItemsSource("InboxFolder");

                        EmailHeader.Text = "";
                        EmailHeaderDate.Text = "";
                        EmailContent.Document.Blocks.Clear();

                        break;
                    }
                }

            }
            else if (MessageBox.Show("Are you sure that want to delete this message permanently?", "Delete E-Mail Message", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                foreach (XmlNode message in messages.SelectNodes("message"))
                {
                    if (message.SelectSingleNode("subject").InnerText == selectedMail)
                    {
                        message.Attributes[0].Value = "Deleted";
                        SetEmalistItemsSource("TrashFolder");

                        EmailHeader.Text = "";
                        EmailHeaderDate.Text = "";
                        EmailContent.Document.Blocks.Clear();
                        break;
                    }
                }
            }

   
        }

        private void NewMailGeneral_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NewMailWindow newMail = new NewMailWindow(true);

            newMail.Show();
         
        }

        private void EmailList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            mailListViewitem temp = new mailListViewitem();
            if (EmailList.SelectedValue != null) temp = (mailListViewitem)EmailList.SelectedItem;
            selectedMail = temp.MailListSubjectText;

            XmlDocument emails = new XmlDocument();

            emails.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"\Emails\Emails.xml");

            XmlNode messages = emails.SelectSingleNode("messagelist");
            XmlNode messageTemp;
            foreach (XmlNode message in messages.SelectNodes("message"))
            {
                if (selectedMail != null && ((message.SelectSingleNode("subject").InnerText) == selectedMail.ToString()))
                {
                    messageTemp = message.SelectSingleNode("content");
                    NewMailWindow newMail = new NewMailWindow(false);

              
                    newMail.ToTextBlock.Text = "From";
                    newMail.FromTextBlock.Text = "To";
                    newMail.SubjectText.Text = message.SelectSingleNode("subject").InnerText;
                    newMail.MailContentText.SetXaml(messageTemp.SelectSingleNode("text").InnerText);
                    newMail.toWhoText.Text = message.SelectSingleNode("sender").InnerText;
                    newMail.DateText.Text = message.SelectSingleNode("date").InnerText;
                    newMail.Show();
                    break;
                }
            }

        }

        private void MailFoldersExpander_Expanded(object sender, RoutedEventArgs e)
        {
            Grid.SetRowSpan(MailFoldersExpander, 3);
            GridLength g2 = new GridLength(1, GridUnitType.Star);
            EmailListColumn.Width = g2;
            MailFoldersExpander.Header = "Collapse";
        }

        private void MailFoldersExpander_Collapsed(object sender, RoutedEventArgs e)
        {
     
            GridLength g2 = new GridLength(20, GridUnitType.Pixel);
            EmailListColumn.Width = g2;
        }

        private void MenuItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void DragButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void DragButton_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            
        }

    }
}
