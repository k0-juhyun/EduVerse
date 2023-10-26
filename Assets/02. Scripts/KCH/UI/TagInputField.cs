using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagInputField : MonoBehaviour
{
    public GameObject TagText;
    public InputField playerNameInput;

    public void Test()
    {
        Debug.Log("1");
    }
    public void Tes1t()
    {
        Debug.Log("12");

        // �θ� ������Ʈ�� ã���ϴ�.
        Transform parentTransform = transform; // ���÷� ���� ��ũ��Ʈ�� ���� ������Ʈ�� �θ�� �����մϴ�.

        // ���ο� �ڽ� ������Ʈ�� �����մϴ�.
        GameObject newChild = Instantiate(TagText);
        // ������ �ڽ� ������Ʈ�� �θ� ������Ʈ�� ù ��° �ڽ����� �����մϴ�.
        newChild.transform.parent = parentTransform;
        // ������ ��ȭ�� ���� ������ �� 0���� ����
        newChild.transform.localScale = Vector3.one;

        // newchild�� chat �ٲ������.
        newChild.GetComponentInChildren<Text>().text = playerNameInput.text;

        // ���� �� �ڽ��� ù ��° �ڽ��̾�� �Ѵٸ�,
        // �ٸ� �������� �ڷ� �о�� �մϴ�.
        for (int i = parentTransform.childCount - 1; i > 0; i--)
        {
            parentTransform.GetChild(i).SetSiblingIndex(i + 1);
        }

    }
    public void Test3()
    {
        Debug.Log("13");
    }
}
