using DG.Tweening;
using Dummiesman;
using Photon.Chat.UtilityScripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    //public void SpawnModel(int modelIndex)
    //{
    //    if (!isObjectBeingPlaced && modelIndex >= 0 && modelIndex < DataBase.instance.model.spawnPrefab.Count)
    //    {
    //        GameObject modelToSpawn = DataBase.instance.model.spawnPrefab[modelIndex];
    //        objectToPlace = PhotonNetwork.Instantiate("3D_Models/" + modelToSpawn.name, player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

    //        // 이 부분에서 .obj 파일을 불러옵니다.
    //        GameObject importedObj = new OBJLoader().Load("Assets/Resources/3D_Models/mesh_3/mesh.obj");
    //        importedObj.transform.SetParent(objectToPlace.transform); // objectToPlace를 부모로 설정합니다.
    //        importedObj.transform.localPosition = Vector3.zero;
    //        importedObj.transform.localRotation = Quaternion.identity;

    //        // 메시의 노말을 뒤집습니다.
    //        Mesh mesh = importedObj.GetComponentInChildren<MeshFilter>().mesh;

    //        FlipMesh(mesh);

    //        GameObject meshObj = importedObj.transform.GetChild(0).gameObject;

    //        // 필요하다면 재질을 적용합니다.
    //        ApplyMaterials(meshObj);

    //        // 오브젝트 생성을 완료합니다.
    //        isObjectBeingPlaced = true; // 오브젝트 생성중 상태로 설정
    //        currentlyDragging = objectToPlace; // 현재 드래깅 오브젝트로 설정
    //    }
    //}

    public void SpawnModel(int modelIndex)
    {
        if (!isObjectBeingPlaced && modelIndex >= 0 && modelIndex < DataBase.instance.model.spawnPrefab.Count)
        {
            GameObject modelToSpawn = DataBase.instance.model.spawnPrefab[modelIndex];

            // 이 부분에서 Instantiate가 성공적으로 반환되었는지 확인합니다.
            objectToPlace = PhotonNetwork.Instantiate("3D_Models/" + modelToSpawn.name, player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

            // objectToPlace가 정상적으로 생성되었는지 확인합니다.
            if (objectToPlace != null)
            {
                // RPC를 사용하여 모든 클라이언트에서 OBJ 로딩과 머티리얼 적용을 수행하도록 요청합니다.
                photonView.RPC("LoadAndApplyOBJ", RpcTarget.AllBuffered, modelToSpawn.name);

                // 오브젝트 생성을 완료합니다.
                isObjectBeingPlaced = true; // 오브젝트 생성중 상태로 설정
                currentlyDragging = objectToPlace; // 현재 드래깅 오브젝트로 설정
            }
            else
            {
                // 오브젝트 생성 실패 처리
                Debug.LogError("Failed to instantiate object on network.");
            }
        }
    }

    [PunRPC]
    public void LoadAndApplyOBJ(string modelName)
    {
        // 이 부분에서 .obj 파일을 불러옵니다.
        if (objectToPlace == null)
        {
            Debug.LogError("objectToPlace is null!");
            return;
        }

        // 이 부분에서 .obj 파일을 불러옵니다.
        GameObject importedObj = new OBJLoader().Load("Assets/Resources/3D_Models/mesh_3/mesh.obj");
        if (importedObj == null)
        {
            Debug.LogError("Failed to load OBJ file.");
            return;
        }
        importedObj.transform.SetParent(objectToPlace.transform); // objectToPlace를 부모로 설정합니다.
        importedObj.transform.localPosition = Vector3.zero;
        importedObj.transform.localRotation = Quaternion.identity;

        // 메시의 노말을 뒤집습니다.
        Mesh mesh = importedObj.GetComponentInChildren<MeshFilter>().mesh;
        FlipMesh(mesh);

        GameObject meshObj = importedObj.transform.GetChild(0).gameObject;

        // 필요하다면 재질을 적용합니다.
        ApplyMaterials(meshObj);
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

    private void ApplyMaterials(GameObject obj)
    {
        // "mesh_3/mesh"라는 이름의 텍스처를 로드합니다.
        Texture2D albedoTexture = Resources.Load<Texture2D>("3D_Models/mesh_3/mesh");

        if (albedoTexture != null)
        {
            print(albedoTexture.name);

            // 오브젝트의 모든 렌더러를 순회하며 각 렌더러의 모든 재질에 텍스처를 할당합니다.
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] != null)
                    {
                        // 새 Material을 생성하기 전에 기존 Material의 속성을 복사합니다.
                        Material newMaterial = new Material(materials[i]); // 기존 Material을 복사하여 새 Material 생성
                        newMaterial.mainTexture = albedoTexture; // 알베도 텍스처 할당
                        materials[i] = newMaterial; // Material 교체
                    }
                }
                renderer.sharedMaterials = materials; // 수정된 재질 배열을 렌더러에 다시 할당합니다.
            }
        }
    }
}
