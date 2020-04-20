using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CraftSlot : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _count;
        [SerializeField] private GameObject _fader;

        private void Awake()
        {
            _icon.gameObject.SetActive(false);
            _count.gameObject.SetActive(false);
            _fader.SetActive(false);
        }
    }
}
