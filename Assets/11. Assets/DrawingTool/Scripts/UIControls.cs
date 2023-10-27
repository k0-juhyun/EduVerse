using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIControls : MonoBehaviour
{
    [SerializeField]
    GameObject colorToggle, undo, clear, scroll, palette, DrawingTool;
    private bool toggle;
    bool color_toggle = false;
    bool pale_toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        toggle = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowHideControls()
    {
        toggle = !toggle;
        undo.SetActive(toggle);
        clear.SetActive(toggle);
        scroll.SetActive(toggle);
        palette.SetActive(toggle);
    }

    public void Colortoggle()
    {
        if (!color_toggle)
        {
            colorToggle.GetComponent<RectTransform>().DOAnchorPosX(-480, 0.5f);
            color_toggle = !color_toggle;
        }
        else
        {
            colorToggle.GetComponent<RectTransform>().DOAnchorPosX(-780, 0.5f);
            color_toggle = !color_toggle;
        }
    }
    public void PaletteQuittoggle()
    {

        colorToggle.GetComponent<RectTransform>().DOAnchorPosX(-780, 0.5f);
        color_toggle = !color_toggle;
        palette.GetComponent<RectTransform>().DOAnchorPosY(-365, 0.5f).OnComplete(() =>
        {
            DrawingTool.SetActive(false);
        });

    }
}
