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
    public LoadButton loadButton;

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
            Debug.Log("실행");
            ShareCam.orthographicSize = 1.9f;
            subMainCam.transform.position = new Vector3(-6, 2.525f, -0.0f);
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
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            maincam.depth = 1;
            maincam.gameObject.tag = "MainCamera";
            ShareCam.gameObject.tag = "Untagged";
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
            // 오디오 리스너 에러
            transform.GetComponent<AudioListener>().enabled = true;
        }

        else
        {
            // 시점 풀릴때
            //canvas.SetActive(false);
            maincam.depth = 1;
            maincam.gameObject.tag = "MainCamera";
            ShareCam.gameObject.tag = "Untagged";
            camera = !camera;

            // 캔버스 키고 다시 클릭 버튼 가능
            startStudy.isClick = false;
            startStudy.enableCanvas = true;
            startStudy._isDrawing = false;
            startStudy._cameraSetting.TPS_Camera.gameObject.SetActive(true);
            //Voice.instance.ActiveToggleCanvas(true);
            // 오디오 리스너 에러

            transform.GetComponent<AudioListener>().enabled = false;
            loadButton.DestroyAllButtons();
        }
    }
}
