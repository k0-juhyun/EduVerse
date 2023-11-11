using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizGroundPanel : MonoBehaviour
{

    public TextMeshProUGUI questionText;

    bool correctCheck;

    private void Update()
    {


    }

    // 버튼을 눌렀을 때 correctCheck 변수를 사용해서 정답 오답 체크한다.

    public void quizdata(string question_, string answer_)
    {
        // 선생이 퀴즈 패널을 띄워주고

        questionText.text = question_;

        // 정답인지 오답인지 체크
        if (answer_ == "O") correctCheck = true;
        else correctCheck = false;

        // 학생은 문제를 풀었는지 오답인지 정답인지의 대한 데이터를 준다.

        // 여기선 아직 못풀었다는 정보를 줘야함.
        // 이름과 같이 줘야함.
        string sendName = DataBase.instance.userInfo.name;

        // 못 푼 인원에 추가.
        MyQuizStorage.Instance.sendUserQuizData();

        // Don't destoryobject되어있는 MyQuizStorage에 넣자.
        // 여기는 개별적으로 생성되어있고 포톤 뷰 선생한테는 이 오브젝트가 없기 때문에
        // 학생 json으로 DB 저장 일단 나중에 하는걸로 하고 일단 
        // 선생 패널에 푼 인원과 못푼 인원 나누는 걸로 하자.

        // 못푼 인원에 추가
        // 풀면 못푼 인원에 있는 데이터 지우고

    }
    IEnumerator quizPaneldelete()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
