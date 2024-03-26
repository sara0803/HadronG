using System.Collections;
using AvancedUI;
using UnityEngine;

namespace GeneralsMiniGames
{
   public class PanelCronometro : PanelUI<PanelCronometro>
   {
      [SerializeField]
      private Animator refAnimatorCronometro;

      public float timeToCompleteChallenge;

      private bool paused;

      private static readonly int Reset = Animator.StringToHash("Reset");

      [Header("Sound")]
      [SerializeField]
      private bool playSound;

      [SerializeField]
      private AudioSource audioSource;

      [SerializeField]
      private AudioClip audioClipBefore5Seconds;

      [SerializeField]
      private AudioClip audioClipAfter5Seconds;

      private void OnEnable()
      {
         ResetAnimator();
         StartCoroutine(CouStartCronometro());
      }

      public void Pause()
      {
         paused = true;
         refAnimatorCronometro.speed = 0f;
         audioSource.Stop(); 
      }

      public void UnPause()
      {
         paused = false;
         refAnimatorCronometro.speed = 0.75f / timeToCompleteChallenge;
      }

      public void StopCronometro()
      {
         StopAllCoroutines();
         ClosePanel();
      }

      IEnumerator CouStartCronometro()
      {
         refAnimatorCronometro.speed = 0.75f / timeToCompleteChallenge;
         var tmpActualTime = 0f;

         while(tmpActualTime < timeToCompleteChallenge)
         {
            if(paused)
            {
               yield return null;
               continue;
            }

            if(playSound)
            {
               if(tmpActualTime >= timeToCompleteChallenge - 5)
               {
                  if(audioSource.clip != audioClipAfter5Seconds)
                  {
                     audioSource.clip = audioClipAfter5Seconds;
                     audioSource.Play();
                  }
                  else if(!audioSource.isPlaying)
                  {
                     audioSource.clip = audioClipAfter5Seconds;
                     audioSource.Play();
                  }
               }
               else
               {
                  if(!audioSource.isPlaying)
                  {
                     audioSource.clip = audioClipBefore5Seconds;
                     audioSource.Play();
                  }
               }
            }

            tmpActualTime += Time.deltaTime;
            yield return null;
         }

         AttemptsCounter.Instance.AddAttempt();
         yield return new WaitForSeconds(0.5f);
         ClosePanel();
      }

      private void ResetAnimator()
      {
         refAnimatorCronometro.SetTrigger(Reset);
      }
   }
}