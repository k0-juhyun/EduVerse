using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            print("À½¾ÇÀç»ý");
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
