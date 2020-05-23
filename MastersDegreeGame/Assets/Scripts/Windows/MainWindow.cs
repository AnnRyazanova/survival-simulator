using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Controllers;
using Characters.Player;
using InventoryObjects.Items;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : BaseWindow
{
    public FixedJoystick Joystick;
    [SerializeField] private PlayerNeedsUiView _playerNeeds;
    [SerializeField] private Button _attakcButton;

    public override void Show() {
        base.Show();
        Init();
    }

    private void Update() {
        _attakcButton.interactable = PlayerMainScript.MyPlayer.equipment.weapon != null &&
                                     PlayerMainScript.MyPlayer.equipment.weapon.item is WeaponItem;
    }

    public void OnPauseButtonClick() {
        // Open SettingsWindow here
        SettingsController.Instance.ShowWindow();
    }

    public void OnInventoryButtonClick() {
        InventoryController.Instance.ShowWindow();
    }

    public void OnCraftButtonClick() {
        CraftController.Instance.ShowWindow();
    }

    public void OnAttackButtonClick() {
        var player = PlayerMainScript.MyPlayer;
        if (Time.time >= player.lastAttackTime) {
            PlayerMainScript.MyPlayer.Attack();
            player.lastAttackTime = Time.time + 1f / player.attackRate;
        }
    }

    public void OnPickUpButtonClick() {
       PlayerMainScript.MyPlayer.InteractWithClosestItem();
    }

    protected override void OnShow() {
    }

    protected override void OnHide() {
    }

    private void Init() {
        StartCoroutine(_playerNeeds.Start());
    }
}