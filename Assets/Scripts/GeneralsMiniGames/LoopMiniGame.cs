using System.Collections;
using ScriptableEvents;
using UnityEngine;

namespace GeneralsMiniGames
{
   public abstract class LoopMiniGame : MonoBehaviour
   {
      [SerializeField]
      protected ScriptableEventEmpty seMiniGameStarted;

      [SerializeField]
      protected ScriptableEventEmpty seMiniGameFinished;

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
         StartCoroutine(CouLoopMiniGame());
      }

      public void MiniGameFinished()
      {
         seMiniGameFinished.ExecuteEvent();
         StopAllCoroutines();
      }

      protected abstract IEnumerator CouLoopMiniGame();
   }
}