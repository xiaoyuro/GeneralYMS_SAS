using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GeneralYMS_SAS.Views.UserControls
{ 
    public class TopMenuItem : MenuItem
    {
        #region 依赖属性

        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(TopMenuItem), new PropertyMetadata(null));

        public static readonly DependencyProperty IconHoverSourceProperty =
            DependencyProperty.Register("IconHoverSource", typeof(ImageSource), typeof(TopMenuItem), new PropertyMetadata(null));

        public static readonly DependencyProperty IconDisabledSourceProperty =
            DependencyProperty.Register("IconDisabledSource", typeof(ImageSource), typeof(TopMenuItem), new PropertyMetadata(null));

        public static readonly DependencyProperty IconPressedSourceProperty =
            DependencyProperty.Register("IconPressedSource", typeof(ImageSource), typeof(TopMenuItem), new PropertyMetadata(null));

        #endregion

        #region 属性

        public ImageSource IconSource
        {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        public ImageSource IconHoverSource
        {
            get { return (ImageSource)GetValue(IconHoverSourceProperty); }
            set { SetValue(IconHoverSourceProperty, value); }
        }

        public ImageSource IconDisabledSource
        {
            get { return (ImageSource)GetValue(IconDisabledSourceProperty); }
            set { SetValue(IconDisabledSourceProperty, value); }
        }

        public ImageSource IconPressedSource
        {
            get { return (ImageSource)GetValue(IconPressedSourceProperty); }
            set { SetValue(IconPressedSourceProperty, value); }
        }

        #endregion
    }
}
