using System.Collections;
using AvancedUI;
using ScriptableEvents;
using UnityEngine;

namespace PanelsUI
{
   public class PanelCountDown : PanelUI<PanelCountDown>
   {
      [SerializeField]
      private GameObject animationCounterDown;

      [SerializeField]
      private ScriptableEventEmpty seMiniGameStarted;

      [ContextMenu("CounterStart")]
      public void TestCounterStart()
      {
         CounterStart(true);
      }

      public void CounterStart(bool argShowCounterStart)
      {
         OpenPanel();
         StartCoroutine(CouAnimateText(argShowCounterStart));
      }

      private IEnumerator CouAnimateText(bool argShowCounterStart)
      {
         if(argShowCounterStart)
         {
            animationCounterDown.SetActive(true);

            var tmpCounter = 1.8f;

            while(tmpCounter > 0)
            {
               tmpCounter -= Time.deltaTime;
               yield return null;
            }

            animationCounterDown.SetActive(false);
            yield return new WaitForSeconds(0.15f);
         }

         yield return null;
         seMiniGameStarted.ExecuteEvent();
         ClosePanel();
      }
   }
}