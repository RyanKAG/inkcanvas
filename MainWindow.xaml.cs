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

            // task : create a method on DrawWindow to handle Undo function
            // Undo Stroke
            case Key.Z:
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    if (drawWindow.InkCanvas.Strokes.Count is not > 0) return;
                    drawWindow.removedStrokes.Add(drawWindow.InkCanvas.Strokes[drawWindow.InkCanvas.Strokes.Count - 1]);
                    drawWindow.InkCanvas.Strokes.RemoveAt(drawWindow.InkCanvas.Strokes.Count - 1);
                }
                break;

            // task : create a method on DrawWindow to handle Redo function
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

