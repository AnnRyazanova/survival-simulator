using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CraftController 
{
    public static CraftController Instance => _instance ?? (_instance = new CraftController());
    private static CraftController _instance;

    private CraftWindow window;
    
    public void ShowWindow()
    {
        if (window != null) {
            return;
        }
        
        window = BaseWindow.LoadWindow("CraftWindow") as CraftWindow;
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

    private void OnWindowHide()
    {
        window.OnWindowHide -= OnWindowHide;
        window = null;
    }
}
