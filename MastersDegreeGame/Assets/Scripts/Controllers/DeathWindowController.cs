using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DeathWindowController 
{
    public static DeathWindowController Instance => _instance ?? (_instance = new DeathWindowController());
    private static DeathWindowController _instance;

    private DeathWindow window;

    public void ShowWindow()
    {
        if (window != null) {
            return;
        }
        
        window = BaseWindow.LoadWindow("DeathWindow") as DeathWindow;
        if (window != null) {
            window.OnWindowHide += OnWindowHide;
            window.Show();
        }
        
        MainWindowController.Instance.HideWindow();
    }

    private void OnWindowHide()
    {
        window.OnWindowHide -= OnWindowHide;
        window = null;
    }
}
