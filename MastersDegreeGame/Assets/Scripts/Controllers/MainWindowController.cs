using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MainWindowController 
{
    public static MainWindowController Instance => _instance ?? (_instance = new MainWindowController());
    private static MainWindowController _instance;

    private MainWindow window;

    public void ShowWindow()
    {
        if (window != null) {
            return;
        }
        
        window = BaseWindow.LoadWindow("MainWindow") as MainWindow;
        if (window != null) {
            window.OnWindowHide += OnWindowHide;
            window.Show();
        }
    }

    public void HideWindow()
    {
        if (window == null) {
            return;
        }
        
        window.Hide();
    }

    public FixedJoystick GetJoystick()
    {
        return window.Joystick;
    }

    private void OnWindowHide()
    {
        window.OnWindowHide -= OnWindowHide;
        window = null;
    }
}
