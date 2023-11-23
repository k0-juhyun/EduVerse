using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // UI ���� ���ӽ����̽� �߰�

[RequireComponent(typeof(AudioSource))]
public class ButtonClickSoundHandler : MonoBehaviour
{
    public AudioClip ButtonClickSound;
    private AudioSource sfxSource;

    private void Start()
    {
        // �ڽ� ������Ʈ ����
        GameObject sfxChild = new GameObject("ButtonSFX");
        sfxChild.transform.SetParent(this.transform);

        // AudioSource ������Ʈ �߰�
        //sfxSource = sfxChild.AddComponent<AudioSource>();
        sfxSource = GetComponent<AudioSource>();
        sfxSource.clip = ButtonClickSound;
        sfxSource.playOnAwake = false;

        // Button ������Ʈ�� ����� ��� �޼��� ����
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
