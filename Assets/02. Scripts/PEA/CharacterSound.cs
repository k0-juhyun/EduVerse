using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSound : MonoBehaviour
{
    private AudioSource audioSource;

    public int curSceneBuildIndex;

    public AudioClip[] groundSceneWalkSounds;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    private void CharacterWalkSound(int walkSoundIndex) // 0 ¶Ç´Â 1
    {
        switch (curSceneBuildIndex)
        {
            // ±¤Àå 
            case 5:
                audioSource.PlayOneShot(groundSceneWalkSounds[walkSoundIndex]);
                break;
            default:
                break;
        }
    }
}
