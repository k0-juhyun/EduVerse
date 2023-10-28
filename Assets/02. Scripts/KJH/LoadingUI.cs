using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    #region ºù±Ûºù±Û
    //public RectTransform Icon;

    //public float timeStep;
    //public float onStepAngle;

    //float startTime;

    //void Start()
    //{
    //    startTime = Time.deltaTime;    
    //}

    //void Update()
    //{
    //    if(Time.time - startTime > timeStep) 
    //    {
    //        Vector3 iconAgle = Icon.localEulerAngles;
    //        iconAgle.z += onStepAngle;

    //        Icon.localEulerAngles = iconAgle;

    //        startTime = Time.time;
    //    }
    //}
    #endregion

    public Image circleImg;
    public Image circleImg1;
    public Image circleImg2;
    public Text textProgress;

    [Range(0, 1)]
    public float progress = 0;

    private void Start()
    {
        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager != null)
        {
            networkManager.OnLoadSceneProgress += SetProgress;
        }
    }

    private void OnDestroy()
    {
        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager != null)
        {
            networkManager.OnLoadSceneProgress -= SetProgress;
        }
    }

    private void SetProgress(float progress)
    {
        circleImg.fillAmount = progress;
        circleImg1.fillAmount = progress;
        circleImg2.fillAmount = progress;
        textProgress.text = Mathf.Floor(progress * 100).ToString();
    }
}
