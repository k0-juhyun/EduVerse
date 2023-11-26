using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BGM
{
    public string BGMName;
    public AudioClip BGMClip;
    [HideInInspector]
    public AudioSource BGMSource;
}

[System.Serializable]
public class SFX
{
    public string SFXName;
    public AudioClip SFXClip;
    [HideInInspector]
    public AudioSource SFXSource;
}

[RequireComponent(typeof(AudioSource))]

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public BGM[] bgm;
    [Space(10)]
    public SFX[] sfx;

    public enum BGMClip
    {
        Login,
        Customize,
        Class,
        Ground
    }

    // �ٸ� ȿ������ ���� �߰��ϱ� ���� enum���� ��
    public enum SFXClip
    {
        Button1,
        Button2
    }

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            InitializeSoundComponents();
            audioSource = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ��� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "1.StartScene":
                PlayBGM(BGMClip.Login);
                break;
            case "2.CustomizeScene":
                PlayBGM(BGMClip.Customize);
                break;
            case "4.ClassRoomScene":
                PlayBGM(BGMClip.Class);
                break;
            case "5.GroundScene":
                PlayBGM(BGMClip.Ground);
                break;
            default:
                break;
        }

        //// "GroundScene" ������ ��ȯ�Ǿ��� ��
        //if (scene.name == "5.GroundScene")
        //{
        //    PlayBGM(1); // 1�� BGM ���
        //}
        //else
        //{
        //    PlayBGM(0); // �� ���� ��쿡�� 0�� BGM ���
        //}
    }

    public void BGMPlayOrStop(bool play)
    {
        if (play)
            audioSource.Play();
        else
            audioSource.Stop();
    }

    private void PlayBGM(BGMClip bgmClip)
    {
        audioSource.clip = bgmClips?[(int)bgmClip];
        audioSource?.Play();
        audioSource.loop = true;
    }

    private void PlayBGM(int index)
    {
        // ��� BGM ����
        foreach (BGM bgmItem in bgm)
        {
            bgmItem.BGMSource.Stop();
        }

        // ������ �ε����� BGM ���
        if (index >= 0 && index < bgm.Length)
        {
            bgm[index].BGMSource.Play();
        }
    }

    private void InitializeSoundComponents()
    {
        foreach (BGM bgmItem in bgm)
        {
            GameObject bgmChild = new GameObject("BGM_" + bgmItem.BGMName);
            bgmChild.transform.SetParent(this.transform);
            bgmItem.BGMSource = bgmChild.AddComponent<AudioSource>();
            bgmItem.BGMSource.clip = bgmItem.BGMClip;
            bgmItem.BGMSource.loop = true;
            bgmItem.BGMSource.PlayOneShot(bgmItem.BGMSource.clip);
            print("�������");
        }

        foreach (SFX sfxItem in sfx)
        {
            GameObject sfxChild = new GameObject("SFX_" + sfxItem.SFXName);
            sfxChild.transform.SetParent(this.transform);
            sfxItem.SFXSource = sfxChild.AddComponent<AudioSource>();
            sfxItem.SFXSource.clip = sfxItem.SFXClip;
        }
    }

    public void PlaySFX(SFXClip sfxClip)
    {
        print("play sfx : " + (int)sfxClip);
        audioSource.PlayOneShot(sfxClips[(int)sfxClip]);
    }
}
