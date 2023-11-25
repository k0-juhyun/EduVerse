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
        yield return new WaitForSeconds(0.5f);

        // ���� �ִ� ������Ʈ�� ���� �ٲٵ���
        // �ƴ� �����ؼ� �ϵ���. ��� �ұ�.

        // ������Ʈ �̸����� ������.
        GameObject quizCanvas = StudentDB.instance.AnalysisDB;

        Debug.Log("quizCanvas" + quizCanvas);


        // string ���� null�� �Ǹ� NaN���� ��.
        // ��� ��Ǭ �ܿ����� �� �ΰ����� ����� ��..
        // �ܿ��� ���

        // ������ ������Ʈ StudentQuizDB ��ũ��Ʈ ������.
        StudentQuizDB QuizDB = quizCanvas.GetComponent<StudentQuizDB>();
        Debug.Log("quizdb" + QuizDB);

        // ó���� ������ ���ش�.
        #region ����
        //QuizDB.Unit_1.Reset_value();      QuizDB.Unit_2.Reset_value();       QuizDB.Unit_3.Reset_value();
        //QuizDB.Unit_4.Reset_value();        QuizDB.Unit_5.Reset_value();

        //QuizDB.Unit_1_Average.text = "0 / 0";
        //QuizDB.Unit_2_Average.text = "0 / 0";
        //QuizDB.Unit_3_Average.text = "0 / 0";
        //QuizDB.Unit_4_Average.text = "0 / 0";
        //QuizDB.Unit_5_Average.text = "0 / 0";

        //QuizDB.CorrectCnt_AnswerCnt.text = "0 / 0";

        //QuizDB.Average.text = "0";
        #endregion

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


        // �� Ǭ ����.
        QuizDB.CorrectCnt_AnswerCnt.text = StudentQuizInfo.QuizCorrectAnswerCnt.ToString() + "/" + StudentQuizInfo.QuizAnswerCnt.ToString();

        // ��ü ���
        // 0���� �� ������ ������
        if ((float)StudentQuizInfo.QuizAnswerCnt == 0)
        {
            QuizDB.Average.text = "0";
        }
        else
        {
            // �Ҽ��� ��° �ڸ�.
            QuizDB.Average.text =
                ((float)StudentQuizInfo.QuizCorrectAnswerCnt / ((float)StudentQuizInfo.QuizAnswerCnt) * 100).ToString("F2");
        }

        // �ܿ� ��.
        // �ϴ� ������ ��Ǭ �ܿ��� üũ�� ����.
        // Ǭ �ܿ������� üũ�ϰ� ¥��� ��.
        // 
        // �ϴ� ������ ��Ǭ �ܿ� ���� -1�� üũ���� 
        // StudentQuizInfo.Unit_1.CorrectAnswer.Count �̰ſ� StudentQuizInfo.Unit_5.IncorrectAnswer.Count �̰� 0�̶�� -1�� üũ �ƴϸ� ��հ��� ����.

        float[] evaluation = new float[5];

        evaluation[0] =
            (StudentQuizInfo.Unit_1.CorrectAnswer.Count == 0 && StudentQuizInfo.Unit_1.IncorrectAnswer.Count == 0) ? -1 : average(StudentQuizInfo.Unit_1);
        evaluation[1] =                                                                                            
            (StudentQuizInfo.Unit_2.CorrectAnswer.Count == 0 && StudentQuizInfo.Unit_2.IncorrectAnswer.Count == 0) ? -1 : average(StudentQuizInfo.Unit_2);
        evaluation[2] =                                                                                            
            (StudentQuizInfo.Unit_3.CorrectAnswer.Count == 0 && StudentQuizInfo.Unit_3.IncorrectAnswer.Count == 0) ? -1 : average(StudentQuizInfo.Unit_3);
        evaluation[3] =                                                                                            
            (StudentQuizInfo.Unit_4.CorrectAnswer.Count == 0 && StudentQuizInfo.Unit_4.IncorrectAnswer.Count == 0) ? -1 : average(StudentQuizInfo.Unit_4);
        evaluation[4] =                                                                                            
            (StudentQuizInfo.Unit_5.CorrectAnswer.Count == 0 && StudentQuizInfo.Unit_5.IncorrectAnswer.Count == 0) ? -1 : average(StudentQuizInfo.Unit_5);

        float evaluation_ = 999;
        string result="";
        for (int i = 0; i < evaluation.Length; i++)
        {
            if (evaluation[i] == -1) continue;
            if (evaluation[i] < evaluation_)
            {                
                evaluation_ = evaluation[i];
                result = (i+1).ToString()+"�ܿ�";
            }
        }

        // ���� ����� ���� �ܿ� ����
        QuizDB.lowestUnit.text = result;
        Debug.Log(result);

        // ������ ���� QuizDB �� ������Ʈ�� quizinfo�� ���� ������ �Ѱ���.
        QuizDB.studentQuizinfo = StudentQuizInfo;

        yield return new WaitForSeconds(1f);
        // ����ϰ� �����ϰ� �� ������Ʈ�� �ִ� �г� ����.
        //StudentDB.instance.ShowPersonalDB();

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
            StudentDB.instance.OffStudentDB();

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
