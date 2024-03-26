using AvancedUI;
using TMPro;
using UnityEngine;

namespace PanelsUI
{
   public class PanelFinishGame : PanelUI<PanelFinishGame>
   {
      [SerializeField]
      private TMP_Text textButton;

      [SerializeField]
      private string textInButton;

      private void OnEnable()
      {
         textButton.text = textInButton;
      }
   }
}