using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWindow : BaseWindow
{
    public void OnMenuButtonClick()
    {
        Debug.Log($"[TestWindow::OnMenuButtonClick] You've clicked on MenuButton");
        Hide();
        MainWindowController.Instance.HideWindow();
        MenuController.Instance.ShowWindow();
        SceneLoaderController.Instance.LoadMenuScene(false, false);
    }
}
