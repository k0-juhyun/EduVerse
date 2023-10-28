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

    private float loadingSpeed = 5f; // �� ���� �����Ͽ� �ε� �ӵ��� ������ �� �ֽ��ϴ�.

    private void Start()
    {
        progress = 0;
    }
    private void Update()
    {
        if (progress < 1) // progress�� 1���� ���� ���� ������Ʈ�մϴ�.
        {
            progress += Time.deltaTime * loadingSpeed;
            progress = Mathf.Min(progress, 1); // progress�� 1�� �����մϴ�.

            circleImg.fillAmount = progress;
            circleImg1.fillAmount = progress;
            circleImg2.fillAmount = progress;

            // �Ҽ��� ���� ������ ǥ��
            int progressPercentage = Mathf.FloorToInt(progress * 100);
            textProgress.text = progressPercentage.ToString();
        }
    }
}
