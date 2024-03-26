using System.Collections;
using PanelsUI;
using UnityEngine;

namespace GeneralsMiniGames
{
   public abstract class ChallengeSetup : MonoBehaviour
   {
      [SerializeField]
      protected ChallengeLevel challengeLevel;

      protected int currentChallengeLevel = -1;

      [SerializeField]
      private int quantityChallengesInLevel = 4;

      private bool showCounterStart = true;

      public virtual void SetupAndStartChallengeInScene()
      {
         AttemptsCounter.Instance.OnReachAllAttemps.RemoveAllListeners();
         AttemptsCounter.Instance.OnReachAllAttemps.AddListener(ResetChallenge);
         PanelCountDown.Instance.CounterStart(showCounterStart);

         if(showCounterStart)
            StartCoroutine(CouStartMiniGame());
         else
            LevelChallengesProgress.Instance.SetupQuantityChallengesInActualLevel(quantityChallengesInLevel);

         showCounterStart = false; //only show one time at init of minigame
      }

      public abstract void ResetChallenge();

      public abstract bool GetIfCurrentChallengeLevelAvailable();

      public abstract void PassToNextChallengeLevelAvaible();

      private IEnumerator CouStartMiniGame()
      {
         yield return new WaitForSeconds(2.5f);
         LevelChallengesProgress.Instance.SetupQuantityChallengesInActualLevel(quantityChallengesInLevel);
      }
   }

   public enum ChallengeLevel
   {
      Basic,
      Intermedium,
      Avanced,
      Master
   }
}