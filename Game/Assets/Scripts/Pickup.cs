using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Pickup : MonoBehaviour
{
    [SerializeField] private int m_reward;
    SoundManager smInstance;
    AudioSource audioSource;
    AudioSource ownAudioSource;

    private void Start()
    {
        smInstance = SoundManager.Instance;
        audioSource = smInstance.GetComponents<AudioSource>()[1];
        ownAudioSource = GetComponent<AudioSource>();
    }
    //thomas's idee
    public int Reward { get { PlayPickupSound(); return m_reward; } }

    public void PlayPickupSound()
    {
        
        if (m_reward > 25) { audioSource.volume = ownAudioSource.volume; audioSource.PlayOneShot(smInstance.gem); } else { audioSource.volume = ownAudioSource.volume; audioSource.PlayOneShot(smInstance.cherry); }
    }
}
