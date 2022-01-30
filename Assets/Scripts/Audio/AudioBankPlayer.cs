using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBankPlayer : MonoBehaviour
{
    public static float m_volume = 1.0f;

    [SerializeField]
    private AudioSource m_audioSource = null;

    [SerializeField]
    private AudioClip[] m_audioClips = null;

    [SerializeField]
    private bool m_playOnStart = false;

    private void Start()
    {
        if (m_playOnStart)
        {
            PlayRand();
        }
    }

    public void PlayRand()
    {
        if (m_audioSource != null && m_audioClips.Length > 0)
        {
            int idx = Random.Range(0, m_audioClips.Length);

            m_audioSource.clip = m_audioClips[idx];
            m_audioSource.Play();

            m_audioSource.volume = m_volume;
        }
    }

    public void SetVolume(float volume)
    {
        m_volume = volume;
    }
}
