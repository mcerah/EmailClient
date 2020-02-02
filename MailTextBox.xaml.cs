using System;
using System.Collections.Generic;
using System.IO;
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

namespace HCI_WPF_Email_App
{
    /// <summary>
    /// Interaction logic for MailTextBox.xaml
    /// </summary>
    public partial class MailTextBox : UserControl
    {
      
        public MailTextBox()
        {
            InitializeComponent();

            FontSizeCombobox.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            FontSizeCombobox.Text = FontSizeCombobox.Items[2].ToString();
            FontCombobox.Text = FontCombobox.Items[2].ToString();

            FontCombobox.SelectionChanged += (s, e) =>
            {
                MailRichText.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, e.AddedItems[0]);
                MailRichText.Focus();
            };

            FontSizeCombobox.SelectionChanged += (s, e) =>
            {
                MailRichText.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, e.AddedItems[0]);
                MailRichText.Focus();
            };
        }

        public void ReadOnlyText(bool Value)
        {
            MailRichText.IsReadOnly = true;
        }


        private void MailRichText_SelectionChanged(object sender, RoutedEventArgs e)
        {


        }

        private void BoldTglBtn_Checked(object sender, RoutedEventArgs e)
        {

            MailRichTextFont(1, FontWeights.Bold);

        }

        private void MailRichTextFont(int FontProperty, object value)
        {
            TextSelection ts = MailRichText.Selection;

            if (ts != null)
            {

                if (FontProperty == 1) ts.ApplyPropertyValue(TextElement.FontWeightProperty, value);
                else if (FontProperty == 2) ts.ApplyPropertyValue(TextElement.FontStyleProperty, value);
                else if (FontProperty == 3) ts.ApplyPropertyValue(Inline.TextDecorationsProperty, value);
                else if (FontProperty == 4) { BrushConverter bc = new BrushConverter(); ts.ApplyPropertyValue(TextElement.ForegroundProperty, bc.ConvertFromString(value.ToString())); }

            }

            MailRichText.Focus();
        }

        private void BoldTglBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            MailRichTextFont(1, FontWeights.Normal);
        }

        private void ItalicTglBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            MailRichTextFont(2, FontStyles.Normal);
        }

        private void ItalicTglBtn_Checked(object sender, RoutedEventArgs e)
        {
            MailRichTextFont(2, FontStyles.Italic);
        }

        private void UnderLineTglBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            MailRichTextFont(3, null);
        }

        private void UnderLineTglBtn_Checked(object sender, RoutedEventArgs e)
        {
            MailRichTextFont(3, TextDecorations.Underline);
        }

        private void TextColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MailRichTextFont(4, TextColorPicker.SelectedColor);
        }

        private void ClearAllTglBtn_Click(object sender, RoutedEventArgs e)
        {
            MailRichTextFont(2, FontStyles.Normal);
            MailRichTextFont(3, null);
            MailRichTextFont(1, FontWeights.Normal);

            BoldTglBtn.IsChecked = false;
            ItalicTglBtn.IsChecked = false;
            UnderLineTglBtn.IsChecked = false;
        }
        public string GetTextasXaml()
        {
            TextRange range = new TextRange(MailRichText.Document.ContentStart, MailRichText.Document.ContentEnd);
            MemoryStream stream = new MemoryStream();
            range.Save(stream, DataFormats.Xaml);
            string xamlText = Encoding.UTF8.GetString(stream.ToArray());
            return xamlText;
        }

        public void SetXaml(string xamlString)
        {
            StringReader stringReader = new StringReader(xamlString);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            Section sec = System.Windows.Markup.XamlReader.Load(xmlReader) as Section;
            FlowDocument doc = new FlowDocument();
         
            while (sec.Blocks.Count > 0)
                doc.Blocks.Add(sec.Blocks.FirstBlock);
            
            this.MailRichText.Document = doc;
        }
        private void ReserveBtn_Click(object sender, RoutedEventArgs e)
        {


        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            Grid.SetRowSpan(MailRichText,1);
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            Grid.SetRowSpan(MailRichText, 2);
        }

        public void HideExpander()
        {
            Grid.SetRowSpan(MailRichText, 2);
            TextFontExpander.Visibility = Visibility.Hidden;
        }
    }
}
