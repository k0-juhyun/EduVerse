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

public class TeacherInteraction : MonoBehaviourPun
{
    public GameObject spawnButton;
    private GameObject player;
    public GameObject buttonPrefab;
    public GameObject scrollView;

    private bool isObjectBeingPlaced = false;
    private GameObject objectToPlace;
    private GameObject currentlyDragging;

    public string TestName;

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

    public void SpawnModel(int modelIndex)
    {
        if (!isObjectBeingPlaced && modelIndex >= 0 && modelIndex < DataBase.instance.model.spawnPrefab.Count)
        {
            GameObject modelToSpawn = DataBase.instance.model.spawnPrefab[modelIndex];

            print(modelToSpawn.name);
            // Instantiate model and check for success.
            objectToPlace = PhotonNetwork.Instantiate("3D_Models/mesh", player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

            // Only proceed if objectToPlace has been successfully created.
            if (objectToPlace != null)
            {
                isObjectBeingPlaced = true; // Set state to placing object.
                currentlyDragging = objectToPlace; // Set current dragging object.

                // Call RPC to load and apply OBJ to the newly instantiated object.
                // We pass objectToPlace's PhotonView ID to ensure the correct object is targeted.
                photonView.RPC("LoadAndApplyOBJ", RpcTarget.AllBuffered, modelToSpawn.name, objectToPlace.GetPhotonView().ViewID);
                print(modelToSpawn.name);
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
            objectToPlace = targetPhotonView.gameObject;

            string objFilePath = Path.Combine(Application.persistentDataPath, "3D_Models/ModelDatas/", modelName + ".obj");

            // OBJ 파일 로드
            GameObject importedObj = new OBJLoader().Load(objFilePath);
            if (importedObj != null)
            {
                importedObj.transform.SetParent(objectToPlace.transform); // Set parent.
                importedObj.transform.localPosition = Vector3.zero;
                importedObj.transform.localRotation = Quaternion.identity;

                // 메시의 노말을 뒤집습니다.
                Mesh mesh = importedObj.GetComponentInChildren<MeshFilter>().mesh;
                FlipMesh(mesh);

                GameObject meshObj = importedObj.transform.GetChild(0).gameObject;

                // 재질 적용
                ApplyMaterials(modelName, meshObj);
            }
            else
            {
                Debug.LogError("Failed to load OBJ file: " + objFilePath);
            }
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
        StartCoroutine(LoadTextureAndApply(modelName, obj));
    }

    private IEnumerator LoadTextureAndApply(string modelName, GameObject obj)
    {
        string texturePath = Path.Combine(Application.persistentDataPath, "3D_Models/ModelDatas/", modelName + ".png");
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture("file:///" + texturePath);

        yield return textureRequest.SendWebRequest();

        if (textureRequest.result == UnityWebRequest.Result.Success)
        {
            Texture2D albedoTexture = DownloadHandlerTexture.GetContent(textureRequest);
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
            Debug.LogError("Failed to load texture: " + textureRequest.error);
        }
    }

}
