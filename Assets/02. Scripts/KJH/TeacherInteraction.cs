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

    private bool isObjectBeingPlaced = false;
    private GameObject objectToPlace;
    private GameObject currentlyDragging;

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

    private void Update()
    {
        if (isObjectBeingPlaced)
        {
            // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ�մϴ�.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            // Z ��ǥ�� ĳ���� ������ �����մϴ�.
            mousePosition.z = player.transform.position.z + 1.0f;

            // ������Ʈ�� ���콺 Ŀ�� ��ġ�� �̵���ŵ�ϴ�.
            objectToPlace.transform.position = mousePosition;

            // ���콺 ��ư�� �ٽ� ������ ������Ʈ�� ��ġ�մϴ�.
            if (Input.GetMouseButtonDown(0))
            {
                isObjectBeingPlaced = false;
                currentlyDragging = null;
            }
        }
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

        // ��ũ�Ѻ��� Content ���� ��ư���� �����մϴ�.
        for (int i = 0; i < modelCount; i++)
        {
            int index = i; // Ŭ���� ���� ������ ���� ���� ����
            GameObject newButton = Instantiate(buttonPrefab, buttonsParent);
            newButton.GetComponentInChildren<Text>().text = DataBase.instance.model.spawnPrefab[index].name; // ���� �������� �̸����� ��ư �ؽ�Ʈ �Ҵ�
            newButton.GetComponent<Button>().onClick.AddListener(() => SpawnModel(index)); // ������ �߰�
        }
    }

    public void SpawnModel(int modelIndex)
    {
        if (!isObjectBeingPlaced && modelIndex >= 0 && modelIndex < DataBase.instance.model.spawnPrefab.Count)
        {
            GameObject modelToSpawn = DataBase.instance.model.spawnPrefab[modelIndex];
            objectToPlace = PhotonNetwork.Instantiate("3D_Models/" + modelToSpawn.name, player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

            // InteractableModel ������Ʈ�� �߰��մϴ�.
            InteractableModel interactableModel = objectToPlace.AddComponent<InteractableModel>();
            // �ʿ��ϴٸ� InteractableModel�� �Ӽ��� ������ �� �ֽ��ϴ�.
            // ��: interactableModel.someProperty = someValue;

            isObjectBeingPlaced = true; // ������Ʈ ������ ���·� ����
            currentlyDragging = objectToPlace; // ���� �巡�� ������Ʈ�� ����
        }
    }

}
