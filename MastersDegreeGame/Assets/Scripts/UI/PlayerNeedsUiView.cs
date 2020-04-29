using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Controllers;
using Characters.Player;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerNeedsUiView : MonoBehaviour
{
    [Header("Player indicators")]
    [SerializeField] private Image _warmProgressBar;
    [SerializeField] private Text _warmText;
    
    [SerializeField] private Image _healthProgressBar;
    [SerializeField] private Text _healthText;
    
    [SerializeField] private Image _energyProgressBar;
    [SerializeField] private Text _energyText;
    
    [SerializeField] private Image _hungerProgressBar;
    [SerializeField] private Text _hungerText;
    [Space (20)]
    
    [SerializeField] private Image _vignetteBkg;

    private Action OnNeedUpdate = () => { };

    private void Awake()
    {
#if CHEAT
        _healthText.gameObject.SetActive(true);
        _energyText.gameObject.SetActive(true);
        _warmText.gameObject.SetActive(true);
        _hungerText.gameObject.SetActive(true);
#endif
        _vignetteBkg.gameObject.SetActive(false);
        OnNeedUpdate += CheckVignette;
    }

    public IEnumerator Start()
    {
        while (PlayerMainScript.MyPlayer == null || PlayerMainScript.MyPlayer.playerObject == null) yield return null;

        UpdateHealth();
        UpdateWarm();
        UpdateEnergy();
        UpdateHunger();
        PlayerMainScript.MyPlayer.playerObject.Health.OnChange += UpdateHealth;
        PlayerMainScript.MyPlayer.playerObject.Warm.OnChange += UpdateWarm;
        PlayerMainScript.MyPlayer.playerObject.Energy.OnChange += UpdateEnergy;
        PlayerMainScript.MyPlayer.playerObject.Hunger.OnChange += UpdateHunger;
    }
    
    private void OnDestroy()
    {
        if (PlayerMainScript.MyPlayer != null && PlayerMainScript.MyPlayer.playerObject != null) {
            PlayerMainScript.MyPlayer.playerObject.Health.OnChange -= UpdateHealth;
            PlayerMainScript.MyPlayer.playerObject.Warm.OnChange -= UpdateWarm;
            PlayerMainScript.MyPlayer.playerObject.Energy.OnChange -= UpdateEnergy;
            PlayerMainScript.MyPlayer.playerObject.Hunger.OnChange -= UpdateHunger;
        }
        OnNeedUpdate -= CheckVignette;
    }

    private void UpdateEnergy()
    {
        var energy = PlayerMainScript.MyPlayer.playerObject.Energy;
        _energyProgressBar.fillAmount = energy.CurrentPointsNormalized;
#if CHEAT
        _energyText.text = energy.CurrentPoints.ToString();
#endif
    }

    private void UpdateWarm()
    {
        var warm = PlayerMainScript.MyPlayer.playerObject.Warm;
        _warmProgressBar.fillAmount = warm.CurrentPointsNormalized;
#if CHEAT
        _warmText.text = warm.CurrentPoints.ToString();
#endif
        OnNeedUpdate();
    }

    private void UpdateHealth()
    {
        var health = PlayerMainScript.MyPlayer.playerObject.Health;
        _healthProgressBar.fillAmount = health.CurrentPointsNormalized;
#if CHEAT
        _healthText.text = health.CurrentPoints.ToString();
#endif
        OnNeedUpdate();
    }

    private void UpdateHunger()
    {
        var hunger = PlayerMainScript.MyPlayer.playerObject.Hunger;
        _hungerProgressBar.fillAmount = hunger.CurrentPointsNormalized;
#if CHEAT
        _hungerText.text = hunger.CurrentPoints.ToString();
#endif
        OnNeedUpdate();
    }

    private void CheckVignette()
    {
        var isCritical = PlayerMainScript.MyPlayer.playerObject.Hunger.IsCritical ||
            PlayerMainScript.MyPlayer.playerObject.Warm.IsCritical;
        
        if (isCritical) {

            if (_vignetteBkg.gameObject.activeSelf == false) {
                _vignetteBkg.gameObject.SetActive(true);
            }
            _vignetteBkg.color = GetColor(_vignetteBkg.color);
        } 
        
        if (isCritical == false && _vignetteBkg.gameObject.activeSelf) {
            _vignetteBkg.gameObject.SetActive(false);
        }
    }

    private Color GetColor(Color color)
    {
        var _color = color;
        var currentHP = PlayerMainScript.MyPlayer.playerObject.Health.CurrentPoints;
        
        var alpha = 255 - (currentHP * 2.55f);
        var res = alpha / 255;

        _color.a = res;
        return _color;
    }
}
