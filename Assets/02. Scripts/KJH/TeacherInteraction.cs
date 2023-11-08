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
    public GameObject buttonPrefab;
    public GameObject scrollView;

    private Button Btn_Spawn;

    public bool isSpawnBtnClick;

    public Transform buttonsParent;

    private CharacterInteraction characterInteraction;

    private void Awake()
    {
        if (DataBase.instance.userInfo.isTeacher == false && photonView.IsMine)
            this.enabled = false;
        else
            spawnButton.gameObject.SetActive(true);

        characterInteraction = GetComponent<CharacterInteraction>();
        scrollView.SetActive(false);

    }

    private void Start()
    {
        player = characterInteraction.Character;
        CreateButtonsForModels();
        Btn_Spawn = spawnButton?.GetComponentInChildren<Button>();
        Btn_Spawn?.onClick.AddListener(() => OnSpawnBtnClick());
    }

    public void OnSpawnBtnClick()
    {
        scrollView.SetActive(!scrollView.activeSelf);
        isSpawnBtnClick = !isSpawnBtnClick;
    }

    private void CreateButtonsForModels()
    {
        int modelCount = DataBase.instance.model.spawnPrefab.Count;

        // 스크롤뷰의 Content 내에 버튼들을 생성합니다.
        for (int i = 0; i < modelCount; i++)
        {
            int index = i; // 클로저 문제 방지를 위한 로컬 변수
            GameObject newButton = Instantiate(buttonPrefab, buttonsParent);
            newButton.GetComponentInChildren<Text>().text = DataBase.instance.model.spawnPrefab[index].name; // 원래 프리팹의 이름으로 버튼 텍스트 할당
            newButton.GetComponent<Button>().onClick.AddListener(() => SpawnModel(index)); // 리스너 추가
        }
    }

    private void SpawnModel(int modelIndex)
    {
        if (modelIndex >= 0 && modelIndex < DataBase.instance.model.spawnPrefab.Count)
        {
            GameObject modelToSpawn = DataBase.instance.model.spawnPrefab[modelIndex];
            GameObject newObject = PhotonNetwork.Instantiate("3D_Models/" + modelToSpawn.name, player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
    }

}
