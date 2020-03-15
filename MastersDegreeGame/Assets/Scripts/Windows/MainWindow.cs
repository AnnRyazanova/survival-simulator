using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWindow : BaseWindow
{
    public FixedJoystick Joystick;
    public override void Show()
    {
        base.Show();
        Init();
    }

    public void OnTestBtnClick()
    {
        TestController.Instance.ShowWindow();
    }

    private void Init()
    {
        // initialize window here
    }
}
