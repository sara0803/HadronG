using System;
using System.Collections;
using AvancedUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GeneralsMiniGames
{
   public class PanelCronometroEvents : PanelUI<PanelCronometroEvents>
   {
      [SerializeField]
      private Image imageCronometro;

      [SerializeField]
      private Sprite[] arrayCronometroImages;

      public float timeToCompleteChallenge;

      private float actualTimeToCompleteChallenge;

      private bool paused;

      [SerializeField]
      private string formatTimer = "mm:ss";

      [Header("Sound")]
      [SerializeField]
      private bool playSound;

      [SerializeField]
      private AudioSource audioSource;

      [SerializeField]
      private AudioClip audioClipBefore5Seconds;

      [SerializeField]
      private AudioClip audioClipAfter5Seconds;

      [Header("Events")]
      [SerializeField]
      private UnityEvent OnCronometroFinish;

      public float ActualTimeToCompleteChallenge
         => actualTimeToCompleteChallenge;

      public string TimeString
      {
         get
         {
            var tmpTime = TimeSpan.FromSeconds(actualTimeToCompleteChallenge);
            DateTime tmpTimeConverted = DateTime.MinValue.Add(tmpTime);
            return tmpTimeConverted.ToString(formatTimer);
         }
      }

      public void StartCronometro()
      {
         OpenPanel();
         StartCoroutine(CouStartCronometro());
      }

      public void StartCronometro(float argTimeToComplete)
      {
         timeToCompleteChallenge = argTimeToComplete;
         OpenPanel();
         StartCoroutine(CouStartCronometro());
      }

      public void Pause()
      {
         paused = true;
         audioSource.Stop();
      }

      public void UnPause()
      {
         paused = false;
      }

      public void StopCronometro()
      {
         audioSource.Stop();
         StopAllCoroutines();
         ClosePanel();
      }

      public void AddTime(float argTimeInSecondsToRemove)
      {
         actualTimeToCompleteChallenge += argTimeInSecondsToRemove;
         actualTimeToCompleteChallenge = Mathf.Clamp(actualTimeToCompleteChallenge, 0, float.MaxValue);
         UpdateSprite();
      }

      private IEnumerator CouStartCronometro()
      {
         actualTimeToCompleteChallenge = 0;

         while(actualTimeToCompleteChallenge < timeToCompleteChallenge)
         {
            actualTimeToCompleteChallenge += Time.deltaTime;
            UpdateSprite();

            while(paused)
               yield return null;

            if(playSound)
            {
               if(actualTimeToCompleteChallenge >= timeToCompleteChallenge - 5)
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

            yield return null;
         }

         audioSource.Stop();
         OnCronometroFinish.Invoke();
      }

      private void UpdateSprite()
      {
         var tmpTimeNormalized = actualTimeToCompleteChallenge / timeToCompleteChallenge;
         var tmpActualIndex = Mathf.FloorToInt(tmpTimeNormalized * (arrayCronometroImages.Length - 1));
         imageCronometro.sprite = arrayCronometroImages[tmpActualIndex];
      }
   }
}