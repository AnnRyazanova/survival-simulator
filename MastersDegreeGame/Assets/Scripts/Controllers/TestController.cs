using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TestController 
{
    public static TestController Instance => _instance ?? (_instance = new TestController());
    private static TestController _instance;

    public void ShowWindow()
    {
        if (TestWindow.Instance != null) {
            return;
        }
        
        var window = BaseWindow.LoadWindow("TestWindow");
        if (window != null) {
            TestWindow.Show(window);
        }
    }
}
