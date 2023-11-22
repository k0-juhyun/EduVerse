using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuizMarketManager : MonoBehaviour
{
    public GameObject Units;
    GameObject Unit_quiz;

    public GameObject QuizPrefab;
    // L Ű�� ������ �ܿ����� Prefab�� ���� ����.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {

        }
    }
    
    // ���� ������ ������
    public virtual void unitOnBtnClick(string unit)
    {
        Transform parentTransform = Units.transform; // �θ� ������Ʈ�� Transform�� �����ɴϴ�.

        // ���� ��ϵ� �ܿ� off
        if(Unit_quiz != null)
        {
            Unit_quiz.SetActive(false);
        }

        // �θ� ������Ʈ�� ��� �ڽĵ��� ��ȸ
        foreach (Transform child in parentTransform)
        {
            if (child.name == unit)
            {
                // Ư�� �̸��� ���� �ڽ� ������Ʈ
                child.gameObject.SetActive(true);
                Unit_quiz = child.gameObject;
            }
        }

        // �׸��� �ܿ��� ������ content �������ش�.
        // Unit �ܿ�

        // viewport �ؿ� ������Ʈ�� ������ �ε� ���ϴ°ɷ� ���� 
        if(Unit_quiz.transform.GetChild(0).GetChild(0).childCount==0)
        {

            List<string> titles = SaveSystem.GetTitlesFromJson("MyQuizTitleData.json");

            if (titles != null)
            {
                foreach (string title in titles)
                {
                    SaveData saveData = SaveSystem.Load(title);

                    // �տ� �ܿ� ���ڸ� ����
                    string extracted = title.Substring(0, 3);
                    string titleSlice = title.Substring(4);

                    Debug.Log(extracted+ " : "+ unit);
                    if(extracted ==unit)
                    {
                        Debug.Log("����");
                        GameObject quiz_obj = Instantiate(QuizPrefab);

                        quiz_obj.transform.parent = Unit_quiz.transform.GetChild(0).GetChild(0);

                        Debug.Log("���⿡ ���� "+Unit_quiz.transform.GetChild(0).GetChild(0));

                        Debug.Log("Commentary" + saveData.Commentary);
                        // �ȿ� �̸��� ����.
                        quiz_obj.GetComponent<LoadQuizPrefab>().Question_Answer(saveData.question, saveData.answer, unit, saveData.Commentary);
                        // �̸� �ٲ�� ��.
                    }

                    // �տ� �ܿ� ����
                    //question.text = title.Substring(4);
                    string a = saveData.question.Substring(4);

                    // �������� �������� üũ�ϰ� �� ������Ʈ üũ.
                }
            }

        }

    }
}
