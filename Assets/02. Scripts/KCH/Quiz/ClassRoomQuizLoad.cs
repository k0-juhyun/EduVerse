using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using static MyQuizStorage;
using DG.Tweening;

public class ClassRoomQuizLoad : MonoBehaviourPun
{


    public GameObject AddQuizViewport;
    public GameObject QuizPrefab;

    public TextMeshProUGUI questionText;

    [Space(20)]
    [Header("�ο�üũ")]
    public GameObject peopleCheckTextprefab;
    public GameObject �����ο�;
    public GameObject �����ο�;
    public GameObject ��Ǭ�ο�;

    [Space(20)]
    public GameObject ��������;
    public GameObject QuizIcon;


    // ������
    // ����
    public enum QuizState
    {
        None,
        Proceeding,
        End
    }

    [Space(20)]
    public QuizState quizState = QuizState.None;
    public GameObject QuizPanel_student;

    // ������ ��.
    string Question;
    string Answer;
    string Commentary;


    private void Start()
    {

        for (int i = 0; i < MyQuizStorage.Instance.quizList.Count; i++)
        {
            Debug.Log(MyQuizStorage.Instance.quizList[i].question);
            GameObject quiz_obj = Instantiate(QuizPrefab);
            quiz_obj.transform.parent = AddQuizViewport.transform;
            quiz_obj.transform.localScale = new Vector3(1, 1, 1);
            quiz_obj.GetComponent<LoadQuizPrefab>().Question_Answer(MyQuizStorage.Instance.quizList[i].question,
            MyQuizStorage.Instance.quizList[i].answer, MyQuizStorage.Instance.quizList[i].unit, MyQuizStorage.Instance.quizList[i].commentary);
        }
        // ���⼭ ������ ����Ʈ �̾Ƽ� �����.




        // �ο� üũ ��������Ʈ
        MyQuizStorage.Instance.correctAnswerCheck = QuizNotYetSolvePeopleCheck;
        MyQuizStorage.Instance.correctCheck = QuizSolvePeopleCheck_O;
        MyQuizStorage.Instance.incorrectCheck = QuizSolvePeopleCheck_X;

    }
    void Update()
    {
        // ���� �ٽ� ����
        if (Input.GetKeyDown(KeyCode.M))
        {

            transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBack);
        }
    }

    // ����Ǵ� �Լ� 
    public void OnExitQuizBtnClick()
    {      
        photonView.RPC(nameof(deletePanel), RpcTarget.All);
        // ���� ���� ��ư
        if(Quiz.instance !=null)
            Quiz.instance.EndQuiz();
    }

    [PunRPC]
    public void deletePanel()
    {
        ��������.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBack).OnComplete(() => {

            Debug.Log("�̰Ž�����?");

            Destroy(gameObject);
        });
    }

    // ���忡���� ���� ��ư
    public void OndisappearCanvasBtnClick()
    {
        // �ٽ� Ű�� ����°͵� ��������.
        ��������.transform.DOScale(0,0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            QuizIcon.transform.DOScale(1, 0.5f).SetEase(Ease.OutCubic);
        });
    }

    public void OnappearCanvasBtnClick()
    {
        Debug.Log("����");
        QuizIcon.transform.DOScale(0, 0.2f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            ��������.transform.DOScale(1, 0.5f).SetEase(Ease.OutCubic);
        });
    }


    // �ο� üũ ������
    public void QuizNotYetSolvePeopleCheck(string str)
    {
        // ���� ���� �ؽ�Ʈ �����

        // ��Ǭ �ο� üũ.
        GameObject checkobj = Instantiate(peopleCheckTextprefab);
        checkobj.transform.parent = ��Ǭ�ο�.transform;
        Debug.Log(str);

        checkobj.GetComponentInChildren<TextMeshProUGUI>().text = str;
    }

    // ���� �ο� üũ
    // üũ �� Firebase�� �����ؾ���.
    public void QuizSolvePeopleCheck_O(string str)
    {
        TextMeshProUGUI[] text = ��Ǭ�ο�.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI item in text)
        {
            if (item.text == str)
            {
                Destroy(item.gameObject);
                break;
            }
        }

        // ��Ǭ �ο� üũ.
        GameObject checkobj = Instantiate(peopleCheckTextprefab);
        checkobj.transform.parent = �����ο�.transform;

        checkobj.GetComponentInChildren<TextMeshProUGUI>().text = str;

        // üũ �� Firebase�� �����ؾ���.


    }
    public void QuizSolvePeopleCheck_X(string str)
    {

        TextMeshProUGUI[] text = ��Ǭ�ο�.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI item in text)
        {
            if (item.text == str)
            {
                Destroy(item.gameObject);
                break;
            }
        }

        // ��Ǭ �ο� üũ.
        GameObject checkobj = Instantiate(peopleCheckTextprefab);
        checkobj.transform.parent = �����ο�.transform;

        checkobj.GetComponentInChildren<TextMeshProUGUI>().text = str;

        // üũ �� Firebase�� �����ؾ���.

    }

    public void SelectQuiz(string question_, string answer_)
    {
        // ���⼭ ������ ������ ����
        questionText.text = question_;

        // ���� �� ���� ������ Ǯ���ٸ� �� �����ڿ� ������ ������ Json���� �Ǵ� firebase�� ����

        // ������ ������ Ǯ���ٸ� ������ ������ üũ ���� ����.

        // �� ������ �信 �´� �л��� DB�� ���� �ϴ� �Լ��� ��������.

        TextMeshProUGUI[] text = �����ο�.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI item in text)
        {
            Destroy(item.gameObject);
        }
        TextMeshProUGUI[] text_ = �����ο�.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI item in text_)
        {
            Destroy(item.gameObject);
        }

    }
    
}
