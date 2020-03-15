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

    private void Init()
    {
        // initialize window here
    }

    public void OnButtonClick(string name)
    {
        Debug.Log($"[TestWindow::OnButtonClick] You've clicked on {name}");
    }
}
