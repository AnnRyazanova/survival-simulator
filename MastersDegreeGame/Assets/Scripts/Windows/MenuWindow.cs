using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWindow : BaseWindow
{
   public override void Show()
   {
      base.Show();
      Init();
   }

   public void OnPlayButtonClick()
   {
      MenuController.Instance.HideWindow();
      MainWindowController.Instance.ShowWindow();
      SceneLoaderController.Instance.LoadStartScene();
      Debug.Log($"[MenuWindow::OnPlayButtonClick] You`ve clicked on PlayButton");
   }

   private void Init()
   {
      //initialize window here
   }
   
}
