using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip m_slideAudioClip;
    [SerializeField] private AudioClip m_addCubeAudioClip;
    [SerializeField] private AudioSource m_playerAudioSource;

    public void Initialize(AudioSource playerAudioSource)
    {
        m_playerAudioSource = playerAudioSource;
    }
    
    public void PlaySlideSound()
    {
        m_playerAudioSource.clip = m_slideAudioClip;
        m_playerAudioSource.Play();
    }

    public void PlayAddCube()
    {
        m_playerAudioSource.clip = m_addCubeAudioClip;
        m_playerAudioSource.Play();
    }
}
