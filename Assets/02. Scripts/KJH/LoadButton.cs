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
    public Dropdown sessionDropdown; // 세션을 선택하는 드롭다운
    private ButtonSessions allSessions;

    public void OnClickCreateButton(Button clickedButton)
    {
        //// 누른 버튼의 이름이 "Button"이 아니라면 새 버튼 생성
        //if (clickedButton.name == "Create")
        //{
        //    GameObject newButton = Instantiate(buttonPrefab, teachingData.transform);
        //    newButton.AddComponent<DraggableButton>();

        //    // 새 버튼에 이름 설정 (예: "NewButton_1", "NewButton_2", ...)
        //    newButton.name = "NewButton_" + newButton.GetInstanceID();
        //}

        GameObject newButton = Instantiate(buttonPrefab, teachingData.transform);

        // 새 버튼에 이름 설정 (예: "NewButton_1", "NewButton_2", ...)
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

                    // 아이템 정보 저장
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

                // 아이템 정보 가져와서 넣기
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
        // 현재 선택된 인덱스를 저장
        int selectedIndex = sessionDropdown.value;

        sessionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < allSessions.sessions.Count; i++)
        {
            options.Add("Session " + (i + 1));
        }
        sessionDropdown.AddOptions(options);

        // 드롭다운 옵션을 업데이트 한 후 이전에 선택된 인덱스를 복원
        // 저장된 세션의 수가 변경되지 않았다면 이전 인덱스를 다시 설정
        if (selectedIndex < allSessions.sessions.Count)
        {
            sessionDropdown.value = selectedIndex;
        }
        else
        {
            // 새 세션을 추가한 경우, 가장 최근 세션을 선택
            sessionDropdown.value = allSessions.sessions.Count - 1;
        }

        // 드롭다운을 강제로 업데이트
        sessionDropdown.RefreshShownValue();
    }

    public void DeleteSelectedSession()
    {
        int selectedSessionIndex = sessionDropdown.value;

        // 선택된 세션을 리스트에서 제거
        if (selectedSessionIndex >= 0 && selectedSessionIndex < allSessions.sessions.Count)
        {
            allSessions.sessions.RemoveAt(selectedSessionIndex);
        }

        // 변경된 세션 데이터를 저장
        SaveAllSessions();

        // 드롭다운 메뉴 업데이트
        UpdateSessionDropdown();
    }

    private void SaveAllSessions()
    {
        string json = JsonUtility.ToJson(allSessions);
        File.WriteAllText(filePath, json);
    }
}
