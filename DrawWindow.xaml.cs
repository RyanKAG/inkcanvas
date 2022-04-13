
using System.Windows;
using System;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Ink;
using System.Windows.Input;
using static inkcanvas.MainWindow;
using System.Windows.Controls;

namespace inkcanvas;

/// <summary>
/// Interaction logic for DrawWindow.xaml
/// </summary>
public partial class DrawWindow : Window
{
    public StrokeCollection removedStrokes = new();
    MainWindow mainWindow;

    public DrawWindow(MainWindow mainWindow, int width, int height, int x, int y, double brushSize, SolidColorBrush solidColorBrush)
    {
        this.Width = width;
        this.Height = height;
        this.Left = x;
        this.Top = y;
        this.mainWindow = mainWindow;

        InitializeComponent();

        InkCanvas.DefaultDrawingAttributes.Height = brushSize;
        InkCanvas.DefaultDrawingAttributes.Width = brushSize;
        InkCanvas.DefaultDrawingAttributes.Color = solidColorBrush.Color;
        InkCanvas.DefaultDrawingAttributes.FitToCurve = true;

        KeyUp += mainWindow.CheckKey_KeyUp;

        this.Show();
    }

    private void DrawWindow_Activated(object sender, EventArgs e)
    {
        SolidColorBrush transparentBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(1, 0, 0, 0));
        this.Background = transparentBrush;
        InkCanvas.Background = transparentBrush;
    }

    private void DrawWindow_Deactivated(object sender, EventArgs e)
    {
        SolidColorBrush transparentBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        this.Background = transparentBrush;
        InkCanvas.Background = transparentBrush;
    }

    public void InkCanvasAttributes_Color(Color color)
    {
        InkCanvas.DefaultDrawingAttributes.Color = color;
    }

    public void InkCanvasAttributes_Size(double size)
    {
        InkCanvas.DefaultDrawingAttributes.Height = size;
        InkCanvas.DefaultDrawingAttributes.Width = size;
    }

    public void InkCanvasStrokes_Clear()
    {
        InkCanvas.Strokes.Clear();
    }

    // task : rename to something ?
    private void TestColor(object sender, InkCanvasSelectionChangingEventArgs e)
    {
        Console.WriteLine(sender);
        Console.WriteLine(e.GetSelectedStrokes()[0].DrawingAttributes.Color);
        Console.WriteLine(e.GetSelectedStrokes()[0].DrawingAttributes.Width);
        InkCanvasAttributes_Color(e.GetSelectedStrokes()[0].DrawingAttributes.Color);
        InkCanvasAttributes_Size(e.GetSelectedStrokes()[0].DrawingAttributes.Width);
        mainWindow.settingsWindow.ColorPicker_Set(new SolidColorBrush(e.GetSelectedStrokes()[0].DrawingAttributes.Color));
        mainWindow.settingsWindow.BrushEllipse_Set(e.GetSelectedStrokes()[0].DrawingAttributes.Width);
        e.Cancel = true;
    }

    public void InkCanvasEditingMode_Set(string mode)
    {
        mainWindow.EraserMode.Foreground = new SolidColorBrush(Colors.Black);
        mainWindow.InkMode.Foreground = new SolidColorBrush(Colors.Black);
        mainWindow.EyeDropperMode.Foreground = new SolidColorBrush(Colors.Black);
        mainWindow.SelectMode.Foreground = new SolidColorBrush(Colors.Black);
        InkCanvas.SelectionChanging -= TestColor;

        switch (mode)
        {
            case "erase":
                mainWindow.EraserMode.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                InkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                break;

            case "ink":
                mainWindow.InkMode.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                break;

            case "select":
                mainWindow.SelectMode.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                InkCanvas.EditingMode = InkCanvasEditingMode.Select;
                break;

            case "dropper":
                mainWindow.EyeDropperMode.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                InkCanvas.EditingMode = InkCanvasEditingMode.Select;
                InkCanvas.SelectionChanging += TestColor;
                break;

            default:
                break;
        }
    }
}

