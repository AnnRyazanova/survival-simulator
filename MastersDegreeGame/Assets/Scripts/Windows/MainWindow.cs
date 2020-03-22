using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : BaseWindow
{
    public FixedJoystick Joystick;

    [Header("Player indicators")]
    [SerializeField] private Image _warmIndicator;
    [SerializeField] private Image _healthIndicator; 
    [SerializeField] private Image _energyIndicator; 
    
    public override void Show()
    {
        base.Show();
        Init();
    }
    
    public void OnPauseButtonClick()
    {
        // Open SettingsWindow here
        TestController.Instance.ShowWindow();
        Debug.Log($"[MainWindow::OnPauseButtonClick] You've clicked on PauseButton");
    }

    public void OnInventoryButtonClick()
    {
        // Open InventoryWindow here
        Debug.Log($"[MainWindow::OnInventoryButtonClick] You've clicked on InventoryButton");
    }

    public void OnPickUpButtonClick()
    {
        Debug.Log($"[MainWindow::OnPickUpButtonClick] You've clicked on PickUpButton");
    }

    public void OnAttackButtonClick()
    {
        Debug.Log($"[MainWindow::OnAttackButtonClick] You've clicked on AttackButton");
    }

    protected override void OnShow() { }
    protected override void OnHide() { }
    
    private void Init()
    {
        // initialize window here
    }
}
