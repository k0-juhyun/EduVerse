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
        // �̺�Ʈ ��� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // "GroundScene" ������ ��ȯ�Ǿ��� ��
        if (scene.name == "5.GroundScene")
        {
            PlayBGM(1); // 1�� BGM ���
        }
        else
        {
            PlayBGM(0); // �� ���� ��쿡�� 0�� BGM ���
        }
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
}
