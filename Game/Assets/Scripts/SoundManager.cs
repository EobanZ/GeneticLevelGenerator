using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : GenericSingletonClass<SoundManager>
{
    [Header("General")]
    public AudioClip music;
    public AudioClip buttonClick;

    [Header("Player")]
    public AudioClip playerJump;
    public AudioClip playerDeath;

    [Header("Pickups")]
    public AudioClip cherry;
    public AudioClip gem;

    [Header("Enemies")]
    public AudioClip frogQuack;
    public AudioClip frogJump;
    public AudioClip frogDeath;
    [Range(2, 6)]
    public float quackIntervall;

    [Header("AudioSources")]
    public AudioSource audioSourceMusic;



    override public void Awake()
    {
        base.Awake();
        audioSourceMusic.clip = music;
    }

    private void Start()
    {
        audioSourceMusic.Play();
    }
}
