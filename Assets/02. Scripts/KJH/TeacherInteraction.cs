using DG.Tweening;
using Photon.Chat.UtilityScripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherInteraction : MonoBehaviourPun
{
    public GameObject spawnButton;
    private GameObject player;
    public List<GameObject> spawnPrefab;
    //public List<Sprite> spawnSprite;

    private Button Btn_Spawn;

    private CharacterInteraction characterInteraction;

    private void Awake()
    {
        if (DataBase.instance.userInfo.isTeacher == false && photonView.IsMine)
            this.enabled = false;
        else
            spawnButton.gameObject.SetActive(true);

        characterInteraction = GetComponent<CharacterInteraction>();
    }

    private void Start()
    {
        player = characterInteraction.Character;
        Btn_Spawn = spawnButton?.GetComponentInChildren<Button>();
        Btn_Spawn?.onClick.AddListener(() => OnSpawnBtnClick());
    }

    public void OnSpawnBtnClick()
    {
        PhotonNetwork.Instantiate("/3D_Models" +spawnPrefab[0].name, new Vector3(player.transform.position.x, player.transform.position.y,player.transform.position.z) , Quaternion.identity);
    }
}
