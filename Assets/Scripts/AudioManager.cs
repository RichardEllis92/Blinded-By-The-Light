using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource levelMusic, gameOverMusic, winMusic, bossFightMusic;

    public AudioSource[] sfx;

    private void Awake()
    {
        Instance = this;
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

    public void PlaySfx(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }

    public void StopSfx(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
    }

    public void PlayBossFightMusic()
    {
        levelMusic.Stop();
        bossFightMusic.Play();
    }
    
}
