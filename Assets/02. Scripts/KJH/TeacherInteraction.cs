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
    [Header("버튼")]
    public GameObject spawnButton;
    public GameObject disableDeskButton;
    private GameObject player;
    public GameObject quizButton;

    [Header("3D모델링 버튼")]
    public GameObject buttonPrefab;
    [Header("3D모델링 스크롤뷰")]
    public GameObject scrollView;

    private GameObject objectToPlace;
    private GameObject currentlyDragging;


    private Button Btn_Spawn;
    private Button Btn_Disable;

    [HideInInspector] public bool isDisableBtnClick;
    private bool isObjectBeingPlaced = false;
    [HideInInspector] public bool isSpawnBtnClick;

    [Header("3D모델링 버튼 스폰 부모")]
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
            // 마우스 위치를 월드 좌표로 변환합니다.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            // Z 좌표를 캐릭터 앞으로 조정합니다.
            mousePosition.z = player.transform.position.z + 1.0f;

            // 오브젝트를 마우스 커서 위치로 이동시킵니다.
            objectToPlace.transform.position = mousePosition;

            // 마우스 버튼을 다시 누르면 오브젝트를 배치합니다.
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

        // 스크롤뷰의 Content 내에 버튼들을 생성합니다.
        for (int i = 0; i < modelCount; i++)
        {
            int index = i; // 클로저 문제 방지를 위한 로컬 변수
            GameObject newButton = Instantiate(buttonPrefab, buttonsParent);
            newButton.GetComponentInChildren<TMP_Text>().text = DataBase.instance.model.spawnPrefab[index].name; // 원래 프리팹의 이름으로 버튼 텍스트 할당
            newButton.GetComponent<Button>().onClick.AddListener(() => SpawnModel(index)); // 리스너 추가

            // rawImage 컴포넌트
            RawImage thumbNailImage = newButton.transform.GetChild(1).GetComponent<RawImage>();
            print(thumbNailImage.gameObject);

            // 리소스 로드 (경로 수정)
            string texturePath = "Thumbnail/" + DataBase.instance.model.spawnPrefab[index].name;
            Texture2D thumbNailTexture = Resources.Load<Texture2D>(texturePath);

            // 텍스처 할당
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

            // 포톤뷰를 가진 빈오브젝트를 불러와서
            objectToPlace = PhotonNetwork.Instantiate("3D_Models/mesh", player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

            //scrollView.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InBack)
            //.OnComplete(() => scrollView.SetActive(false));

            isSpawnBtnClick = false;

            if (objectToPlace != null)
            {
                isObjectBeingPlaced = true;
                currentlyDragging = objectToPlace;

                // rpc 로 오브젝트 생성
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
            // OBJ 파일 로드
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

                // 메시의 노말을 뒤집습니다.
                Mesh mesh = importedObj.GetComponentInChildren<MeshFilter>().mesh;
                FlipMesh(mesh);

                GameObject meshObj = importedObj.transform.GetChild(0).gameObject;

                // 재질 적용
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

        // 빛을 받는 영역이 이상해진다
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
    }

    // mat 적용
    private void ApplyMaterials(string modelName, GameObject obj)
    {
        Texture2D albedoTexture = null;
#if UNITY_EDITOR
        // 에디터에서는 Resources 폴더에서 텍스처를 로드합니다.
        //albedoTexture = Resources.Load<Texture2D>("3D_Models/ModelDatas/" + modelName);
        string path = Path.Combine(Application.persistentDataPath, "3D_Models/ModelDatas/" + modelName + ".png");

        if (File.Exists(path))
        {
            // 파일에서 바이트 배열을 읽습니다.
            byte[] fileData = File.ReadAllBytes(path);

            // 새 Texture2D 인스턴스를 생성합니다.
            albedoTexture = new Texture2D(2, 2);

            // 읽은 바이트 데이터로 텍스처를 로드합니다.
            albedoTexture.LoadImage(fileData); // 텍스처의 크기가 자동으로 설정됩니다.
        }
#elif UNITY_STANDALONE
        //albedoTexture = Resources.Load<Texture2D>("3D_Models/ModelDatas/" + modelName);
        string path = Path.Combine(Application.persistentDataPath, "3D_Models/ModelDatas/" + modelName + ".png");

        if (File.Exists(path))
        {
            // 파일에서 바이트 배열을 읽습니다.
            byte[] fileData = File.ReadAllBytes(path);

            // 새 Texture2D 인스턴스를 생성합니다.
            albedoTexture = new Texture2D(2, 2);

            // 읽은 바이트 데이터로 텍스처를 로드합니다.
            albedoTexture.LoadImage(fileData); // 텍스처의 크기가 자동으로 설정됩니다.
        }
#elif UNITY_ANDROID
    // 안드로이드에서는 persistentDataPath에서 텍스처를 로드합니다.
    string textureFilePath = Path.Combine(Application.persistentDataPath, "3D_Models/ModelDatas", modelName + ".png"); // 확장자가 필요합니다.

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
        return; // 파일이 없으면 여기서 처리를 중단합니다.
    }
#endif
        // 텍스처 적용 로직
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

    // 책상 없애고 일어서게하기
    public void OnClickDisableDesk()
    {
        photonView.RPC("OnClickDisableDeskRPC", RpcTarget.All);

        photonView.RPC("animPlayRPC", RpcTarget.Others, "Idle");
        characterInteraction.SetPlayerIdle();

        print("책상 없애기");
    }

    // 책상 다없애기
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