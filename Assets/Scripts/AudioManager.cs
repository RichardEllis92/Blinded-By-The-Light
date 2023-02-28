using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource levelMusic, gameOverMusic, winMusic, choiceMusic, secretEndingMusic;

    public AudioSource[] sfx;

    private void Awake()
    {
        instance = this;
    }

    public void PlayGameOver()
    {
        levelMusic.Stop();
        gameOverMusic.Play();
    }

    public void PlayLevelWin()
    {
        levelMusic.Stop();
        winMusic.Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }

    public void StopSFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
    }

    public void PlayChoiceMusic()
    {
        levelMusic.Stop();
        choiceMusic.Play();
    }

    public void PlaySecretEndingMusic()
    {
        choiceMusic.Stop();
        secretEndingMusic.Play();
    }
}
