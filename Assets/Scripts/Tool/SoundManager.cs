using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonBehavior<SoundManager>
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayEffectSound(Sounds soundName)
    {
        var soundString = GetSoundString(soundName);
        StartCoroutine(AssetLoaderManager.Inst.LoadSoundAsync<AudioClip>(soundString, a =>
        {
            audioSource.clip = a;
            audioSource.Play();
        }));
    }

    #region Link Sounds with Enums
    public enum Sounds
    {
        KnifeDash,
        TearPaper,
        CutPaper,
    }

    private string GetSoundString(Sounds soundEnum) =>
        soundEnum switch
        {
            Sounds.CutPaper => "종이찢기2",
            Sounds.TearPaper => "종이찢기1",
            Sounds.KnifeDash => "검베기1",
            _ => throw new System.Exception(),
        };
    #endregion
}
