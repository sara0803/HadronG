using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Economy
{
   public class EconomyManager : MonoBehaviour
   {
      public int QuantityCoins
      {
         get => PlayerPrefs.GetInt("QuantityCoins") + TemporalCoins;
         set
         {
            TemporalCoins += value - QuantityCoins;
            UpdateCoinsCount();
         }
      }

      public int TemporalCoins { get; private set; }

      [SerializeField]
      private GiveCoinUI prefabGiveCoinUI;

      [SerializeField]
      private GameObject prefabConfeti;

      private Canvas refCanvas;

      private TMP_Text textCountCoins;

      public static EconomyManager Intance { get; private set; }

      private Canvas RefCanvas
      {
         get
         {
            if(refCanvas == null)
               refCanvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();

            return refCanvas;
         }
      }

      private TMP_Text TextQuantityCoins
      {
         get
         {
            if(textCountCoins == null)
               textCountCoins = GameObject.FindWithTag("TextCountCoins").GetComponent<TMP_Text>();

            return textCountCoins;
         }
      }

      private void Awake()
      {
         if(!Intance)
         {
            Intance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
         }
         else
            DestroyImmediate(gameObject);
      }

      private void OnSceneLoaded(Scene argScene, LoadSceneMode argLoadSceneMode)
      {
         if(argScene.name.Contains("MiniGame"))
            UpdateCoinsCount();
      }

      public void GiveCoinsQuantity(int argQuantity)
      {
         Instantiate(prefabConfeti, Vector3.zero, Quaternion.identity);
         StartCoroutine(CouGiveCoinsQuantity(argQuantity));
      }

      private IEnumerator CouGiveCoinsQuantity(int argQuantity)
      {
         var tmpQuantityCoinsCreated = 0;

         while(tmpQuantityCoinsCreated < argQuantity)
         {
            tmpQuantityCoinsCreated++;
            Instantiate(prefabGiveCoinUI, RefCanvas.transform);

            var tmpTime = 1.5f;

            while(tmpTime > 0)
            {
               tmpTime -= Time.deltaTime;
               yield return null;
            }
         }

         yield return null;
      }

      [ContextMenu("TestCoins")]
      public void TestCoins()
      {
         GiveCoinsQuantity(5);
      }

      private void UpdateCoinsCount()
      {
         TextQuantityCoins.text = QuantityCoins.ToString();
      }

      public void SaveActualQuantityCoins()
      {
         PlayerPrefs.SetInt("QuantityCoins", QuantityCoins);
         TemporalCoins = 0;
         UpdateCoinsCount();
      }

      public void ResetActualQuantityCoins()
      {
         TemporalCoins = 0;
         UpdateCoinsCount();
      }
   }
}