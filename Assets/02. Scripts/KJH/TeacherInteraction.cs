using DG.Tweening;
using Dummiesman;
using Photon.Chat.UtilityScripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System.Net.Security;
using TMPro;

public class TeacherInteraction : MonoBehaviourPun
{
    [Header("��ư")]
    public GameObject spawnButton;
    public GameObject disableDeskButton;
    private GameObject player;
    public GameObject quizButton;

    [Header("3D�𵨸� ��ư")]
    public GameObject buttonPrefab;
    [Header("3D�𵨸� ��ũ�Ѻ�")]
    public GameObject scrollView;

    private GameObject objectToPlace;
    private GameObject currentlyDragging;


    private Button Btn_Spawn;
    private Button Btn_Disable;

    [HideInInspector] public bool isDisableBtnClick;
    private bool isObjectBeingPlaced = false;
    [HideInInspector] public bool isSpawnBtnClick;

    [Header("3D�𵨸� ��ư ���� �θ�")]
    public Transform buttonsParent;

    private CharacterInteraction characterInteraction;
    private ClassRoomHandler classRoomHandler;

    private void Awake()
    {
        if (DataBase.instance.user.isTeacher == false && photonView.IsMine)
            this.enabled = false;

        else
        {
            if (SceneManager.GetActiveScene().name == "4.ClassRoomScene")
            {
                spawnButton.gameObject.SetActive(true);
                disableDeskButton.gameObject.SetActive(true);
            }
            if (SceneManager.GetActiveScene().name == "5.GroundScene" && DataBase.instance.user.isTeacher)
            {
                quizButton.gameObject.SetActive(true);
            }
        }

        characterInteraction = GetComponentInParent<CharacterInteraction>();

        if (SceneManager.GetActiveScene().name == "4.ClassRoomScene")
            classRoomHandler = GameObject.Find("ClassRoom").GetComponent<ClassRoomHandler>();

        scrollView.SetActive(false);

    }
    private void Start()
    {
        player = characterInteraction.Character;
        CreateButtonsForModels();

        Btn_Spawn = spawnButton?.GetComponent<Button>();
        Btn_Disable = disableDeskButton?.GetComponent<Button>();

        Btn_Spawn?.onClick.AddListener(() => OnSpawnBtnClick());
        Btn_Disable?.onClick.AddListener(() => OnClickDisableDesk());
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


    public void OnSpawnBtnClick()
    {
        scrollView.SetActive(true);
        isSpawnBtnClick = true;
    }

    private void CreateButtonsForModels()
    {
        int modelCount = DataBase.instance.model.spawnPrefab.Count;

        // ��ũ�Ѻ��� Content ���� ��ư���� �����մϴ�.
        for (int i = 0; i < modelCount; i++)
        {
            int index = i; // Ŭ���� ���� ������ ���� ���� ����
            GameObject newButton = Instantiate(buttonPrefab, buttonsParent);
            newButton.GetComponentInChildren<TMP_Text>().text = DataBase.instance.model.spawnPrefab[index].name; // ���� �������� �̸����� ��ư �ؽ�Ʈ �Ҵ�
            newButton.GetComponent<Button>().onClick.AddListener(() => SpawnModel(index)); // ������ �߰�

            // rawImage ������Ʈ
            RawImage thumbNailImage = newButton.transform.GetChild(1).GetComponent<RawImage>();
            print(thumbNailImage.gameObject);

            // ���ҽ� �ε� (��� ����)
            string texturePath = "Thumbnail/" + DataBase.instance.model.spawnPrefab[index].name;
            Texture2D thumbNailTexture = Resources.Load<Texture2D>(texturePath);

            // �ؽ�ó �Ҵ�
            if (thumbNailTexture != null)
            {
                thumbNailImage.texture = thumbNailTexture;
                print(thumbNailImage.texture.name + " : " + thumbNailTexture);
            }
        }
    }

    public void TestBtnClick()
    {
        SpawnModel(0);
    }

    public void SpawnModel(int modelIndex)
    {
        if (!isObjectBeingPlaced && modelIndex >= 0 && modelIndex < DataBase.instance.model.spawnPrefab.Count)
        {
            GameObject modelToSpawn = DataBase.instance.model.spawnPrefab[modelIndex];

            // ����並 ���� �������Ʈ�� �ҷ��ͼ�
            objectToPlace = PhotonNetwork.Instantiate("3D_Models/mesh", player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

            //scrollView.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InBack)
            //.OnComplete(() => scrollView.SetActive(false));

            isSpawnBtnClick = false;

            if (objectToPlace != null)
            {
                isObjectBeingPlaced = true;
                currentlyDragging = objectToPlace;

                // rpc �� ������Ʈ ����
                photonView.RPC("LoadAndApplyOBJ", RpcTarget.AllBuffered, modelToSpawn.name, objectToPlace.GetPhotonView().ViewID);
            }

            else
            {
                Debug.LogError("Failed to instantiate object on network.");
            }
        }
    }

    [PunRPC]
    public void LoadAndApplyOBJ(string modelName, int viewID)
    {
        PhotonView targetPhotonView = PhotonView.Find(viewID);
        if (targetPhotonView != null)
        {
            print("loadA");
            objectToPlace = targetPhotonView.gameObject;

#if UNITY_ANDROID
            string objFilePath = Application.persistentDataPath + "/3D_Models/ModelDatas/"+ modelName + ".obj";
            WWW www = new WWW(objFilePath);

            print("filePath: "+ objFilePath);
            // OBJ ���� �ε�
            GameObject importedObj = new OBJLoader().Load(objFilePath);
            print(importedObj);
#else
            GameObject importedObj = new OBJLoader().Load(Application.persistentDataPath + "/3D_Models/ModelDatas/" + modelName + ".obj");
#endif
            if (importedObj != null)
            {
                importedObj.transform.SetParent(objectToPlace.transform); // Set parent.
                importedObj.transform.localPosition = Vector3.zero;
                importedObj.transform.localRotation = Quaternion.identity;
                importedObj.transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);

                // �޽��� �븻�� �������ϴ�.
                Mesh mesh = importedObj.GetComponentInChildren<MeshFilter>().mesh;
                FlipMesh(mesh);

                GameObject meshObj = importedObj.transform.GetChild(0).gameObject;

                // ���� ����
                ApplyMaterials(modelName, meshObj);

                meshObj.gameObject.layer = 10;
            }
            //else
            //{
            //    Debug.LogError("Failed to load OBJ file: " + objFilePath);
            //}
        }
    }

    private void FlipMesh(Mesh mesh)
    {
        int[] triangles = mesh.triangles;
        int temp;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            temp = triangles[i];
            triangles[i] = triangles[i + 1];
            triangles[i + 1] = temp;
        }
        mesh.triangles = triangles;

        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] *= -1;
        }
        mesh.normals = normals;

        // ���� �޴� ������ �̻�������
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
    }

    // mat ����
    private void ApplyMaterials(string modelName, GameObject obj)
    {
        Texture2D albedoTexture = null;
#if UNITY_EDITOR
        // �����Ϳ����� Resources �������� �ؽ�ó�� �ε��մϴ�.
        //albedoTexture = Resources.Load<Texture2D>("3D_Models/ModelDatas/" + modelName);
        string path = Path.Combine(Application.persistentDataPath, "3D_Models/ModelDatas/" + modelName + ".png");

        if (File.Exists(path))
        {
            // ���Ͽ��� ����Ʈ �迭�� �н��ϴ�.
            byte[] fileData = File.ReadAllBytes(path);

            // �� Texture2D �ν��Ͻ��� �����մϴ�.
            albedoTexture = new Texture2D(2, 2);

            // ���� ����Ʈ �����ͷ� �ؽ�ó�� �ε��մϴ�.
            albedoTexture.LoadImage(fileData); // �ؽ�ó�� ũ�Ⱑ �ڵ����� �����˴ϴ�.
        }
#elif UNITY_STANDALONE
        //albedoTexture = Resources.Load<Texture2D>("3D_Models/ModelDatas/" + modelName);
        string path = Path.Combine(Application.persistentDataPath, "3D_Models/ModelDatas/" + modelName + ".png");

        if (File.Exists(path))
        {
            // ���Ͽ��� ����Ʈ �迭�� �н��ϴ�.
            byte[] fileData = File.ReadAllBytes(path);

            // �� Texture2D �ν��Ͻ��� �����մϴ�.
            albedoTexture = new Texture2D(2, 2);

            // ���� ����Ʈ �����ͷ� �ؽ�ó�� �ε��մϴ�.
            albedoTexture.LoadImage(fileData); // �ؽ�ó�� ũ�Ⱑ �ڵ����� �����˴ϴ�.
        }
#elif UNITY_ANDROID
    // �ȵ���̵忡���� persistentDataPath���� �ؽ�ó�� �ε��մϴ�.
    string textureFilePath = Path.Combine(Application.persistentDataPath, "3D_Models/ModelDatas", modelName + ".png"); // Ȯ���ڰ� �ʿ��մϴ�.

    if (File.Exists(textureFilePath))
    {
        byte[] bytes = File.ReadAllBytes(textureFilePath);
        albedoTexture = new Texture2D(2, 2);
        albedoTexture.LoadImage(bytes);
        albedoTexture.Apply();
    }
    else
    {
        Debug.LogError("Texture file not found: " + textureFilePath);
        return; // ������ ������ ���⼭ ó���� �ߴ��մϴ�.
    }
#endif
        // �ؽ�ó ���� ����
        if (albedoTexture != null)
        {
            print(albedoTexture.name);

            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] != null)
                    {
                        Material newMaterial = new Material(materials[i]);
                        newMaterial.mainTexture = albedoTexture;
                        materials[i] = newMaterial;
                    }
                }
                renderer.sharedMaterials = materials;
            }
        }
        else
        {
            Debug.LogError("Failed to load texture.");
        }
    }

    // å�� ���ְ� �Ͼ���ϱ�
    public void OnClickDisableDesk()
    {
        photonView.RPC("OnClickDisableDeskRPC", RpcTarget.All);

        photonView.RPC("animPlayRPC", RpcTarget.Others, "Idle");
        characterInteraction.SetPlayerIdle();

        print("å�� ���ֱ�");
    }

    // å�� �پ��ֱ�
    [PunRPC]
    private void OnClickDisableDeskRPC()
    {
        classRoomHandler.StudentDesk.SetActive(isDisableBtnClick);
        classRoomHandler.Floor.SetActive(isDisableBtnClick);
        classRoomHandler.FloorWithoutShadow.SetActive(!isDisableBtnClick);

        isDisableBtnClick = !isDisableBtnClick;
    }

    public void OnClickCloseButton()
    {
        scrollView.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => scrollView.SetActive(false));
        isSpawnBtnClick = false;
    }
}