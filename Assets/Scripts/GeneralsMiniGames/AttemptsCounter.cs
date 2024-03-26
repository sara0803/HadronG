using Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GeneralsMiniGames
{
   public class AttemptsCounter : Singleton<AttemptsCounter>
   {
      [SerializeField]
      private AudioSource asAttemptWrong;

      [SerializeField]
      private int quantityAttempts = 5;

      public UnityEvent OnReachAllAttemps;

      private int actualAttempts;

      private TMP_Text[] arrayTextCountAttempts;

      private TMP_Text[] ArrayTextCountAttempts
      {
         get
         {
            if(arrayTextCountAttempts == null)
            {
               var tmpAllTextCountAttempts = GameObject.FindGameObjectsWithTag("TextCountAttempts");
               arrayTextCountAttempts = new TMP_Text[tmpAllTextCountAttempts.Length];

               for(int i = 0; i < tmpAllTextCountAttempts.Length; i++)
                  arrayTextCountAttempts[i] = tmpAllTextCountAttempts[i].GetComponent<TMP_Text>();
            }

            return arrayTextCountAttempts;
         }
      }

      public bool AddAttempt()
      {
         actualAttempts++;
         asAttemptWrong.Play();

         if(actualAttempts > quantityAttempts)
         {
            OnReachAllAttemps.Invoke();
            actualAttempts = 0;
            UpdateUIValues();
            return false;
         }

         UpdateUIValues();
         return true;
      }

      public void ResetAttemps()
      {
         actualAttempts = 0;
         UpdateUIValues();
      }

      private void UpdateUIValues()
      {
         foreach(var tmpTextCountAttempt in ArrayTextCountAttempts)
            tmpTextCountAttempt.text = actualAttempts.ToString();
      }
   }
}