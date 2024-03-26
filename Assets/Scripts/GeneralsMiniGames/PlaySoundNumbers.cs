using Singleton;
using UnityEngine;

namespace GeneralsMiniGames
{
   public class PlaySoundNumbers : Singleton<PlaySoundNumbers>
   {
      [SerializeField]
      private AudioSource asPlaySounds;

      [SerializeField]
      private AudioClip[] arrayAudioClipNumbers;

      public void PlaySoundNumber(int argNumber)
      {
         asPlaySounds.PlayOneShot(arrayAudioClipNumbers[argNumber]);
      }
   }
}