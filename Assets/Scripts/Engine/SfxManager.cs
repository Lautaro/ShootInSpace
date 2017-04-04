using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SfxManager : MonoBehaviour
{

    [SerializeField]
    bool doDebug;

    [SerializeField]
    string sourceFolderPath;

    public bool Mute;


    static SfxManager instance;
    private static List<AudioSource> players = new List<AudioSource>();
    private static AudioSource musicPlayer;
    private static Dictionary<string, AudioClip> sfxs;
    private static Dictionary<string, AudioClip> dynamicSfx;

    #region setup
    void Start()
    {

        sfxs = new Dictionary<string, AudioClip>();

        if (!instance)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Only one SfxManager should be present in the same scene");
        }

        LoadSfxs();
    }

    private static AudioSource GetAudioPlayer()
    {
        AudioSource player = (from p in players
                              where p.isPlaying == false
                              select p).FirstOrDefault();

        if (!player)
        {
            player = SfxManager.instance.gameObject.AddComponent<AudioSource>();
            players.Add(player);
        }

        return player;
    }

    private void LoadSfxs()
    {

        var loadedSfxs = Resources.LoadAll(sourceFolderPath, typeof(AudioClip));

        foreach (AudioClip sfx in loadedSfxs)
        {
            sfxs.Add(sfx.name, sfx);
        }

        int loadedSfx = sfxs.Count;

        if (doDebug)
        {
            Debug.Log(String.Format("Loaded {0} audio files. ", loadedSfx));
        }
    }
    #endregion

    /// <summary>
    /// Use for sounds that you want to swtich of later
    /// </summary>
    /// <param name="sfx">nameo of sfx</param>
    /// <param name="go">object soundisconnected to</param>
    /// <param name="loop"></param>
    /// <returns></returns>
    public static void PlayDynamicSfxOnGo(string sfx, GameObject go, bool loop = true)
    {
        if (!instance.Mute)
        {
            // Check if gameObject already has a ready player dedicated to this sfx
            var audioPlayer = (from a in go.GetComponents<AudioSource>()
                               where a.clip.name == sfx
                               select a).FirstOrDefault();


            if (!audioPlayer)
            {
                audioPlayer = go.AddComponent<AudioSource>();
            }

            audioPlayer.loop = loop;

            audioPlayer.clip = sfxs[sfx];
            if (!audioPlayer.isPlaying)
            {
                audioPlayer.Play();
            }

        }
    }

    public static void StopDynamicSfxOnGO(string sfx, GameObject go)
    {
        foreach (var audioSource in go.GetComponents<AudioSource>())
        {
            if (audioSource.clip.name == sfx)
            {
                audioSource.Stop();
            }
        };
    }

    public static void PlaySfx(string sfx, float delay = 0f, float volume = 1f)
    {
        if (!instance.Mute)
        {
            var player = GetAudioPlayer();
            player.volume = volume;
            player.clip = sfxs[sfx];
            player.PlayDelayed(delay);
        }
    }

    public static void PlaySfx(string sfx, GameObject location, float delay = 0f, float volume = 1f)
    {
        if (!instance.Mute)
        {
            var locationPlayer = location.GetComponent<AudioSource>();

            if (locationPlayer == null)
            {//If GameObject does not have a AudioSource we will give it one.
                locationPlayer = location.AddComponent<AudioSource>();
                locationPlayer.playOnAwake = false;
            }
            locationPlayer.volume = volume;
            locationPlayer.clip = sfxs[sfx];
            locationPlayer.PlayDelayed(delay);
        }
    }
}
