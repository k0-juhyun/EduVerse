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

    // 다른 효과음도 쉽게 추가하기 위해 enum으로 함
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
        // 이벤트 등록 해제
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

        //// "GroundScene" 씬으로 전환되었을 때
        //if (scene.name == "5.GroundScene")
        //{
        //    PlayBGM(1); // 1번 BGM 재생
        //}
        //else
        //{
        //    PlayBGM(0); // 그 외의 경우에는 0번 BGM 재생
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
        // 모든 BGM 정지
        foreach (BGM bgmItem in bgm)
        {
            bgmItem.BGMSource.Stop();
        }

        // 지정된 인덱스의 BGM 재생
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
            print("음악재생");
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
