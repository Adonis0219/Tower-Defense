using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("# BGM")]
    public AudioClip[] bgmClip;
    public float[] bgmVolume;
    AudioSource bgmPlayer;

    [Header("# SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx
    {
        GameStart, Click, OkClk, NoClk, GetClk, UnlockClk,
        PanelBtClk, UpBtClk, Complete, Cursor, CardEqu,

        Fire, Hit, Die, PlayerHit, PlayerDie
    }

    protected override void Awake()
    {
        Init();

        base.Awake();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObj = new GameObject("BgmPlayer");
        bgmObj.transform.parent = transform;
        bgmPlayer = bgmObj.AddComponent<AudioSource>();
        bgmPlayer.loop = true;

        // 효과음 플레이어 초기화
        GameObject sfxObj = new GameObject("SfxPlayer");
        sfxObj.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObj.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlayBgm(SceneType type)
    {
        bgmPlayer.clip = bgmClip[(int)type];
        bgmPlayer.volume = bgmVolume[(int)type];
        bgmPlayer.Play();
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            // 가장 마지막에 플레이한 인덱스
            // channelIndex가 15인 경우 i가 2만 돼도 17로 넘어감
            // 17일 때 1로 검사하기 위해 sfxPlayers.Length의 나머지로 활용
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue; // 반복문 도중 다음 루프로 건너뛰는 키워드

            channelIndex = loopIndex;

            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
