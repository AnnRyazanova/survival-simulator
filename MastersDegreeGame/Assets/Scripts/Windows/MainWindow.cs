using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Controllers;
using Characters.Player;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : BaseWindow
{
    public FixedJoystick Joystick;
    [SerializeField] private PlayerNeedsUiView _playerNeeds;
    [SerializeField] protected float attackRate = 1.5f;
        
    protected float LastAttackTime = 0.0f;
    
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
        if (Time.time >= LastAttackTime) {
            PlayerMainScript.MyPlayer.Attack();
            LastAttackTime = Time.time + 1f / attackRate;
        }
    }
    
    public void OnPickUpButtonClick()
    {
        PlayerMainScript.MyPlayer.InteractWithClosestItem();
    }

    protected override void OnShow() { }
    protected override void OnHide() { }
    
    private void Init()
    {
        StartCoroutine(_playerNeeds.Start());
    }
}
