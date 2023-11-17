using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student_QuizData : MonoBehaviour
{
    // ���� 
    // 1 student db���� UID ��ư ����Լ� ����
    // 2. �� ��ư ������ �� QuizToBase�� �ִ� GetQuizData �Լ� ����. (UID, obj) ����
    // 3. ������Ʈ�� ������ �� ������Ʈ�� �ִ� studentQuizInfo ���� �ٲ���.
    // 4. �� �����͸� ����� �� ������Ʈ.


    [HideInInspector]
    public QuizInfo StudentQuizInfo;

    [HideInInspector]
    public GameObject MygameObject;

    [HideInInspector]
    public string UID;
    // Start is called before the first frame update
    void Start()
    {
        MygameObject = this.gameObject;

        // �л� UID�� �����.

        //Debug.Log(QuizToFireBase.instance.submitQuizCnt);
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log(StudentQuizInfo.QuizCorrectAnswerCnt);
        }
    }

    // �� �Լ��� ������������.
    public void test()
    {

        Debug.Log(StudentQuizInfo.QuizCorrectAnswerCnt);
    }

    // UID �޾� ��ư ���.
    public void StudentUIDSaveBtnClick(string uid)
    {
        // ��ư�� �������� ����� UID�� ������Ʈ�� �����Ѵ�.
        GetComponent<Button>().onClick.AddListener(() => QuizToFireBase.instance.GetQuizData(uid, MygameObject));
    }
}
