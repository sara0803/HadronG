using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GeneralsMiniGames
{
   public class RelojArenaPequeno : MonoBehaviour
   {
      [SerializeField]
      private SpriteRenderer spriteRenderer;

      [SerializeField]
      private Sprite[] arrayFrames;

      public void StartReloj(float argTimeToFinish, UnityAction argTimeFinish)
      {
         gameObject.SetActive(true);
         StartCoroutine(CouTimer(argTimeToFinish, argTimeFinish));
      }

      private IEnumerator CouTimer(float argTimeToFinish, UnityAction argOnTimeFinish)
      {
         var tmpActualTime = 0f;

         while(tmpActualTime < argTimeToFinish)
         {
            tmpActualTime += Time.deltaTime;
            spriteRenderer.sprite = arrayFrames[Mathf.RoundToInt(arrayFrames.Length * (tmpActualTime / argTimeToFinish))];
            yield return null;
         }

         argOnTimeFinish();
         gameObject.SetActive(false);
      }
   }
}