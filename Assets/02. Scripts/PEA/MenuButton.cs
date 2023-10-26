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

    private void Start()
    {
        buttonSets = new Button[transform.childCount];

        for (int i = 0; i < buttonSets.Length; i++)
        {
            buttonSets[i] = transform.GetChild(i).GetComponent<Button>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clickObj = EventSystem.current.currentSelectedGameObject;

            foreach (Button btn in buttonSets)
            {
                if (btn.gameObject == clickObj)
                {
                    selectedBtn = btn;
                    return;
                }
            }
            selectedBtn.Select();
        }
    }
}
