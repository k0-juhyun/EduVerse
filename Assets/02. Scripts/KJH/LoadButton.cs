using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class ButtonPositionData
{
    public List<ButtonPosition> buttonPositions = new List<ButtonPosition>();
}

[System.Serializable]
public class ButtonPosition
{
    public string buttonName;
    public float posX, posY;
    public Item item;
}

[System.Serializable]
public class ButtonSessions
{
    public List<ButtonPositionData> sessions = new List<ButtonPositionData>();
}


public class LoadButton : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject teachingData;
    public Canvas canvas;
    private string filePath;
    public Dropdown sessionDropdown; // ������ �����ϴ� ��Ӵٿ�
    private ButtonSessions allSessions;

    public void OnClickCreateButton(Button clickedButton)
    {
        //// ���� ��ư�� �̸��� "Button"�� �ƴ϶�� �� ��ư ����
        //if (clickedButton.name == "Create")
        //{
        //    GameObject newButton = Instantiate(buttonPrefab, teachingData.transform);
        //    newButton.AddComponent<DraggableButton>();

        //    // �� ��ư�� �̸� ���� (��: "NewButton_1", "NewButton_2", ...)
        //    newButton.name = "NewButton_" + newButton.GetInstanceID();
        //}

        GameObject newButton = Instantiate(buttonPrefab, teachingData.transform);

        // �� ��ư�� �̸� ���� (��: "NewButton_1", "NewButton_2", ...)
        newButton.name = "NewButton_" + newButton.GetInstanceID();
    }

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "buttonSessions.json");
        LoadAllSessions();
        UpdateSessionDropdown();
    }

    public void SaveCurrentSession()
    {
        ButtonPositionData currentSession = new ButtonPositionData();

        foreach (Transform child in teachingData.transform)
        {
            if (child.GetComponent<Button>() != null)
            {
                RectTransform rectTransform = child.GetComponent<RectTransform>();
                InteractionBtn interactionBtn = child.GetComponent<InteractionBtn>();
                currentSession.buttonPositions.Add(new ButtonPosition
                {
                    buttonName = child.name,
                    posX = rectTransform.anchoredPosition.x,
                    posY = rectTransform.anchoredPosition.y,

                    // ������ ���� ����
                    item = interactionBtn.Item
                });
            }
        }

        allSessions.sessions.Add(currentSession);
        string json = JsonUtility.ToJson(allSessions);
        File.WriteAllText(filePath, json);
        UpdateSessionDropdown();
    }

    public void LoadSelectedSession()
    {
        int selectedSessionIndex = sessionDropdown.value;
        if (selectedSessionIndex < allSessions.sessions.Count)
        {
            ButtonPositionData selectedSession = allSessions.sessions[selectedSessionIndex];

            foreach (ButtonPosition buttonPosition in selectedSession.buttonPositions)
            {
                GameObject newButton = Instantiate(buttonPrefab, teachingData.transform);
                newButton.name = buttonPosition.buttonName;
                RectTransform rectTransform = newButton.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(buttonPosition.posX, buttonPosition.posY);
                //newButton.AddComponent<DraggableButton>();

                // ������ ���� �����ͼ� �ֱ�
                newButton.GetComponent<InteractionBtn>().Item = buttonPosition.item;
            }
        }
    }

    private void LoadAllSessions()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            allSessions = JsonUtility.FromJson<ButtonSessions>(json);
        }
        else
        {
            allSessions = new ButtonSessions();
        }
    }

    private void UpdateSessionDropdown()
    {
        // ���� ���õ� �ε����� ����
        int selectedIndex = sessionDropdown.value;

        sessionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < allSessions.sessions.Count; i++)
        {
            options.Add("Session " + (i + 1));
        }
        sessionDropdown.AddOptions(options);

        // ��Ӵٿ� �ɼ��� ������Ʈ �� �� ������ ���õ� �ε����� ����
        // ����� ������ ���� ������� �ʾҴٸ� ���� �ε����� �ٽ� ����
        if (selectedIndex < allSessions.sessions.Count)
        {
            sessionDropdown.value = selectedIndex;
        }
        else
        {
            // �� ������ �߰��� ���, ���� �ֱ� ������ ����
            sessionDropdown.value = allSessions.sessions.Count - 1;
        }

        // ��Ӵٿ��� ������ ������Ʈ
        sessionDropdown.RefreshShownValue();
    }

    public void DeleteSelectedSession()
    {
        int selectedSessionIndex = sessionDropdown.value;

        // ���õ� ������ ����Ʈ���� ����
        if (selectedSessionIndex >= 0 && selectedSessionIndex < allSessions.sessions.Count)
        {
            allSessions.sessions.RemoveAt(selectedSessionIndex);
        }

        // ����� ���� �����͸� ����
        SaveAllSessions();

        // ��Ӵٿ� �޴� ������Ʈ
        UpdateSessionDropdown();
    }

    private void SaveAllSessions()
    {
        string json = JsonUtility.ToJson(allSessions);
        File.WriteAllText(filePath, json);
    }
}
