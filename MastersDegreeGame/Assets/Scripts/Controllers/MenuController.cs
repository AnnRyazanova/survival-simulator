using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MenuController 
{
    public static MenuController Instance => _instance ?? (_instance = new MenuController());
    private static MenuController _instance;

    private MenuWindow window;
    
    public void ShowWindow()
    {
        if(window != null){return;}
        
        window = BaseWindow.LoadWindow("MenuWindow") as MenuWindow;
        if (window != null)
        {
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
