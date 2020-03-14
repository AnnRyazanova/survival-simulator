using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWindow : BaseWindow
{
    public static TestWindow Instance => _instance;
    private static TestWindow _instance;

    public static void Show(BaseWindow instance)
    {
        _instance = instance as TestWindow;
    }

    public override void Hide()
    {
        _instance = null;
        base.Hide();
    }

    public void OnButtonClick(string name)
    {
        Debug.Log($"[TestWindow::OnButtonClick] You've clicked on {name}");
    }
}
