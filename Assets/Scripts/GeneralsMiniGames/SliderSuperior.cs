using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliderSuperior : MonoBehaviour
{
   [SerializeField]
   private TMP_Text TextCountCoins;
   [SerializeField]
   private TMP_Text TextCountMedals;
   [SerializeField]
   private TMP_Text TextCountShoppingCart;
   [SerializeField]
   private TMP_Text TextCountMission;

   private void OnEnable()
   {
      TextCountCoins.text = $"{PlayerDataManager.Instance.PlayerCoinsAmount}";
      TextCountMedals.text = $"{PlayerDataManager.Instance.PlayerMedalsAmount}";
      TextCountMission.text = $"{PlayerDataManager.Instance.PlayerCurrentMission}";
      TextCountShoppingCart.text = $"";
   }
}
