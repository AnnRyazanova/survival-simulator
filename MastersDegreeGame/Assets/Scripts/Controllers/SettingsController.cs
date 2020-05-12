using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SettingsController 
{
    public static SettingsController Instance => _instance ?? (_instance = new SettingsController());
    private static SettingsController _instance;

    private SettingsWindow window;

    public void ShowWindow()
    {
        if (window != null) {
            return;
        }
        
        window = BaseWindow.LoadWindow("SettingsWindow") as SettingsWindow;
        if (window != null) {
            window.OnWindowHide += OnWindowHide;
            window.id = BaseWindow.WindowId.Settings;
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

    private void OnWindowHide()
    {
        window.OnWindowHide -= OnWindowHide;
        window = null;
    }
}
