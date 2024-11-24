using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip mainMenuAmb;

    private IEnumerator currMusicCoroutine;

    

    void Start()
    {
        // Persist audio objects on loading new scene
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(musicPlayer.gameObject);

        // Start with main menu music
        PlayMainMenuMusic();
    }

    void Update()
    {
        
    }
    
    //////////////////////
    // PUBLIC FUNCTIONS //
    //////////////////////
    
    public void PlayMainMenuMusic()
    {
        if (currMusicCoroutine != null)
            StopCoroutine(currMusicCoroutine);
        currMusicCoroutine = I_PlayMainMenuMusic();
        StartCoroutine(currMusicCoroutine);
    }

    /////////////////////////
    // MUSIC FUNCTIONALITY //
    /////////////////////////

    private IEnumerator I_PlayMainMenuMusic()
    {
        // play intro music
        musicPlayer.volume = 0.5f;
        musicPlayer.spatialBlend = 0.0f;
        musicPlayer.clip = mainMenuMusic;
        musicPlayer.loop = false;
        musicPlayer.Play();

        // after intro music is done, loop ambience
        yield return new WaitForSeconds(41f);
        musicPlayer.Stop();

        musicPlayer.volume = 1.0f;
        musicPlayer.spatialBlend = 0.0f;
        musicPlayer.loop = true;
        currMusicCoroutine = I_PlayAndFadeIn(mainMenuAmb, 3.0f, 0.0f, 0.7f);
        StartCoroutine(currMusicCoroutine);
    }

    //////////////////////
    // HELPER FUNCTIONS //
    //////////////////////

    private IEnumerator I_FadeOutThenPlay(AudioClip music, float fadeSecs, float startVol, float endVol)
    {
        int ticks = 100;

        // fade out
        musicPlayer.volume = startVol;
        for (int i = 0; i < ticks; i++) {
            yield return new WaitForSeconds(fadeSecs / ticks);
            musicPlayer.volume -= ((startVol - endVol) / ticks);
        }

        // play new music
        musicPlayer.clip = music;
        musicPlayer.Play();
    }

    private IEnumerator I_PlayAndFadeIn(AudioClip music, float fadeSecs, float startVol, float endVol)
    {
        int ticks = 100;

        // play new music
        musicPlayer.clip = music;
        musicPlayer.Play();

        // fade in
        musicPlayer.volume = startVol;
        for (int i = 0; i < ticks; i++) {
            yield return new WaitForSeconds(fadeSecs / ticks);
            musicPlayer.volume += ((endVol - startVol) / ticks);
        }
    }
}
