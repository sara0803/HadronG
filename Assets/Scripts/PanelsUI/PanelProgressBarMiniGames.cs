using AvancedUI;
using UnityEngine;
using UnityEngine.Events;

namespace PanelsUI
{
   public class PanelProgressBarMiniGames : PanelUI<PanelProgressBarMiniGames>
   {
      [SerializeField]
      private GameObject[] arrayProgressParts;

      private int progressToComplete;

      [SerializeField]
      private UnityEvent OnComplete;

      private byte actualProgressComplete;

      public float ProgressNormalized
         => (float)actualProgressComplete / progressToComplete;

      public void SetupProgressToComplete(int argProgressToComplete)
      {
         progressToComplete = argProgressToComplete;
      }

      [ContextMenu("AddCompleteProgress")]
      public void AddCompleteProgress()
      {
         actualProgressComplete++;

         var tmpActualMaxIndex = Mathf.Min(Mathf.RoundToInt(ProgressNormalized * arrayProgressParts.Length), arrayProgressParts.Length);

         for(int i = 0; i < tmpActualMaxIndex; i++)
            arrayProgressParts[i].SetActive(true);

         if(actualProgressComplete == progressToComplete)
            OnComplete.Invoke();
      }

      public void ResetProgressBar()
      {
         actualProgressComplete = 0;

         foreach(var tmpProgressBarPart in arrayProgressParts)
            tmpProgressBarPart.SetActive(false);
      }
   }
}