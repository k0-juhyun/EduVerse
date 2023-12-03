using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviourPun
{
    Camera maincam;
    Camera ShareCam;
    GameObject subMainCam;
    public GameObject canvas;
    public GameObject teacherComputer;

    private StartStudy startStudy;
    bool camera;


    private void Start()
    {
        maincam = Camera.main;
        subMainCam = this.gameObject;
        ShareCam = GetComponent<Camera>();

        startStudy = teacherComputer.GetComponent<StartStudy>();

        if (DataBase.instance.user.isTeacher == false)
        {
            Debug.Log("����");
            ShareCam.orthographicSize = 2.16f;
            subMainCam.transform.position = new Vector3(-6, 2.525f, -0.02f);
            subMainCam.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha2))
        {
            maincam.depth = -1;
            maincam.gameObject.tag = "Untagged";
            ShareCam.gameObject.tag = "MainCamera";
            Debug.Log(ShareCam.gameObject.tag);
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            maincam.depth = 1;
            maincam.gameObject.tag = "MainCamera";
            ShareCam.gameObject.tag = "Untagged";
            Debug.Log(maincam.gameObject.tag);
        }
    }

    public void ChangeCamera()
    {
        if (!camera)
        {
            canvas.SetActive(true);
            maincam.depth = -1;
            maincam.gameObject.tag = "Untagged";
            ShareCam.gameObject.tag = "MainCamera";
            camera = !camera;
            //Voice.instance.ActiveToggleCanvas(false);
            // ����� ������ ����
            transform.GetComponent<AudioListener>().enabled = true;
            LoadButton.instance.DestroyAllButtons();
        }

        else
        {
            // ���� Ǯ����
            //canvas.SetActive(false);
            maincam.depth = 1;
            maincam.gameObject.tag = "MainCamera";
            ShareCam.gameObject.tag = "Untagged";
            camera = !camera;

            // ĵ���� Ű�� �ٽ� Ŭ�� ��ư ����
            startStudy.isClick = false;
            startStudy.enableCanvas = true;
            startStudy._isDrawing = false;
            startStudy._cameraSetting.TPS_Camera.gameObject.SetActive(true);
            //Voice.instance.ActiveToggleCanvas(true);
            // ����� ������ ����

            transform.GetComponent<AudioListener>().enabled = false;
            LoadButton.instance.DestroyAllButtons();
        }
    }
}
