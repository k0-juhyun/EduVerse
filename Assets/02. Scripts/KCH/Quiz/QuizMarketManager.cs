using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuizMarketManager : MonoBehaviour
{
    public GameObject Units;
    GameObject Unit_quiz;

    public GameObject QuizPrefab;

    private GameObject lastClickedButton;

    public Button[] sectionButtons;

    private void Start()
    {
        foreach (var button in sectionButtons) 
        {
            button.onClick.AddListener(() => ToggleButtonColor(button.gameObject));
        }
    }

    // 퀴즈 데이터 가져옴
    public virtual void unitOnBtnClick(string unit)
    {
        Transform parentTransform = Units.transform; // 부모 오브젝트의 Transform을 가져옵니다.

        // 전에 등록된 단원 off
        if(Unit_quiz != null)
        {
            Unit_quiz.SetActive(false);
        }

        // 부모 오브젝트의 모든 자식들을 순회
        foreach (Transform child in parentTransform)
        {
            if (child.name == unit)
            {
                // 특정 이름을 가진 자식 오브젝트
                child.gameObject.SetActive(true);
                Unit_quiz = child.gameObject;
            }
        }

        // 그리고 단원별 문제들 content 생성해준다.
        // Unit 단원

        // viewport 밑에 오브젝트가 있으면 로드 안하는걸로 하자 
        if(Unit_quiz.transform.GetChild(0).GetChild(0).childCount==0)
        {

            List<string> titles = SaveSystem.GetTitlesFromJson("MyQuizTitleData.json");

            if (titles != null)
            {
                foreach (string title in titles)
                {
                    SaveData saveData = SaveSystem.Load(title);

                    // 앞에 단원 글자만 추출
                    string extracted = title.Substring(0, 3);
                    string titleSlice = title.Substring(4);

                    Debug.Log(extracted+ " : "+ unit);
                    if(extracted ==unit)
                    {
                        Debug.Log("실행");
                        GameObject quiz_obj = Instantiate(QuizPrefab);

                        quiz_obj.transform.parent = Unit_quiz.transform.GetChild(0).GetChild(0);

                        Debug.Log("여기에 문제 "+Unit_quiz.transform.GetChild(0).GetChild(0));

                        Debug.Log("Commentary" + saveData.Commentary);
                        // 안에 이름을 넣음.
                        quiz_obj.GetComponent<LoadQuizPrefab>().Question_Answer(saveData.question, saveData.answer, unit, saveData.Commentary);
                        // 이름 바꿔야 함.
                    }

                    // 앞에 단원 삭제
                    //question.text = title.Substring(4);
                    string a = saveData.question.Substring(4);

                    // 정답인지 오답인지 체크하고 그 오브젝트 체크.
                }
            }

        }

    }

    private void ToggleButtonColor(GameObject clickedButton)
    {
        // 현재 클릭된 버튼
        Image currentColor = clickedButton.GetComponent<Image>();
        Color originColor = currentColor.color;

        if (lastClickedButton != null)
        {
            // 이전에 클릭된 버튼 색상변경
            Image lastOutline = lastClickedButton.GetComponent<Image>();
            if (lastOutline != null)
            {
                lastOutline.color = originColor;
            }
        }

        if (currentColor != null)
        {
            // 사용자 지정 색상 설정
            currentColor.color = new Color(245f / 255f, 189f / 255f, 27f / 255f);
            lastClickedButton = clickedButton;
        }
    }
}
