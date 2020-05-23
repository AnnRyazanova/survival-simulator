using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathWindow : BaseWindow
{
    [SerializeField] private Text _description; 
    
    public override void Show()
    {
        base.Show();
        Init();
    }
    
    public void OnMenuButtonClick()
    {
        Debug.Log($"[TestWindow::OnMenuButtonClick] You've clicked on MenuButton");
        Hide();
        MenuController.Instance.ShowWindow();
        SceneLoaderController.Instance.LoadMenuScene(false, false);
    }

    private void Init()
    {
        _description.text = string.Format(_description.text, DayNightCycleController.Get.DaysAmount);
    }
}
