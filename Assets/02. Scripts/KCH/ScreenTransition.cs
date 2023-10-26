using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    Camera maincam;
    Camera ShareCam;
    public GameObject canvas;
    public GameObject teacherComputer;

    private StartStudy startStudy;
    bool camera;


    private void Start()
    {
        maincam = Camera.main;
        ShareCam = GetComponent<Camera>();

        startStudy = teacherComputer.GetComponent<StartStudy>();
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
        }

        else
        {
            // 시점 풀릴때
            canvas.SetActive(false);
            maincam.depth = 1;
            maincam.gameObject.tag = "MainCamera";
            ShareCam.gameObject.tag = "Untagged";
            camera = !camera;

            // 캔버스 키고 다시 클릭 버튼 가능
            startStudy.isClick = false;
            startStudy.enableCanvas = true;

        }
    }
}
