using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Rendering;

public class DayNightCycleController : MonoBehaviour
{
    [SerializeField] private Light _sun;
    [SerializeField] private float _secInDay = 120f;
    [SerializeField] private Gradient _sunColor;
    [SerializeField] private Gradient _skyBoxColor;
    
    // 0 is midnight
    // 0.25 is sunrise
    // 0.5 is noon
    // 0.75 is sunset
    [SerializeField] [Range(0,1)] public float _currentTimeOfDay = 0;
    
    public static DayNightCycleController Get { get; private set; }

    public int DaysAmount { get; private set; }
    
    private float _timeMultiplier = 1f;
    private float _sunStartIntensity;

    private void Awake()
    {
        Get = this;
    }

    private void OnDestroy()
    {
        Get = null;
    }
    
    private void Start()
    {
        _sunStartIntensity = _sun.intensity;

        // При старте сцены всегда будет рассвет, потом можно поменять
        _currentTimeOfDay = 0.3f;
        DaysAmount = 1;
        
        RenderSettings.ambientMode = AmbientMode.Trilight;
    }

    private void Update()
    {
        UpdateSun();
        
        _currentTimeOfDay += (Time.deltaTime / _secInDay) * _timeMultiplier;
 
        if (_currentTimeOfDay >= 1) {
            _currentTimeOfDay = 0;
            DaysAmount++;
        }
    }

    private void UpdateSun()
    {
        // We rotate the sun 360 degrees around the x-axis according to the current time of day.
        // We subtract 90 degrees from this to make the sun rise at 0.25 instead of 0.
        // I just found that value easier to work with.
        // The y-axis determines where on the horizon the sun will rise and set.
        // I set this to 170 because it fit test scene. The z-axis rotation does nothing here.
        _sun.transform.localRotation = Quaternion.Euler((_currentTimeOfDay * 360f) - 90, 170, 0);

        UpdateSunIntensity();
        UpdateSunColor();
        UpdateSkyBox();
    }

    private void UpdateSunColor()
    {
        _sun.color = _sunColor.Evaluate(_currentTimeOfDay);
    }

    private void UpdateSunIntensity()
    {
        if (_currentTimeOfDay > 0.4f && _currentTimeOfDay < 0.6f) {
            return;
        }
        
        float intensityMultiplier = 1;
        
        // We want the intensity of the sun to be 0 when it's below the horizon so it doesn't shine up from below the ground
       
        if (_currentTimeOfDay <= 0.23f || _currentTimeOfDay >= 0.75f) {
            intensityMultiplier = 0;
        } 
        else if (_currentTimeOfDay <= 0.4f) { 
            // in the range from 0.23 to 0.4 intensity should change from 0 to 1
            // 0.4 - 0.23 = 0.17
            // 1 / 0.17 ~ 5.88
            // (t - 0.23) * 5.88 = I
            intensityMultiplier = Mathf.Clamp01((_currentTimeOfDay - 0.23f) * 5.88f);
        }
        else if (_currentTimeOfDay >= 0.6f) {
            // in the range from 0.6 to 0.75 intensity should change from 1 to 0
            // 0.75 - 0.6 = 0.15
            // 1 / 0.15 ~ 6.67
            // 1 - (t - 0.6) * 6.67 = I
            intensityMultiplier = Mathf.Clamp01(1 - (_currentTimeOfDay - 0.6f) * 6.67f);
        }
 
        _sun.intensity = _sunStartIntensity * intensityMultiplier;
    }
    

    private void UpdateSkyBox()
    {
        RenderSettings.ambientSkyColor = _skyBoxColor.Evaluate(_currentTimeOfDay);
        UpdateNightIntensity();
    }

    private void UpdateNightIntensity()
    {
        if (_currentTimeOfDay > 0.4f && _currentTimeOfDay < 0.6f) {
            return;
        }

        var intensityMultiplier = 1f;
        
        if (_currentTimeOfDay >= 0.6) {
            // in the range from 0.6 to 1 intensity should change from 1 to 0.2
            // 1 - 0.6 = 0.4
            // 1 - 0.2 = 0.8
            // 0.8 / 0.4 = 2
            // 1 - (t * 2 - 1) + 0.2 = I   =>  2.2 - t * 2 = I
            intensityMultiplier = Mathf.Clamp01(2.2f - _currentTimeOfDay * 2f);
        } else if (_currentTimeOfDay <= 0.4) {
            // in the range from 0 to 0.4 intensity should change from 0.2 to 1
            // 0.4 - 0 = 0.4
            // 1 - 0.2 = 0.8
            // 0.8 / 0.4 = 2
            // t * 2 + 0.2 = I
            intensityMultiplier = Mathf.Clamp01(_currentTimeOfDay * 2f + 0.2f);
        }

        RenderSettings.ambientIntensity = intensityMultiplier;
        RenderSettings.reflectionIntensity = intensityMultiplier;
    }
}
