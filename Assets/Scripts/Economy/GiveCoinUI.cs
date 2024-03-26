using UnityEngine;

namespace Economy
{
   public class GiveCoinUI : MonoBehaviour
   {
      public void DestroyCoin()
      {
         Destroy(gameObject);
      }

      private void OnDestroy()
      {
         EconomyManager.Intance.QuantityCoins++;
      }
   }
}