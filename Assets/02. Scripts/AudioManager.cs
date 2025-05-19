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
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObj = new GameObject("BgmPlayer");
        bgmObj.transform.parent = transform;
        bgmPlayer = bgmObj.AddComponent<AudioSource>();
        bgmPlayer.loop = true;

        // ȿ���� �÷��̾� �ʱ�ȭ
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
            // ���� �������� �÷����� �ε���
            // channelIndex�� 15�� ��� i�� 2�� �ŵ� 17�� �Ѿ
            // 17�� �� 1�� �˻��ϱ� ���� sfxPlayers.Length�� �������� Ȱ��
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue; // �ݺ��� ���� ���� ������ �ǳʶٴ� Ű����

            channelIndex = loopIndex;

            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
