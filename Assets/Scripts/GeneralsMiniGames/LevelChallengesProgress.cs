using TMPro;
using UnityEngine;

namespace GeneralsMiniGames
{
   public class LevelChallengesProgress : MonoBehaviour
   {
      [SerializeField]
      private TMP_Text textQuantityChallengesInLevel;

      [SerializeField]
      private TMP_Text textActualChallegesResolved;

      public static LevelChallengesProgress Instance { get; private set; }

      private void Awake()
      {
         Instance = this;
      }

      public void SetupQuantityChallengesInActualLevel(int argQuantityChallengesInLevel)
      {
         textQuantityChallengesInLevel.text = argQuantityChallengesInLevel.ToString();
      }

      public void SetupQuantityChallengesFinishInActualLevel(int argQuantityChallengesFinishInLevel)
      {
         textActualChallegesResolved.text = argQuantityChallengesFinishInLevel.ToString();
      }
   }
}