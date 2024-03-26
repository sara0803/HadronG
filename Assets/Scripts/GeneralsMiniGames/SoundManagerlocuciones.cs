using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerlocuciones : MonoBehaviour
{
   [SerializeField]
   private AudioMixer refAudioMixerLocutions;

   [SerializeField]
   private AudioSource refAudioSourceLocutions;

   private string audioMixerVolumeTag = "AudioMixerLocucionesMasterVolume";

   public static SoundManagerlocuciones Instance { get; private set; }

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

   public void PlayAudioLocution(AudioClip argAudioClip)
   {
      refAudioSourceLocutions.PlayOneShot(argAudioClip);
   }

   public void StopAudioLocution()
   {
      refAudioSourceLocutions.Stop();
   }

   public void MuteAudioLocution()
   {
      refAudioMixerLocutions.SetFloat(audioMixerVolumeTag, -80);
   }

   public void UnMuteAudioLocution()
   {
      refAudioMixerLocutions.SetFloat(audioMixerVolumeTag, 0);
   }
}
