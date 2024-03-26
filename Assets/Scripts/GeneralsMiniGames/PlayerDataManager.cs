using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
   public static PlayerDataManager Instance
   { private set; get; }

   private int playerCoinsAmount;
   private int playerMedalsAmount;
   private int playerCurrentMission;
   private string playerCurrentLevel;
   private int playerAttempts;

   public int PlayerCoinsAmount 
      => playerCoinsAmount;
   public int PlayerMedalsAmount 
      => playerMedalsAmount;
   public int PlayerCurrentMission 
      => playerCurrentMission;

   public int PlayerAttempts 
      => playerAttempts;

   public string PlayerCurrentLevel 
      => playerCurrentLevel;

   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         DestroyImmediate(this);
      }
      else
      {
         Instance = this;
         DontDestroyOnLoad(this);
      }
   }

   public void SetCurrentMission(int argCurrentMission)
   {
      playerCurrentMission = argCurrentMission;
   }

   public void SetCurrentLevel(string argCurrentLevel)
   {
      playerCurrentLevel = argCurrentLevel;
   }

   public void AddCoinsToPlayer(int argAmountCoinsToAdd)
   {
      playerCoinsAmount += argAmountCoinsToAdd;
   }

   public void AddMedalsToPlayer(int argAmountMedalsToAdd)
   {
      playerMedalsAmount += argAmountMedalsToAdd;
   }

   public void AddAttempt()
   {
      playerAttempts++;
   }

   public void ResetAttempts()
   {
      playerAttempts = 0;
   }
}
