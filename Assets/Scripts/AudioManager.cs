using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script gestiona la reproducción de sonidos y música en el juego.
public enum LevelMusic
{
    Overworld,
    UnderGround,
    Castle,
    IntoTunnel
}

public class AudioManager : MonoBehaviour
{
    // Creación de una instancia para el AudioManager.
    public static AudioManager instance;

// Creamos dos fuentes de audio, una para efectos de sonido (SFX) y otra para música.
    public AudioSource sfx;
    public AudioSource music;

    public AudioClip clipJump;
    public AudioClip clipBigJump;
    public AudioClip clipCoin;
    public AudioClip clipStomp;
    public AudioClip clipShoot;
    public AudioClip clipPowerUp;
    public AudioClip clipFlipDie;
    public AudioClip clipBreak;
    public AudioClip clipBump;
    public AudioClip clipDie;
    public AudioClip clipPowerDown;
    public AudioClip clipPowerUpAppear;
    public AudioClip clip1UP;
    public AudioClip clipFlagPoleDown;
    public AudioClip clipBowserFall;


// Música de los niveles
    public AudioClip clipOverworld;
    public AudioClip clipUnderworld;
    public AudioClip clipBowserCastle;


    public AudioClip clipStarman;
    public AudioClip clipLevelCompleted;
    public AudioClip clipCastleCompleted;
    public AudioClip clipGameover;
    public AudioClip clipIntoTunnel;


    LevelMusic current;
    bool starmanActivated;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //SFX = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayJump()
    {
        sfx.PlayOneShot(clipJump);
    }

    public void PlayBigJump()
    {
        sfx.PlayOneShot(clipBigJump);
    }

    public void PlayCoin()
    {
        sfx.PlayOneShot(clipCoin);
    }

    public void PlayStomp()
    {
        sfx.PlayOneShot(clipStomp);
    }

    public void PlayFlipDie()
    {
        sfx.PlayOneShot(clipFlipDie);
    }

    public void PlayShoot()
    {
        sfx.PlayOneShot(clipShoot);
    }

    public void PlayPowerUp()
    {
        sfx.PlayOneShot(clipPowerUp);
    }

    public void PlayPowerDown()
    {
        sfx.PlayOneShot(clipPowerDown);
    }
    //Al parecer el audio de meterse en una tuberia es el mismo que recibir daño
    public void PlayPipe()
    {
        sfx.PlayOneShot(clipPowerDown);
    }

    public void PlayPowerAppear()
    {
        sfx.PlayOneShot(clipPowerUpAppear);
    }

    public void PlayBreak()
    {
        sfx.PlayOneShot(clipBreak);
    }

    public void Play1UP()
    {
        sfx.PlayOneShot(clip1UP);
    }

    public void PlayBump()
    {
        sfx.PlayOneShot(clipBump);
    }

    public void PlayBowserFall()
    {
        sfx.PlayOneShot(clipBowserFall);
    }


    public void PlayDie()
    {
        music.pitch = 1f;
        music.clip = clipDie;
        music.loop = false;
        music.Play();
    }

    public void PlayFlagPole()
    {
        music.pitch = 1f;
        music.clip = clipFlagPoleDown;
        music.loop = false;
        music.Play();
    }



    public void PlayLevelStageMusic(LevelMusic levelMusic)
    {
        switch (levelMusic)
        {
            case LevelMusic.Overworld:
                MusicOverworld();
                break;
            case LevelMusic.UnderGround:
                MusicUnderWorld();
                break;
            case LevelMusic.Castle:
                MusicCastle();
                break;
            case LevelMusic.IntoTunnel:
                PlayIntoTunnel();
                break;
        }
    }


    void MusicOverworld()
    {
        current = LevelMusic.Overworld;
        if (!starmanActivated)
        {
            music.clip = clipOverworld;
            music.loop = true;
            music.Play();
        }
    }

    void MusicUnderWorld()
    {
        current = LevelMusic.UnderGround;
        if (!starmanActivated)
        {
            music.clip = clipUnderworld;
            music.loop = true;
            music.Play();
        }
    }

    void MusicCastle()
    {
        current = LevelMusic.Castle;
        if (!starmanActivated)
        {
            music.clip = clipBowserCastle;
            music.loop = true;
            music.Play();
        }
    }


    public void MusicStar()
    {
        starmanActivated = true;
        music.pitch = 1f;
        music.clip = clipStarman;
        music.loop = true;
        music.Play();
    }

    public void StopMusicStar(bool playLevelMusic)
    {
        if (starmanActivated)
        {
            starmanActivated = false;
            if (playLevelMusic)
            {
                PlayLevelStageMusic(current);
            }
        }
    }

    public void PlayLevelCompleted()
    {
        music.pitch = 1f;
        music.clip = clipLevelCompleted;
        music.loop = false;
        music.Play();
    }

    public void PlayCastleCompleted()
    {
        music.pitch = 1f;
        music.clip = clipCastleCompleted;
        music.loop = false;
        music.Play();
    }

    public void PlayGameover()
    {
        music.pitch = 1f;
        music.clip = clipGameover;
        music.loop = false;
        music.Play();
    }

    void PlayIntoTunnel()
    {
        music.clip = clipIntoTunnel;
        music.loop = false;
        music.Play();
    }

//Hacemos que suene más rapido.
    public void SpeedMusic()
    {
        if (!starmanActivated) // Solo acelerar si no está el modo estrella activo
        {
            music.pitch = 1.5f;
        }
    }
}
