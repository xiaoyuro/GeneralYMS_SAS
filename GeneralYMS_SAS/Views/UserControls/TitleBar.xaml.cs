using System.Windows.Controls;
using System.Windows.Media;

namespace GeneralYMS_SAS.Views.UserControls
{
    /// <summary>
    /// TitleBar.xaml 的交互逻辑
    /// </summary>
    public partial class TitleBar : UserControl
    {
        public  object ImageSource 
        {
            get { return imgTitle.Source; }
            set { imgTitle.Source = (Visual)value; } 
        }
        public  string TitleText 
        {
            get { return tbTitle.Text; }
            set { tbTitle.Text = value; }
        }
        public  Brush TitleColor 
        {
            get { return tbTitle.Foreground; }
            set { imgTitle.Foreground = value; tbTitle.Foreground = value;}
        }

        public TitleBar()
        {
            InitializeComponent();
        }
    }
}
