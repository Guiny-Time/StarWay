using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MusicMgr : BaseManager<MusicMgr>
{
    //唯一的背景音乐组件
    private static AudioSource bkMusic = null;
    //音乐大小
    private float bkValue = 1;
    //音效列表
    private List<AudioSource> soundList = new List<AudioSource>();
    private float musicVolume = 0.8f;
    private float effectVolome = 0.8f;

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBkMusic()
    {
        bkMusic.Play();
    }

    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    public void SetBgmVol(float v)
    {
        musicVolume = v;
        PlayerPrefs.SetFloat("bgm",v);
    }

    public void SetEffectVol(float v)
    {
        effectVolome = v;
        PlayerPrefs.SetFloat("audio",v);
    }

    public float GetBgmVol()
    {
        return musicVolume;
    }

    public float GetEffectVol()
    {
        return effectVolome;
    }
}