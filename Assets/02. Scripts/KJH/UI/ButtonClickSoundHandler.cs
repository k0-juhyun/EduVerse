using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // UI 관련 네임스페이스 추가

[RequireComponent(typeof(AudioSource))]
public class ButtonClickSoundHandler : MonoBehaviour
{
    public AudioClip ButtonClickSound;
    private AudioSource sfxSource;

    private void Start()
    {
        // 자식 오브젝트 생성
        GameObject sfxChild = new GameObject("ButtonSFX");
        sfxChild.transform.SetParent(this.transform);

        // AudioSource 컴포넌트 추가
        //sfxSource = sfxChild.AddComponent<AudioSource>();
        sfxSource = GetComponent<AudioSource>();
        sfxSource.clip = ButtonClickSound;
        sfxSource.playOnAwake = false;

        // Button 컴포넌트에 오디오 재생 메서드 연결
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    private void PlaySound()
    {
        print("audio play");
        sfxSource.PlayOneShot(ButtonClickSound);
    }
}
