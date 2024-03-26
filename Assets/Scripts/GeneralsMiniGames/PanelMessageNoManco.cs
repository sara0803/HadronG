using AvancedUI;
using TMPro;
using UnityEngine;

namespace GeneralsMiniGames
{
   public class PanelMessageNoManco : PanelUI<PanelMessageNoManco>
   {
      [SerializeField]
      private TMP_Text textMessageNoManco;

      public void Setup(string argMessage)
      {
         textMessageNoManco.text = argMessage;
         OpenPanel();
      }

      [ContextMenu("SetupTest")]
      public void SetupTest()
      {
         Setup("TestMessagae sadfljasdjf;a");
      }
   }
}