using System.Collections;  
using System.Collections.Generic;
using UnityEngine;
using AvancedUI;
using Economy;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PanelFinishedAttempts : PanelUI<PanelFinishedAttempts>
{
   [SerializeField]
   private Button refSoundButton;

   [SerializeField]
   private AudioClip locutionFinishedAttempsAudio;

   private void OnEnable()
   {
      EconomyManager.Intance.ResetActualQuantityCoins();
      StartCoroutine(CouClosePanel());

      if (locutionFinishedAttempsAudio)
         SoundManagerlocuciones.Instance.PlayAudioLocution(locutionFinishedAttempsAudio);

      refSoundButton.onClick.AddListener(() =>
      {
         SoundManagerlocuciones.Instance.MuteAudioLocution();
      });
   }

   private void OnDisable()
   {
      refSoundButton.onClick.RemoveListener(() =>
      {
         SoundManagerlocuciones.Instance.MuteAudioLocution();
      });
   }

   IEnumerator CouClosePanel()
   {
      yield return new WaitForSeconds(8.1f);
      ClosePanel();
   }
}
