using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Controllers;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : BaseWindow
{
    public FixedJoystick Joystick;
    [SerializeField] private PlayerNeedsUiView _playerNeeds;
    
    public override void Show()
    {
        base.Show();
        Init();
    }
    
    public void OnPauseButtonClick()
    {
        // Open SettingsWindow here
        TestController.Instance.ShowWindow();
    }

    public void OnInventoryButtonClick()
    {
        InventoryController.Instance.ShowWindow();
    }

    public void OnCraftButtonClick()
    {
        CraftController.Instance.ShowWindow();
    }

    public void OnAttackButtonClick()
    {
        PlayerMainScript.MyPlayer.Attack();
    }
    
    public void OnPickUpButtonClick()
    {
        
    }

    protected override void OnShow() { }
    protected override void OnHide() { }
    
    private void Init()
    {
        StartCoroutine(_playerNeeds.Start());
    }
}
