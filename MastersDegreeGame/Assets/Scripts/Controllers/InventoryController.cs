using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InventoryController 
{
    public static InventoryController Instance => _instance ?? (_instance = new InventoryController());
    private static InventoryController _instance;

    private InventoryWindow window;
    
    public void ShowWindow()
    {
        if (window != null) {
            return;
        }
        
        window = BaseWindow.LoadWindow("InventoryWindow") as InventoryWindow;
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
