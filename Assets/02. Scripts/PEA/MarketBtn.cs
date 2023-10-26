using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketBtn : MonoBehaviour
{
    public GameObject marketCanvas;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowMarket);
    }

    void Update()
    {
        
    }

    private void ShowMarket()
    {
        marketCanvas.SetActive(true);
    }
}
