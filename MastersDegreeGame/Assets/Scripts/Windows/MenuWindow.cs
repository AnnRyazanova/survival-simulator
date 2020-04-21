using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : BaseWindow
{
   public Dropdown sceneDropdown;
   public void OnPlayButtonClick()
   {
      MenuController.Instance.HideWindow();
      SceneDropdownHandle();
   }

   public void SceneDropdownHandle()
   {
      //Debug.Log("Number of scene:" + sceneDropdown.value);
      SceneLoaderController.Instance.LoadStartScene(sceneDropdown.value);
   }

   public void OnQuitApplication()
   {
      Application.Quit();
   }
}
