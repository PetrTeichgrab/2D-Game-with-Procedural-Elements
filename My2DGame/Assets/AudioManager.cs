using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXsource;

    [SerializeField] 
    AudioClip[] backgroundMusicClips = new AudioClip[8];

    [SerializeField]
    public AudioClip playerSpell;
    public AudioClip playerPickUpColorCore;
    public AudioClip EnemyAttackRange;
    public AudioClip EnemyAttackMelee1;
    public AudioClip EnemyAttackMelee2;
    public AudioClip Revive;
    public AudioClip Death;
    public AudioClip placeColorCore;

    private List<int> shuffledIndices = new List<int>();
    private int currentTrackIndex = 0;

    void Start()
    {
        ShuffleMusicOrder();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXsource.PlayOneShot(clip);
    }

    void ShuffleMusicOrder()
    {
        shuffledIndices.Clear();
        for (int i = 0; i < backgroundMusicClips.Length; i++)
        {
            shuffledIndices.Add(i);
        }

        // Fisher-Yates shuffle
        for (int i = shuffledIndices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = shuffledIndices[i];
            shuffledIndices[i] = shuffledIndices[j];
            shuffledIndices[j] = temp;
        }

        currentTrackIndex = 0;
    }

    public void PlayNextTrack()
    {
        if (currentTrackIndex >= shuffledIndices.Count)
        {
            ShuffleMusicOrder();
        }

        int clipIndex = shuffledIndices[currentTrackIndex];
        AudioClip nextClip = backgroundMusicClips[clipIndex];

        musicSource.clip = nextClip;
        musicSource.Play();
        currentTrackIndex++;

        Invoke(nameof(PlayNextTrack), nextClip.length);
    }

}
