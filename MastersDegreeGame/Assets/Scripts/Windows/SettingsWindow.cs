using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsWindow : BaseWindow
{
    public void OnButtonClick(string name)
    {
        Hide();
        MainWindowController.Instance.HideWindow();
        MenuController.Instance.ShowWindow();
        SceneLoaderController.Instance.LoadMenuScene(false, false);
    }
}
