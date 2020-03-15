using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TestController 
{
    public static TestController Instance => _instance ?? (_instance = new TestController());
    private static TestController _instance;

    private TestWindow window;

    public void ShowWindow()
    {
        if (window != null) {
            return;
        }
        
        window = BaseWindow.LoadWindow("TestWindow") as TestWindow;
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
