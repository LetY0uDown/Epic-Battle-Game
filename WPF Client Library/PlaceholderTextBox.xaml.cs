using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_Client_Library;

public partial class PlaceholderTextBox : UserControl
{
    static PlaceholderTextBox ()
    {
        PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(PlaceholderTextBox),
                                                          new PropertyMetadata(string.Empty));

        PlaceholderForegroundProperty = DependencyProperty.Register(nameof(PlaceholderForeground), typeof(Brush), typeof(PlaceholderTextBox),
                                                                    new PropertyMetadata(Brushes.LightGray));

        TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(PlaceholderTextBox),
                                                   new PropertyMetadata(string.Empty));
        
        MaxLengthProperty = DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(PlaceholderTextBox),
                                                        new PropertyMetadata(int.MaxValue));
    }

    public PlaceholderTextBox ()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty TextProperty;

    public static readonly DependencyProperty PlaceholderForegroundProperty;

    public static readonly DependencyProperty PlaceholderProperty;

    public static readonly DependencyProperty MaxLengthProperty;

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Brush PlaceholderForeground
    {
        get => (Brush)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
}