using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonBehavior<SoundManager>
{
    private readonly List<AudioSource> audioSources = new List<AudioSource>();
    private readonly HashSet<int> usingIndexs = new HashSet<int>();

    private void Awake()
    {
        audioSources.Add(gameObject.AddComponent<AudioSource>());
    }

    /// <summary>
    /// 원하는 효과음을 재생
    /// </summary>
    /// <param name="soundName">재생할 사운드의 이름 (SoundManager.Sound Enum 참고)</param>
    /// <param name="volume">0~1 사이의 float value</param>
    public void PlayEffectSound(Sounds soundName, float volume = -1)
    {
        var soundSettings = GetSoundSettings(soundName);
        if (volume > 0)
        {
            soundSettings.volume = volume;
        }

        int emptyAudioIndex = -1;
        for (int i = 0; i < audioSources.Count; ++i)
        {
            if (!usingIndexs.Contains(i) && !audioSources[i].isPlaying)
            {
                emptyAudioIndex = i;
                usingIndexs.Add(emptyAudioIndex);
                break;
            }
        }
        // 만일 모든 AudioSource가 사용중일때
        if (emptyAudioIndex < 0)
        {
            audioSources.Add(gameObject.AddComponent<AudioSource>());
            emptyAudioIndex = audioSources.Count - 1;
        }

        var audioSourceToUse = audioSources[emptyAudioIndex];

        StartCoroutine(AssetLoaderManager.Inst.LoadSoundAsync<AudioClip>(soundSettings.name, a =>
        {
            audioSourceToUse.clip = a;
            audioSourceToUse.volume = soundSettings.volume;
            audioSourceToUse.Play();
            usingIndexs.Remove(emptyAudioIndex);
        }));
    }

    #region Link Sounds with Enums
    public enum Sounds
    {
        KnifeDash,
        TearPaper,
        CutPaper,
    }

    private class SoundSettings
    {
        public string name;
        private float _volume;
        public float volume
        {
            get { return _volume; }
            set { _volume = Mathf.Clamp01(value); }
        }

        public SoundSettings(string name, float volume)
        {
            this.name = name;
            this.volume = volume;
        }
        public SoundSettings(string name)
        {
            this.name = name;
            this.volume = 1;
        }
    }

    private SoundSettings GetSoundSettings(Sounds soundEnum) =>
        soundEnum switch
        {
            Sounds.CutPaper => new SoundSettings("종이찢기2"),
            Sounds.TearPaper => new SoundSettings("종이찢기1"),
            Sounds.KnifeDash => new SoundSettings("검베기1", 0.5f),
            _ => new SoundSettings("검베기(기모으기)"),
        };
    #endregion
}
