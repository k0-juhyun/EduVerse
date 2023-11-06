using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagInputField : MonoBehaviour
{
    public GameObject TagText;
    public InputField playerNameInput;

    public void SendBtn()
    {
        // 부모 오브젝트
        Transform parentTransform = GetComponent<Transform>();

        // 부모 객체에 있는 모든 자식객체 가져옴

        foreach (Transform childTransform in parentTransform)
        {
            // 각 자식 객체의 이름을 출력합니다.
            Debug.Log("자식 객체 이름: " + childTransform.gameObject.name);
            if(childTransform.gameObject.name == "TagText(Clone)")
            {
                Debug.Log("실행");
                Destroy(childTransform.gameObject);
            }
        }
    }
    public void EnterTxtBtn()
    {
        Debug.Log("12");

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
}
