using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HCI_WPF_Email_App
{
    class ListBoxItemData
    {
        private string _Title;
        private string _Size;
        public string Title
        {
            get { return this._Title; }
            set { this._Title = value; }
        }

        public string Size
        {
            get { return this._Size; }
            set { this._Size = value; }
        }

        private BitmapImage _ImageData;
        public BitmapImage ImageData
        {
            get { return this._ImageData; }
            set { this._ImageData = value; }
        }

    }
}
