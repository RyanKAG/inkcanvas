using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace inkcanvas;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public enum EditingMode
    {
        EraseBy,
        Ink,
    }

    public SolidColorBrush solidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 255));
    public double brushSize = 16;

    public DrawWindow drawWindow;
    public SettingsWindow settingsWindow;

    public MainWindow()
    {
        InitializeComponent();

        KeyUp += CheckKey_KeyUp;


        drawWindow = DrawWindow_Init(solidColorBrush);
        drawWindow.InkCanvasEditingMode_Set("ink");

        settingsWindow = new SettingsWindow(this, solidColorBrush, brushSize);
    }

    public void CheckKey_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            // Select Black Brush
            case Key.D1:
                drawWindow.Focus();
                settingsWindow.ColorPicker_Set(new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)));
                break;

            // Select White Brush
            case Key.D2:
                drawWindow.Focus();
                settingsWindow.ColorPicker_Set(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)));
                break;

            // Select Blue Brush
            case Key.D3:
                drawWindow.Focus();
                settingsWindow.ColorPicker_Set(new SolidColorBrush(Color.FromArgb(255, 68, 132, 255)));
                break;

            // Select Teal Brush
            case Key.D4:
                drawWindow.Focus();
                settingsWindow.ColorPicker_Set(new SolidColorBrush(Color.FromArgb(255, 73, 255, 244)));
                break;

            // Select Purple Brush
            case Key.D5:
                drawWindow.Focus();
                settingsWindow.ColorPicker_Set(new SolidColorBrush(Color.FromArgb(255, 201, 91, 253)));
                break;

            // Select Yellow Brush
            case Key.D6:
                drawWindow.Focus();
                settingsWindow.ColorPicker_Set(new SolidColorBrush(Color.FromArgb(255, 251, 255, 78)));
                break;

            // Select Orange Brush
            case Key.D7:
                drawWindow.Focus();
                settingsWindow.ColorPicker_Set(new SolidColorBrush(Color.FromArgb(255, 252, 148, 52)));
                break;

            // Select Red Brush
            case Key.D8:
                drawWindow.Focus();
                settingsWindow.ColorPicker_Set(new SolidColorBrush(Color.FromArgb(255, 255, 54, 54)));
                break;

            // Focus MainWindow
            case Key.A:
                this.Focus();
                break;

            // Focus SettingsWindow
            case Key.S:
                settingsWindow.Focus();
                break;

            // Use Ink
            case Key.D:
                drawWindow.Focus();
                drawWindow.InkCanvasEditingMode_Set("ink");
                break;

            // Use Clear
            case Key.Back:
                drawWindow.InkCanvasStrokes_Clear();
                break;

            // Use Eraser
            case Key.E:
                drawWindow.Focus();
                drawWindow.InkCanvasEditingMode_Set("erase");
                break;

            // Use Ink Dropper
            case Key.I:
                drawWindow.Focus();
                drawWindow.InkCanvasEditingMode_Set("dropper");
                break;

            // Use Select
            case Key.M:
                drawWindow.Focus();
                drawWindow.InkCanvasEditingMode_Set("select");
                break;

            // Undo Stroke
            case Key.Z:
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    if (drawWindow.InkCanvas.Strokes.Count is not > 0) return;
                    drawWindow.removedStrokes.Add(drawWindow.InkCanvas.Strokes[drawWindow.InkCanvas.Strokes.Count - 1]);
                    drawWindow.InkCanvas.Strokes.RemoveAt(drawWindow.InkCanvas.Strokes.Count - 1);
                }
                break;

            // Redo Stroke
            case Key.Y:
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    if (drawWindow.removedStrokes.Count is not > 0) return;
                    drawWindow.InkCanvas.Strokes.Add(drawWindow.removedStrokes[drawWindow.removedStrokes.Count - 1]);
                    drawWindow.removedStrokes.RemoveAt(drawWindow.removedStrokes.Count - 1);
                }
                break;

            default:
                break;
        }
    }

    private void ClearStrokes_Click(object sender, RoutedEventArgs e)
    {
        drawWindow.InkCanvasStrokes_Clear();
    }

    public DrawWindow DrawWindow_Init(SolidColorBrush solidColorBrush)
    {
        int minX = 0, maxX = 0, minY = 0, maxY = 0;

        foreach (var screen in Screen.AllScreens)
        {
            if (screen.Bounds.X < minX)
            {
                minX = screen.Bounds.X;
            }
            if (screen.Bounds.X + screen.Bounds.Width > maxX)
            {
                maxX = screen.Bounds.X + screen.Bounds.Width;
            }
            if (screen.Bounds.Y < minY)
            {
                minY = screen.Bounds.Y;
            }
            if (screen.Bounds.Y + screen.Bounds.Height > maxY)
            {
                maxY = screen.Bounds.Y + screen.Bounds.Height;
            }
        }

        int height = maxY - minY;
        int width = maxX - minX;

        DrawWindow drawWindow = new DrawWindow(this, width, height, minX, minY, brushSize, solidColorBrush);
        drawWindow.Show();

        return drawWindow;
    }

    public void DrawAttributes_Color(SolidColorBrush solidColorBrush)
    {
        drawWindow.InkCanvas.DefaultDrawingAttributes.Color = solidColorBrush.Color;
    }

    public void DrawAttributes_Size()
    {
        drawWindow.InkCanvas.DefaultDrawingAttributes.Width = brushSize;
        drawWindow.InkCanvas.DefaultDrawingAttributes.Height = brushSize;
    }

    private void SettingsWindow_Open()
    {
        settingsWindow.Close();
        this.settingsWindow = new SettingsWindow(this, solidColorBrush, brushSize);
        settingsWindow.Show();
    }

    private void MainWindowButton_Click(object sender, RoutedEventArgs e)
    {

        if (sender is not System.Windows.Controls.Button button) return;
        switch (button.Tag)
        {
            case "clear":
                drawWindow.InkCanvasStrokes_Clear();
                break;

            case "erase":
                drawWindow.Focus();
                drawWindow.InkCanvasEditingMode_Set("erase");
                break;

            case "ink":
                drawWindow.Focus();
                drawWindow.InkCanvasEditingMode_Set("ink");
                break;

            case "select":
                drawWindow.Focus();
                drawWindow.InkCanvasEditingMode_Set("select");
                break;

            case "dropper":
                drawWindow.Focus();
                drawWindow.InkCanvasEditingMode_Set("dropper");
                break;

            case "settings":
                SettingsWindow_Open();
                break;

            default:
                break;
        }
    }

    private void MainWindow_Closing(object sender, CancelEventArgs e)
    {
        drawWindow.Close();
        settingsWindow.Close();
    }
}

