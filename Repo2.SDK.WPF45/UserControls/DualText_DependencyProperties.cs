using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Repo2.SDK.WPF45.UserControls
{
    public partial class DualText : UserControl
    {
        public string Text1
        {
            get { return (string)GetValue(Text1Property); }
            set { SetValue(Text1Property, value); }
        }
        public static readonly DependencyProperty Text1Property =
            DependencyProperty.Register("Text1", typeof(string), typeof(DualText));

        public string Text2
        {
            get { return (string)GetValue(Text2Property); }
            set { SetValue(Text2Property, value); }
        }
        public static readonly DependencyProperty Text2Property =
            DependencyProperty.Register("Text2", typeof(string), typeof(DualText));



        public GridLength? Text1Width
        {
            get { return (GridLength?)GetValue(Text1WidthProperty); }
            set { SetValue(Text1WidthProperty, value); }
        }
        public static readonly DependencyProperty Text1WidthProperty =
            DependencyProperty.Register("Text1Width", typeof(GridLength?), typeof(DualText));

        public GridLength? GapWidth
        {
            get { return (GridLength?)GetValue(GapWidthProperty); }
            set { SetValue(GapWidthProperty, value); }
        }
        public static readonly DependencyProperty GapWidthProperty =
            DependencyProperty.Register("GapWidth", typeof(GridLength?), typeof(DualText));

        public GridLength? Text2Width
        {
            get { return (GridLength?)GetValue(Text2WidthProperty); }
            set { SetValue(Text2WidthProperty, value); }
        }
        public static readonly DependencyProperty Text2WidthProperty =
            DependencyProperty.Register("Text2Width", typeof(GridLength?), typeof(DualText));



        public TextAlignment? Text1Alignment
        {
            get { return (TextAlignment?)GetValue(Text1AlignmentProperty); }
            set { SetValue(Text1AlignmentProperty, value); }
        }
        public static readonly DependencyProperty Text1AlignmentProperty =
            DependencyProperty.Register("Text1Alignment", typeof(TextAlignment?), typeof(DualText));

        public TextAlignment? Text2Alignment
        {
            get { return (TextAlignment?)GetValue(Text2AlignmentProperty); }
            set { SetValue(Text2AlignmentProperty, value); }
        }
        public static readonly DependencyProperty Text2AlignmentProperty =
            DependencyProperty.Register("Text2Alignment", typeof(TextAlignment?), typeof(DualText));



        public TextWrapping Text1Wrapping
        {
            get { return (TextWrapping)GetValue(Text1WrappingProperty); }
            set { SetValue(Text1WrappingProperty, value); }
        }
        public static readonly DependencyProperty Text1WrappingProperty =
            DependencyProperty.Register("Text1Wrapping", typeof(TextWrapping), typeof(DualText));

        public TextWrapping Text2Wrapping
        {
            get { return (TextWrapping)GetValue(Text2WrappingProperty); }
            set { SetValue(Text2WrappingProperty, value); }
        }
        public static readonly DependencyProperty Text2WrappingProperty =
            DependencyProperty.Register("Text2Wrapping", typeof(TextWrapping), typeof(DualText));



        public Brush Text1Brush
        {
            get { return (Brush)GetValue(Text1BrushProperty); }
            set { SetValue(Text1BrushProperty, value); }
        }
        public static readonly DependencyProperty Text1BrushProperty =
            DependencyProperty.Register("Text1Brush", typeof(Brush), typeof(DualText));

        public Brush Text2Brush
        {
            get { return (Brush)GetValue(Text2BrushProperty); }
            set { SetValue(Text2BrushProperty, value); }
        }
        public static readonly DependencyProperty Text2BrushProperty =
            DependencyProperty.Register("Text2Brush", typeof(Brush), typeof(DualText));



        public FontWeight? Text1Weight
        {
            get { return (FontWeight?)GetValue(Text1WeightProperty); }
            set { SetValue(Text1WeightProperty, value); }
        }
        public static readonly DependencyProperty Text1WeightProperty =
            DependencyProperty.Register("Text1Weight", typeof(FontWeight?), typeof(DualText));

        public FontWeight? Text2Weight
        {
            get { return (FontWeight?)GetValue(Text2WeightProperty); }
            set { SetValue(Text2WeightProperty, value); }
        }
        public static readonly DependencyProperty Text2WeightProperty =
            DependencyProperty.Register("Text2Weight", typeof(FontWeight?), typeof(DualText));



        public FontStyle Text1FontStyle
        {
            get { return (FontStyle)GetValue(Text1FontStyleProperty); }
            set { SetValue(Text1FontStyleProperty, value); }
        }
        public static readonly DependencyProperty Text1FontStyleProperty =
            DependencyProperty.Register("Text1FontStyle", typeof(FontStyle), typeof(DualText));

        public FontStyle Text2FontStyle
        {
            get { return (FontStyle)GetValue(Text2FontStyleProperty); }
            set { SetValue(Text2FontStyleProperty, value); }
        }
        public static readonly DependencyProperty Text2FontStyleProperty =
            DependencyProperty.Register("Text2FontStyle", typeof(FontStyle), typeof(DualText));



        public double? Text1Size
        {
            get { return (double?)GetValue(Text1SizeProperty); }
            set { SetValue(Text1SizeProperty, value); }
        }
        public static readonly DependencyProperty Text1SizeProperty =
            DependencyProperty.Register("Text1Size", typeof(double?), typeof(DualText));

        public double? Text2Size
        {
            get { return (double?)GetValue(Text2SizeProperty); }
            set { SetValue(Text2SizeProperty, value); }
        }
        public static readonly DependencyProperty Text2SizeProperty =
            DependencyProperty.Register("Text2Size", typeof(double?), typeof(DualText));



        public Visibility Text1Ellipsis
        {
            get { return (Visibility)GetValue(Text1EllipsisProperty); }
            set { SetValue(Text1EllipsisProperty, value); }
        }
        public static readonly DependencyProperty Text1EllipsisProperty =
            DependencyProperty.Register("Text1Ellipsis", typeof(Visibility), typeof(DualText));

        public Visibility Text2Ellipsis
        {
            get { return (Visibility)GetValue(Text2EllipsisProperty); }
            set { SetValue(Text2EllipsisProperty, value); }
        }
        public static readonly DependencyProperty Text2EllipsisProperty =
            DependencyProperty.Register("Text2Ellipsis", typeof(Visibility), typeof(DualText));
    }
}
