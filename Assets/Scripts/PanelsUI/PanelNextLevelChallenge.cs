using AvancedUI;
using Economy;
using GeneralsMiniGames;
using TMPro;
using UnityEngine;

namespace PanelsUI
{
   public class PanelNextLevelChallenge : PanelUI<PanelNextLevelChallenge>
   {
      [SerializeField]
      private TMP_Text textButtonLevel;

      private void OnEnable()
      {
         switch(SelectChallenge.Instance.CurrentLevelChallenge)
         {
            case 0:
               textButtonLevel.text = "Ir al nivel intermedio";
               break;

            case 1:
               textButtonLevel.text = "Ir al nivel avanzado";
               break;

            case 2:
               textButtonLevel.text = "Ir al nivel máster";
               break;
         }
      }

      public void ButtonSetupNextLevelChallenge()
      {
         EconomyManager.Intance.SaveActualQuantityCoins();
         SelectChallenge.Instance.SetupNextLevelChallenge();
         ClosePanel();
      }
   }
}