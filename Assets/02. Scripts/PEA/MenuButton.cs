using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour
{
    private Button[] buttonSets;

    private RaycastHit hit;
    private Button selectedBtn;

    public GameObject[] menus;

    public GameObject quizCanvas;

    private void OnEnable()
    {
        if(buttonSets != null)
        {
            buttonSets[0].Select();
            selectedBtn = buttonSets[0];
        }
    }

    private void Start()
    {
        buttonSets = new Button[transform.childCount];

        for (int i = 0; i < buttonSets.Length; i++)
        {
            buttonSets[i] = transform.GetChild(i).GetComponent<Button>();
        }

        buttonSets[0].Select();
        selectedBtn = buttonSets[0];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clickObj = EventSystem.current.currentSelectedGameObject;

            for (int i = 0; i < buttonSets.Length; i++)
            {
                if (buttonSets[i].gameObject == clickObj)
                {
                    selectedBtn = buttonSets[i];
                }
            }

            for (int i = 0; i < buttonSets.Length; i++)
            {
                if(menus[i] != null)
                {
                    menus[i].SetActive(buttonSets[i] == selectedBtn);
                }
            }
            selectedBtn.Select();
        }
    }

    public void OnQuizDataBtnClick()
    {
        quizCanvas.SetActive(true);
    }
}
