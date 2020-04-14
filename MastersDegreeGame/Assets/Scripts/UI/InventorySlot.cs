using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _icon;

    private InventoryWindow _window;

    public void Init(InventoryWindow window, Sprite sprite = null)
    {
        _window = window;
        if (sprite != null) {
            _icon.sprite = sprite;
            _icon.gameObject.SetActive(true);
        }
    }
    
    private void Awake()
    {
        _icon.gameObject.SetActive(false);
    }
}
