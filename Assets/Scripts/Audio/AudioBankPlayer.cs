using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBankPlayer : MonoBehaviour
{
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
        }
    }
}
