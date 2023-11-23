using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPointerSoundHandler : MonoBehaviour,IPointerEnterHandler
{
    public AudioClip ButtonClickSound;
    private AudioSource sfxSource;

    private void Start()
    {
        // 자식 오브젝트 생성
        GameObject sfxChild = new GameObject("ButtonSFX");
        sfxChild.transform.SetParent(this.transform);

        // AudioSource 컴포넌트 추가
        sfxSource = sfxChild.AddComponent<AudioSource>();
        sfxSource.clip = ButtonClickSound;
        sfxSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sfxSource.Play();
    }
}
