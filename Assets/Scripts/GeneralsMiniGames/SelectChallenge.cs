using Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GeneralsMiniGames
{
   public class SelectChallenge : Singleton<SelectChallenge>
   {
      [SerializeField]
      private ChallengeSetup[] arrayChallengeSetup;

      private ChallengeSetup refChallengeSetupSelected;

      private int currentLevelChallenge = -1;

      [SerializeField]
      private UnityEvent OnAllChallengesOfLevelFinish;

      [SerializeField]
      private UnityEvent OnAllLevelsFinish;

      public ChallengeSetup RefChallengeSetupSelected
         => refChallengeSetupSelected;

      public int CurrentLevelChallenge
         => currentLevelChallenge;

      private TMP_Text textLevelRank;

      private TMP_Text TextLevelRank
      {
         get
         {
            if(textLevelRank == null)
               textLevelRank = GameObject.FindWithTag("TextLevelRank").GetComponent<TMP_Text>();

            return textLevelRank;
         }
      }

      [ContextMenu("SelectChallengeSetupBasic")]
      public void SetupNextLevelChallenge()
      {
         if(refChallengeSetupSelected == null)
         {
            currentLevelChallenge++;
            AttemptsCounter.Instance.ResetAttemps();
            SetupTextLevelRank();
            refChallengeSetupSelected = arrayChallengeSetup[currentLevelChallenge];
         }

         if(refChallengeSetupSelected)
         {
            refChallengeSetupSelected.PassToNextChallengeLevelAvaible();

            if(refChallengeSetupSelected.GetIfCurrentChallengeLevelAvailable())
               refChallengeSetupSelected.SetupAndStartChallengeInScene();
            else
            {
               if(refChallengeSetupSelected == arrayChallengeSetup[^1])
                  OnAllLevelsFinish.Invoke();
               else
                  OnAllChallengesOfLevelFinish.Invoke();

               refChallengeSetupSelected = null;
            }
         }
      }

      public void PlayAgainActualChallegeSetupSelected()
      {
         if(refChallengeSetupSelected)
            refChallengeSetupSelected.SetupAndStartChallengeInScene();
      }

      private void SetupTextLevelRank()
      {
         switch(currentLevelChallenge)
         {
            case 0:
               TextLevelRank.text = "B";
               break;

            case 1:
               TextLevelRank.text = "I";
               break;

            case 2:
               TextLevelRank.text = "A";
               break;

            case 3:
               TextLevelRank.text = "M";
               break;
         }
      }
   }
}