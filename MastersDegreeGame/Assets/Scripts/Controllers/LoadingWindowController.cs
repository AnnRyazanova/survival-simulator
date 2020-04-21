using UnityEngine;

public sealed class LoadingWindowController
{
    public static LoadingWindowController Instance => _instance ?? (_instance = new LoadingWindowController());
    private static LoadingWindowController _instance;

    private LoadingWindow window;
    
    public void ShowWindow()
    {
        if (window != null) {
            return;
        }
        
        window = BaseWindow.LoadWindow("LoadingWindow") as LoadingWindow;
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
