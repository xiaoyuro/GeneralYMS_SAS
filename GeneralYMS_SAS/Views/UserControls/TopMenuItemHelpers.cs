using System.Windows;
using System.Windows.Media;

namespace GeneralYMS_SAS.Views.UserControls
{
    public static class TopMenuItemHelpers
    {
        #region 附加属性

        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.RegisterAttached("IconSource", typeof(ImageSource), typeof(TopMenuItemHelpers), new PropertyMetadata(null));

        public static readonly DependencyProperty IconHoverSourceProperty =
            DependencyProperty.RegisterAttached("IconHoverSource", typeof(ImageSource), typeof(TopMenuItemHelpers), new PropertyMetadata(null));

        public static readonly DependencyProperty IconPressSourceProperty =
            DependencyProperty.RegisterAttached("IconPressSource", typeof(ImageSource), typeof(TopMenuItemHelpers), new PropertyMetadata(null));

        public static readonly DependencyProperty IconDisabledSourceProperty =
            DependencyProperty.RegisterAttached("IconDisabledSource", typeof(ImageSource), typeof(TopMenuItemHelpers), new PropertyMetadata(null));

        #endregion

        #region IconSource
        public static ImageSource GetIconSource(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(IconSourceProperty);
        }

        public static void SetIconSource(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(IconSourceProperty, value);
        }

        #endregion

        #region IconHoverSource

        public static ImageSource GetIconHoverSource(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(IconHoverSourceProperty);
        }

        public static void SetIconHoverSource(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(IconHoverSourceProperty, value);
        }

        #endregion

        #region IconPressSource

        public static ImageSource GetIconPressSource(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(IconPressSourceProperty);
        }

        public static void SetIconPressSource(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(IconPressSourceProperty, value);
        }

        #endregion

        #region IconDisabledSource

        public static ImageSource GetIconDisabledSource(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(IconDisabledSourceProperty);
        }

        public static void SetIconDisabledSource(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(IconDisabledSourceProperty, value);
        }

        #endregion
    }
}
