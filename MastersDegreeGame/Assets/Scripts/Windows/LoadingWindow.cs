using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingWindow : BaseWindow
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;

    public void SetProgress(float progress)
    {
        slider.value = progress;
        var val = Mathf.CeilToInt(progress * 100f);
        progressText.text = val + "%";
    }
    
    private void Awake()
    {
        loadingScreen.SetActive(true);
    }
}
