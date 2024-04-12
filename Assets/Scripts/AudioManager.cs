using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip m_hitToPlatform;
    [SerializeField]private AudioSource m_soundEffectAudioSource;
    [SerializeField]private AudioSource m_backgroundMusicAudioSource;

    public void Initialize()
    {
        AudioListener.volume = 1;
        m_hitToPlatform.LoadAudioData();
    }
    
    public void HitToPlatformSound(int levelNum)
    {
        m_soundEffectAudioSource.clip = m_hitToPlatform;
        m_soundEffectAudioSource.Play();
    }
}
