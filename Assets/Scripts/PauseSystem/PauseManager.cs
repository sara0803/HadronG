using GeneralsMiniGames;
using PanelsUI;
using UnityEngine;

namespace PauseSystem
{
   public class PauseManager : MonoBehaviour
   {
      public void SetMiniGamePaused()
      {
         Time.timeScale = 0;
         PanelPause.Instance.gameObject.SetActive(true);
      }

      public void SetMiniGameUnPaused()
      {
         Time.timeScale = 1;
         PanelPause.Instance.gameObject.SetActive(false);
      }
   }
}