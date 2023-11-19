using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public GameObject QuizdataCanvs;
    // Start is called before the first frame update
    void Start()
    {
        MygameObject = this.gameObject;

        // �л� UID�� �����.

        //�ϴ� �׽�Ʈ��
        //GetComponent<Button>().onClick.AddListener(() => QuizToFireBase.instance.GetQuizData("27KHHFa2SWcs9Yo5L4A8zKOEls52", MygameObject));
        GetComponent<Button>().onClick.AddListener(StudentQuizDataUpdateBtnClick);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StudentUIDSaveBtnClick("27KHHFa2SWcs9Yo5L4A8zKOEls52");
        }
    }

    // �� �Լ��� ������������.
    public void StudentQuizDataUpdateBtnClick()
    {

        StartCoroutine(QuizdataUpdate());
    }
    IEnumerator QuizdataUpdate()
    {
        // ���Ƿ� 1�� ��ٸ�
        // StudentQuizInfo �ε� �ð��� ����.
        StudentDB.instance.OffStudentDB();
        yield return new WaitForSeconds(0.3f);

        // ���� �ִ� ������Ʈ�� ���� �ٲٵ���
        // �ƴ� �����ؼ� �ϵ���. ��� �ұ�.

        // ������Ʈ �̸����� ������.
        GameObject quizCanvas = GameObject.Find("������ ����");


        // string ���� null�� �Ǹ� NaN���� ��.
        // ��� ��Ǭ �ܿ����� �� �ΰ����� ����� ��..
        // �ܿ��� ���

        // ������ ������Ʈ StudentQuizDB ��ũ��Ʈ ������.
        StudentQuizDB QuizDB = quizCanvas.GetComponent<StudentQuizDB>();
        Debug.Log(QuizDB);

        // �ܿ��� �ݿ� �׷���.
        QuizDB.Unit_1.semicircleTween(
            (average(StudentQuizInfo.Unit_1) * 0.5f));
        QuizDB.Unit_2.semicircleTween(
            (average(StudentQuizInfo.Unit_2) * 0.5f));
        QuizDB.Unit_3.semicircleTween(
            (average(StudentQuizInfo.Unit_3) * 0.5f));
        QuizDB.Unit_4.semicircleTween(
            (average(StudentQuizInfo.Unit_4) * 0.5f));
        QuizDB.Unit_5.semicircleTween(
            (average(StudentQuizInfo.Unit_5) * 0.5f));

        // �ܿ��� ���
        QuizDB.Unit_1_Average.text =
            StudentQuizInfo.Unit_1.CorrectAnswer.Count.ToString() + "/" +
            (StudentQuizInfo.Unit_1.CorrectAnswer.Count + StudentQuizInfo.Unit_1.IncorrectAnswer.Count).ToString();
        QuizDB.Unit_2_Average.text =
            StudentQuizInfo.Unit_2.CorrectAnswer.Count.ToString() + "/" +
            (StudentQuizInfo.Unit_2.CorrectAnswer.Count + StudentQuizInfo.Unit_2.IncorrectAnswer.Count).ToString();
        QuizDB.Unit_3_Average.text =
            StudentQuizInfo.Unit_3.CorrectAnswer.Count.ToString() + "/" +
            (StudentQuizInfo.Unit_3.CorrectAnswer.Count + StudentQuizInfo.Unit_3.IncorrectAnswer.Count).ToString();
        QuizDB.Unit_4_Average.text =
            StudentQuizInfo.Unit_4.CorrectAnswer.Count.ToString() + "/" +
            (StudentQuizInfo.Unit_4.CorrectAnswer.Count + StudentQuizInfo.Unit_4.IncorrectAnswer.Count).ToString();
        QuizDB.Unit_5_Average.text =
            StudentQuizInfo.Unit_5.CorrectAnswer.Count.ToString() + "/" +
            (StudentQuizInfo.Unit_5.CorrectAnswer.Count + StudentQuizInfo.Unit_5.IncorrectAnswer.Count).ToString();



        List<titleinfo> a = StudentQuizInfo.Unit_1.CorrectAnswer;
        

        // �� Ǭ ����.
        QuizDB.CorrectCnt_AnswerCnt.text = StudentQuizInfo.QuizCorrectAnswerCnt.ToString() + "/" + StudentQuizInfo.QuizAnswerCnt.ToString();

        // ��ü ���
        // 0���� �� ������ ������
        if((float)StudentQuizInfo.QuizAnswerCnt == 0)
        {
            QuizDB.Average.text = "0";
        }
        else
        {
        QuizDB.Average.text =
            ((float)StudentQuizInfo.QuizCorrectAnswerCnt / ((float)StudentQuizInfo.QuizAnswerCnt) * 100).ToString();
        }


        // ������ ���� QuizDB �� ������Ʈ�� quizinfo�� ���� ������ �Ѱ���.
        QuizDB.studentQuizinfo = StudentQuizInfo;

        // ����ϰ� �����ϰ� �� ������Ʈ�� �ִ� �г� ����.
        StudentDB.instance.ShowPersonalDB();

    }

    // UID �޾� ��ư ���.
    public void StudentUIDSaveBtnClick(string uid)
    {
        MygameObject = this.gameObject;
        Debug.Log(uid);
        // ��ư�� �������� ����� UID�� ������Ʈ�� �����Ѵ�.
        // �������⿣ ����� �ȉ�.
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Listener added for GetQuizData");
            QuizToFireBase.instance.GetQuizData(uid, MygameObject);
        });
    }

    // �ݿ� ��ճ���.
    float average(answerinfo answerinfo)
    {
        float correctCount = (float)answerinfo.CorrectAnswer.Count;
        float totalCount = correctCount + (float)answerinfo.IncorrectAnswer.Count;

        float result = totalCount != 0 ? correctCount / totalCount : 0f;
        return result;
    }
}
