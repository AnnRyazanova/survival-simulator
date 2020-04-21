using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWindow : BaseWindow
{
    public override void Show()
    {
        base.Show();
        Init();
    }
    
    public void OnButtonClick(string name)
    {
        Debug.Log($"[TestWindow::OnButtonClick] You've clicked on {name}");
        if (name == "МЕНЮ")
        {
            Hide();
            MainWindowController.Instance.HideWindow();
            MenuController.Instance.ShowWindow();
            SceneLoaderController.Instance.LoadMenuScene(false, false);
        }
        
    }

    private void Init()
    {
        // initialize window here
    }
}
