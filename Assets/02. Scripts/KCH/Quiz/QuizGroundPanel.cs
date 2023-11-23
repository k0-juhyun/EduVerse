using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizGroundPanel : MonoBehaviour
{

    public TextMeshProUGUI questionText;

    bool correctCheck;

    public string Question;
    public string Answer;
    public string Unit;
    public string Commentary;


    private void Update()
    {


    }

    // 버튼을 눌렀을 때 correctCheck 변수를 사용해서 정답 오답 체크한다.

    public void quizdata(string question_, string answer_,string unit_, string commentary_)
    {
        // 선생이 퀴즈 패널을 띄워주고

        questionText.text = question_;

        Question = question_;
        Answer = answer_;
        Unit = unit_;
        Commentary = commentary_;

        // 정답인지 오답인지 체크
        if (answer_ == "O") correctCheck = true;
        else correctCheck = false;

        // 학생은 문제를 풀었는지 오답인지 정답인지의 대한 데이터를 준다.

        // 여기선 아직 못풀었다는 정보를 줘야함.
        // 이름과 같이 줘야함.
        string sendName = DataBase.instance.user.name;

        // 못 푼 인원에 추가.
        MyQuizStorage.Instance.sendUserQuizData();

        // 광장에서의 퀴즈 시작
        Quiz.instance.startquiz();

        // 문제와 정답 quiz 스크립트에 담는다
        Quiz.instance.unit = unit_;
        Quiz.instance.question = question_;
        Quiz.instance.answer = answer_;
        Quiz.instance.commentary = commentary_;
        // Quiz 스크립트에 해설부분 다 가져감.
        StartCoroutine(quizPaneldelete());
    }




    // 시간이 다 끝나면 패널에 정답 띄워줌.
    // 아니다 개인적으로 띄워주는걸로 해야겠다 그게 더 편하겠네.

    IEnumerator quizPaneldelete()
    {
        yield return new WaitForSeconds(13);
        Destroy(gameObject);

    }
}
