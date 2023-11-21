using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(AudioSource))]
public class CharacterSound : MonoBehaviour
{
    private AudioSource audioSource;

    private int curSceneBuildIndex;

    public AudioClip[] groundSceneWalkSounds;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void OnChangeScene(int sceneBuildIndex)
    {
        curSceneBuildIndex = sceneBuildIndex;
    }

    public void CharacterWalkSound(int walkSoundIndex) // 0 ¶Ç´Â 1
    {
        print("CharacterWalkSound : " + curSceneBuildIndex + ", " + walkSoundIndex);
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
