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

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public BGM[] bgm;
    [Space(10)]
    public SFX[] sfx;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            InitializeSoundComponents();
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
        // "GroundScene" 씬으로 전환되었을 때
        if (scene.name == "5.GroundScene")
        {
            PlayBGM(1); // 1번 BGM 재생
        }
        else
        {
            PlayBGM(0); // 그 외의 경우에는 0번 BGM 재생
        }
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
}
