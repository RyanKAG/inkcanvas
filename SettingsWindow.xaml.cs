using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace inkcanvas;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    byte[][] defaultSwatchesValue = new byte[][]
    {
        new byte[] {255,0, 0, 0},
        new byte[] {255,68, 132, 255},
        new byte[] {255,73, 255, 244},
        new byte[] {255,201, 91, 253},
        new byte[] {255,251, 255, 78},
        new byte[] {255,252, 148, 52},
        new byte[] {255,255, 54, 54},
        new byte[] {255,255, 255, 255},
    };

    public MainWindow mainWindow;
    public ObservableCollection<SolidColorBrush> defaultSwatches = new();
    public ObservableCollection<SolidColorBrush> colorSwatches = new();

    public SettingsWindow(MainWindow mainWindow, SolidColorBrush solidColorBrush, double brushSize)
    {
        this.mainWindow = mainWindow;

        InitializeComponent();

        mainWindow.ColorSwatch.Background = solidColorBrush;
        ColorPicker.Color = solidColorBrush.Color;
        BrushSizeSlider.Value = brushSize;
        BrushEllipse_Set(brushSize);
        DefaultSwatches_Init(solidColorBrush);

        KeyUp += mainWindow.CheckKey_KeyUp;

        this.Show();
    }

    public void BrushEllipse_Set(double brushSize)
    {
        BrushEllipse.Fill = mainWindow.solidColorBrush;
        BrushEllipse.Stroke = new SolidColorBrush(Colors.Black);
        BrushEllipse.StrokeThickness = 1;
        BrushEllipse.Width = brushSize;
        BrushEllipse.Height = brushSize;
        Canvas.SetLeft(BrushEllipse, 50 - brushSize / 2);
        Canvas.SetTop(BrushEllipse, 50 - brushSize / 2);
    }

    private void BrushSizeSlider_ValueChanged(object sender, EventArgs e)
    {
        mainWindow.brushSize = BrushSizeSlider.Value;
        BrushEllipse_Set(BrushSizeSlider.Value);
        mainWindow.DrawAttributes_Size();
    }

    private void DefaultSwatches_Init(SolidColorBrush solidColorBrush)
    {
        foreach (byte[] values in defaultSwatchesValue)
        {
            defaultSwatches.Add(new SolidColorBrush(Color.FromArgb(values[0], values[1], values[2], values[3])));
        }
        DefaultSwatches.ItemsSource = defaultSwatches;
    }

    public void ColorSwatches_Add(SolidColorBrush solidColorBrush)
    {
        if (colorSwatches.Contains(solidColorBrush)) return;
        if (colorSwatches.Count > 3)
        {
            colorSwatches.RemoveAt(0);
        }
        colorSwatches.Add(solidColorBrush);
        ColorSwatches.ItemsSource = colorSwatches;
    }

    public void ColorPicker_Set(SolidColorBrush solidColorBrush)
    {
        mainWindow.solidColorBrush = solidColorBrush;
        mainWindow.ColorSwatch.Background = solidColorBrush;
        BrushEllipse.Fill = solidColorBrush;
        BrushEllipse.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(155, 0, 0, 0));
        BrushEllipse.StrokeThickness = 1;
        mainWindow.DrawAttributes_Color(solidColorBrush);
    }

    private void ColorPicker_MouseUp(object sender, MouseButtonEventArgs e)
    {

        SolidColorBrush newSolidBrush = new SolidColorBrush(ColorPicker.Color);
        ColorPicker_Set(newSolidBrush);
        ColorSwatches_Add(newSolidBrush);
        mainWindow.DrawAttributes_Color(mainWindow.solidColorBrush);
    }

    private void ColorSwatch_Set(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;
        if (button.Background is not SolidColorBrush brush) return;
        ColorPicker_Set(brush);
    }
}