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

    public void EnterTxtBtn(string s)
    {
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

        // �ڽ� ī��Ʈ�� tag ����
        switch (parentTransform.childCount)
        {
            case 1:
                tagText.text = "����";
                break;
            case 2:
                if(!transform.parent.GetComponent<Toggle_kch>().multimedio_.isOn)
                tagText.text = "����";
                break;
            case 3:
                tagText.text = "�ܿ�";
                break;
            case 4:
                tagText.text = "����";
                break;
        }


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

    public void tagchage()
    {
        Transform parentTransform = transform;

        Debug.Log(parentTransform.childCount);

        // �̰� ���� �����ϰ� ������Ʈ�� ����� ������ +1�� ���ش�.
        switch (parentTransform.childCount)
        {
            case 2:
                if (!transform.parent.GetComponent<Toggle_kch>().multimedio_.isOn)
                    tagText.text = "����";
                break;
            case 3:
                tagText.text = "����";
                break;
            case 4:
                tagText.text = "�ܿ�";
                break;
            case 5:
                tagText.text = "����";
                break;
        }

    }
}
