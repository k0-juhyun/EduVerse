using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student_QuizData : MonoBehaviour
{
    // 순서 
    // 1 student db에서 UID 버튼 등록함수 실행
    // 2. 그 버튼 눌렀을 때 QuizToBase에 있는 GetQuizData 함수 실행. (UID, obj) 전송
    // 3. 오브젝트를 전송해 그 오브젝트에 있는 studentQuizInfo 값을 바꿔줌.
    // 4. 그 데이터를 사용해 씬 업데이트.


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

        // 학생 UID를 줘야함.

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

    // 이 함수를 실행시켜줘야지.
    public void test()
    {

        Debug.Log(StudentQuizInfo.QuizCorrectAnswerCnt);
    }

    // UID 받아 버튼 등록.
    public void StudentUIDSaveBtnClick(string uid)
    {
        // 버튼을 눌렀을떄 사용자 UID와 오브젝트를 전달한다.
        GetComponent<Button>().onClick.AddListener(() => QuizToFireBase.instance.GetQuizData(uid, MygameObject));
    }
}
