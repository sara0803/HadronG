using System.Collections;
using ScriptableEvents;
using TMPro;
using UnityEngine;

namespace GeneralsMiniGames
{
   public class Timer : MonoBehaviour
   {
      [SerializeField]
      private ScriptableEventEmpty seMiniGameStarted;

      [SerializeField]
      private TMP_Text textTimer;

      private float actualTime;

      private bool timerRunning;

      public float ActualTime
         => actualTime;

      private void OnEnable()
      {
         seMiniGameStarted.Subscribe(OnMiniGameStarted);
      }

      private void OnDisable()
      {
         seMiniGameStarted.Unsubscribe(OnMiniGameStarted);
      }

      private void OnMiniGameStarted()
      {
         if(timerRunning)
            return;

         StartCoroutine(CouExecuteTimer());
      }

      private IEnumerator CouExecuteTimer()
      {
         timerRunning = true;
         actualTime = 0;

         while(true)
         {
            actualTime += Time.deltaTime;
            var tmpMinutes = Mathf.FloorToInt(actualTime / 60);
            var tmpSeconds = Mathf.FloorToInt(actualTime % 60);

            if(textTimer)
               textTimer.text = $"{tmpMinutes:00}:{tmpSeconds:00}";

            yield return null;
         }
      }
   }
}