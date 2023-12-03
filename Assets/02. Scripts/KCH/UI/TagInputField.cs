using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TagInputField : MonoBehaviour
{
    public GameObject TagText;
    public TMP_InputField playerNameInput;
    public Text tagText;

    private void Start()
    {
        playerNameInput.onSubmit.AddListener((s) => EnterTxtBtn(s));
    }

    public void SendBtn()
    {
        // 부모 오브젝트
        Transform parentTransform = GetComponent<Transform>();

        // 부모 객체에 있는 모든 자식객체 가져옴

        foreach (Transform childTransform in parentTransform)
        {
            // 각 자식 객체의 이름을 출력합니다.
            if (childTransform.gameObject.name == "TagText(Clone)")
            {
                Destroy(childTransform.gameObject);
            }
        }
    }

    public void EnterTxtBtn(string s)
    {
        // 부모 오브젝트를 찾습니다.
        Transform parentTransform = transform; // 예시로 현재 스크립트가 붙은 오브젝트를 부모로 설정합니다.

        // 새로운 자식 오브젝트를 생성합니다.
        GameObject newChild = Instantiate(TagText);
        // 생성된 자식 오브젝트를 부모 오브젝트의 첫 번째 자식으로 설정합니다.
        newChild.transform.parent = parentTransform;
        // 스케일 변화로 인한 스케일 값 0으로 설정
        newChild.transform.localScale = Vector3.one;

        // newchild의 chat 바꿔줘야함.
        newChild.GetComponentInChildren<Text>().text = playerNameInput.text;

        // 만약 이 자식이 첫 번째 자식이어야 한다면,
        // 다른 형제들을 뒤로 밀어내야 합니다.

        // inputField 가장 뒤로 밀기.
        playerNameInput.transform.SetAsLastSibling();

        playerNameInput.text = string.Empty;

        // 자식 카운트로 tag 변경
        switch (parentTransform.childCount)
        {
            case 1:
                tagText.text = "문제";
                break;
            case 2:
                if(!transform.parent.GetComponent<Toggle_kch>().multimedio_.isOn)
                tagText.text = "과목";
                break;
            case 3:
                tagText.text = "단원";
                break;
            case 4:
                tagText.text = "제목";
                break;
        }


    }
    public void BackBtn()
    {
        // 부모 오브젝트
        Transform parentTransform = GetComponent<Transform>();

        // 부모 객체에 있는 모든 자식객체 가져옴


        foreach (Transform childTransform in parentTransform)
        {
            // 각 자식 객체의 이름을 출력합니다.
            if (childTransform.gameObject.name == "TagText(Clone)")
            {
                Destroy(childTransform.gameObject);
            }
        }

    }

    public void tagchage()
    {
        Transform parentTransform = transform;

        Debug.Log(parentTransform.childCount);

        // 이걸 먼저 실행하고 오브젝트를 지우기 때문에 +1을 해준다.
        switch (parentTransform.childCount)
        {
            case 2:
                if (!transform.parent.GetComponent<Toggle_kch>().multimedio_.isOn)
                    tagText.text = "문제";
                break;
            case 3:
                tagText.text = "과목";
                break;
            case 4:
                tagText.text = "단원";
                break;
            case 5:
                tagText.text = "제목";
                break;
        }

    }
}
