using ScriptableEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GeneralsMiniGames
{
   public class LoadMiniGameNotify : MonoBehaviour
   {
      [SerializeField]
      private ScriptableEventEmpty seSceneMiniGameLoadEnds;

      public static LoadMiniGameNotify Instance { get; set; }

      private void Awake()
      {
         if(Instance)
         {
            Destroy(gameObject);
         }
         else
         {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
         }
      }

      private void OnSceneLoaded(Scene argScene, LoadSceneMode argLoadSceneMode)
      {
         if(argScene.name.Contains("MiniGame"))
            seSceneMiniGameLoadEnds.ExecuteEvent();
      }
   }
}