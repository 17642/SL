using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    Walk, Pickup, Attack, Unlock
}
public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip walkSound;
    [SerializeField]
    AudioClip itemPickupSound;
    [SerializeField]
    AudioClip attackSound;
    [SerializeField]
    AudioClip unlockSound;

    [SerializeField]
    Player player;

    [SerializeField]
    float crawlVolume;

    float originVolume;

    AudioSource[] audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponents<AudioSource>();
        audioSource[0].loop = false;
        audioSource[1].loop = true;

        originVolume = audioSource[1].volume;

        audioSource[1].clip = walkSound;
    }

    // Update is called once per frame
    void Update()
    {
        playWalking();
    }

    public void PlaySound(Sound soundType)
    {
        if (audioSource[0].isPlaying)
        {
            audioSource[0].Stop();
        }
        switch (soundType)
        {
            case Sound.Walk:
                audioSource[0].clip = walkSound;
                break;
            case Sound.Attack:
                audioSource[0].clip = attackSound;
                break;
            case Sound.Unlock:
                audioSource[0].clip = unlockSound;
                break;
            case Sound.Pickup:
                audioSource[0].clip = itemPickupSound;
                break;
            default:
                Debug.Log("肋给等 可记涝聪促.");
                break;
        }
        audioSource[0].Play();
    }

    void playWalking()
    {
        if (player.isCrawling)
        {
            audioSource[1].volume = originVolume* crawlVolume;
        }
        else
        {
            audioSource[1].volume = originVolume;
        }
        if (audioSource[1].isPlaying && !player.isMoving)
        {
            audioSource[1].Stop();
        }else if(!audioSource[1].isPlaying&&player.isMoving) 
        {
            audioSource[1].Play();
        }
    }

    public void StopAllSounds()
    {
        audioSource[0].Stop();
        audioSource[1].Stop();

        audioSource[0].clip = null;
        audioSource[1].clip = null;
    }
}
