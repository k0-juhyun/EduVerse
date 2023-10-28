using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public Image circleImg;
    public Image circleImg1;
    public Image circleImg2;
    public Text textProgress;

    [Range(0, 1)]
    public float progress = 0;

    private float loadingSpeed = 5f; // 이 값을 조절하여 로딩 속도를 변경할 수 있습니다.

    private void Start()
    {
        progress = 0;
    }
    private void Update()
    {
        if (progress < 1) // progress가 1보다 작을 때만 업데이트합니다.
        {
            progress += Time.deltaTime * loadingSpeed;
            progress = Mathf.Min(progress, 1); // progress를 1로 제한합니다.

            circleImg.fillAmount = progress;
            circleImg1.fillAmount = progress;
            circleImg2.fillAmount = progress;

            // 소수점 없이 정수로 표현
            int progressPercentage = Mathf.FloorToInt(progress * 100);
            textProgress.text = progressPercentage.ToString();
        }
    }
}
