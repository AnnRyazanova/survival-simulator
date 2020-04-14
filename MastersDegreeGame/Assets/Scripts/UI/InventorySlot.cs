using System;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public sealed class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _count;

    private InventoryWindow _window;
    
    public void Init(InventoryWindow window, Sprite sprite = null, int count = 0)
    {
        _window = window;
        if (sprite != null) {
            _icon.sprite = sprite;
            _icon.gameObject.SetActive(true);
        }

        if (count > 1) {
            _count.SetText(count.ToString());
            _count.gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        _icon.gameObject.SetActive(false);
        _count.gameObject.SetActive(false);
    }
}
