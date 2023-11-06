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
        // �θ� ������Ʈ
        Transform parentTransform = GetComponent<Transform>();

        // �θ� ��ü�� �ִ� ��� �ڽİ�ü ������

        foreach (Transform childTransform in parentTransform)
        {
            // �� �ڽ� ��ü�� �̸��� ����մϴ�.
            Debug.Log("�ڽ� ��ü �̸�: " + childTransform.gameObject.name);
            if(childTransform.gameObject.name == "TagText(Clone)")
            {
                Debug.Log("����");
                Destroy(childTransform.gameObject);
            }
        }
    }
    public void EnterTxtBtn()
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
        
        // inputField ���� �ڷ� �б�.
        playerNameInput.transform.SetAsLastSibling();

        playerNameInput.text = string.Empty;

    }
    public void BackBtn()
    {
        // �θ� ������Ʈ
        Transform parentTransform = GetComponent<Transform>();

        // �θ� ��ü�� �ִ� ��� �ڽİ�ü ������

        foreach (Transform childTransform in parentTransform)
        {
            // �� �ڽ� ��ü�� �̸��� ����մϴ�.
            if (childTransform.gameObject.name == "TagText(Clone)")
            {
                Destroy(childTransform.gameObject);
            }
        }
    }
}
