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

    //public void SpawnModel(int modelIndex)
    //{
    //    if (!isObjectBeingPlaced && modelIndex >= 0 && modelIndex < DataBase.instance.model.spawnPrefab.Count)
    //    {
    //        GameObject modelToSpawn = DataBase.instance.model.spawnPrefab[modelIndex];
    //        objectToPlace = PhotonNetwork.Instantiate("3D_Models/" + modelToSpawn.name, player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

    //        // �� �κп��� .obj ������ �ҷ��ɴϴ�.
    //        GameObject importedObj = new OBJLoader().Load("Assets/Resources/3D_Models/mesh_3/mesh.obj");
    //        importedObj.transform.SetParent(objectToPlace.transform); // objectToPlace�� �θ�� �����մϴ�.
    //        importedObj.transform.localPosition = Vector3.zero;
    //        importedObj.transform.localRotation = Quaternion.identity;

    //        // �޽��� �븻�� �������ϴ�.
    //        Mesh mesh = importedObj.GetComponentInChildren<MeshFilter>().mesh;

    //        FlipMesh(mesh);

    //        GameObject meshObj = importedObj.transform.GetChild(0).gameObject;

    //        // �ʿ��ϴٸ� ������ �����մϴ�.
    //        ApplyMaterials(meshObj);

    //        // ������Ʈ ������ �Ϸ��մϴ�.
    //        isObjectBeingPlaced = true; // ������Ʈ ������ ���·� ����
    //        currentlyDragging = objectToPlace; // ���� �巡�� ������Ʈ�� ����
    //    }
    //}

    public void SpawnModel(int modelIndex)
    {
        if (!isObjectBeingPlaced && modelIndex >= 0 && modelIndex < DataBase.instance.model.spawnPrefab.Count)
        {
            GameObject modelToSpawn = DataBase.instance.model.spawnPrefab[modelIndex];

            // �� �κп��� Instantiate�� ���������� ��ȯ�Ǿ����� Ȯ���մϴ�.
            objectToPlace = PhotonNetwork.Instantiate("3D_Models/" + modelToSpawn.name, player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

            // objectToPlace�� ���������� �����Ǿ����� Ȯ���մϴ�.
            if (objectToPlace != null)
            {
                // RPC�� ����Ͽ� ��� Ŭ���̾�Ʈ���� OBJ �ε��� ��Ƽ���� ������ �����ϵ��� ��û�մϴ�.
                photonView.RPC("LoadAndApplyOBJ", RpcTarget.AllBuffered, modelToSpawn.name);

                // ������Ʈ ������ �Ϸ��մϴ�.
                isObjectBeingPlaced = true; // ������Ʈ ������ ���·� ����
                currentlyDragging = objectToPlace; // ���� �巡�� ������Ʈ�� ����
            }
            else
            {
                // ������Ʈ ���� ���� ó��
                Debug.LogError("Failed to instantiate object on network.");
            }
        }
    }

    [PunRPC]
    public void LoadAndApplyOBJ(string modelName)
    {
        // �� �κп��� .obj ������ �ҷ��ɴϴ�.
        if (objectToPlace == null)
        {
            Debug.LogError("objectToPlace is null!");
            return;
        }

        // �� �κп��� .obj ������ �ҷ��ɴϴ�.
        GameObject importedObj = new OBJLoader().Load("Assets/Resources/3D_Models/mesh_3/mesh.obj");
        if (importedObj == null)
        {
            Debug.LogError("Failed to load OBJ file.");
            return;
        }
        importedObj.transform.SetParent(objectToPlace.transform); // objectToPlace�� �θ�� �����մϴ�.
        importedObj.transform.localPosition = Vector3.zero;
        importedObj.transform.localRotation = Quaternion.identity;

        // �޽��� �븻�� �������ϴ�.
        Mesh mesh = importedObj.GetComponentInChildren<MeshFilter>().mesh;
        FlipMesh(mesh);

        GameObject meshObj = importedObj.transform.GetChild(0).gameObject;

        // �ʿ��ϴٸ� ������ �����մϴ�.
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

        // ���� �޴� ������ �̻�������
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
    }

    private void ApplyMaterials(GameObject obj)
    {
        // "mesh_3/mesh"��� �̸��� �ؽ�ó�� �ε��մϴ�.
        Texture2D albedoTexture = Resources.Load<Texture2D>("3D_Models/mesh_3/mesh");

        if (albedoTexture != null)
        {
            print(albedoTexture.name);

            // ������Ʈ�� ��� �������� ��ȸ�ϸ� �� �������� ��� ������ �ؽ�ó�� �Ҵ��մϴ�.
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] != null)
                    {
                        // �� Material�� �����ϱ� ���� ���� Material�� �Ӽ��� �����մϴ�.
                        Material newMaterial = new Material(materials[i]); // ���� Material�� �����Ͽ� �� Material ����
                        newMaterial.mainTexture = albedoTexture; // �˺��� �ؽ�ó �Ҵ�
                        materials[i] = newMaterial; // Material ��ü
                    }
                }
                renderer.sharedMaterials = materials; // ������ ���� �迭�� �������� �ٽ� �Ҵ��մϴ�.
            }
        }
    }
}
