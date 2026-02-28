using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop;
        
        [HideInInspector]
        public AudioSource source;
    }
    
    [Header("Sound Categories")]
    public Sound[] sfx;
    public Sound[] music;
    public Sound[] ambience;
    
    [Header("Spatial Audio")]
    public AudioMixerGroup sfxMixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup ambienceMixer;
    
    private Dictionary<string, Sound> sfxDict = new Dictionary<string, Sound>();
    private Dictionary<string, Sound> musicDict = new Dictionary<string, Sound>();
    private Sound currentMusic;
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeSounds(sfx, sfxDict, "SFX");
        InitializeSounds(music, musicDict, "Music");
        InitializeSounds(ambience, ambienceDict, "Ambience");
    }
    
    void InitializeSounds(Sound[] sounds, Dictionary<string, Sound> dict, string mixerName)
    {
        foreach (var s in sounds)
        {
            GameObject child = new GameObject($"Audio_{s.name}");
            child.transform.SetParent(transform);
            
            s.source = child.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            
            // Set mixer group
            switch (mixerName)
            {
                case "SFX": s.source.outputAudioMixerGroup = sfxMixer; break;
                case "Music": s.source.outputAudioMixerGroup = musicMixer; break;
                case "Ambience": s.source.outputAudioMixerGroup = ambienceMixer; break;
            }
            
            dict[s.name] = s;
        }
    }
    
    public void PlaySFX(string name)
    {
        if (sfxDict.ContainsKey(name))
        {
            sfxDict[name].source.Play();
        }
    }
    
    public void PlayMusic(string name)
    {
        if (currentMusic != null)
        {
            currentMusic.source.Stop();
        }
        
        if (musicDict.ContainsKey(name))
        {
            currentMusic = musicDict[name];
            currentMusic.source.Play();
        }
    }
    
    public void StopMusic()
    {
        if (currentMusic != null)
        {
            currentMusic.source.Stop();
            currentMusic = null;
        }
    }
    
    public void SetVolume(string mixer, float volume)
    {
        float db = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
        
        switch (mixer.ToLower())
        {
            case "sfx": sfxMixer?.audioMixer.SetFloat("SFXVolume", db); break;
            case "music": musicMixer?.audioMixer.SetFloat("MusicVolume", db); break;
            case "master": sfxMixer?.audioMixer.SetFloat("MasterVolume", db); break;
        }
    }
}
