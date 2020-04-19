using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameSettingsController 
{
    public static GameSettingsController Instance => _instance ?? (_instance = new GameSettingsController());
    private static GameSettingsController _instance;

    public bool IsPaused {
        get => Time.timeScale == 0;
        set {
            var timeScale = value == true ? 0 : 1;
            Time.timeScale = timeScale;
        }
    }
}
