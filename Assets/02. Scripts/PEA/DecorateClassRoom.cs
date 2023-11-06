using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorateClassRoom : MonoBehaviour
{
    public static DecorateClassRoom instance = null;

    private List<Texture2D> myDraws = new List<Texture2D>();

    private Texture2D curSelectedDraw;

    public Transform myDrawsContent;
    public Transform boardPanel;
    public GameObject myDrawItem;
    public GameObject decoItem;
    public Button uploadBtn;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadMyDraws();

        uploadBtn.onClick.AddListener(UploadMyDraw);
    }

    void Update()
    {
        
    }

    private void LoadMyDraws()
    {
        if(Directory.Exists(Application.persistentDataPath + "/MyDraws/"))
        {
            string[] paths = Directory.GetFiles(Application.persistentDataPath + "/MyDraws/");
            foreach (string  path in paths)
            {
                byte[] bytes = File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                texture.Apply();
                myDraws.Add(texture);

                ShowMyDraws(texture);
            }
        }
    }

    private void ShowMyDraws(Texture2D texture)
    {
        Instantiate(myDrawItem, myDrawsContent).transform.GetChild(0).GetComponent<RawImage>().texture = texture;
    }

    public void AddMyDraw(Texture2D texture)
    {
        myDraws.Add(texture);
        ShowMyDraws(texture);
    }

    public void UploadMyDraw()
    {
        if (curSelectedDraw == null)
            return;

        if(boardPanel.childCount < 15)
        {
            GameObject drawItem = Instantiate(decoItem, boardPanel);
            drawItem.transform.GetChild(0).GetComponent<RawImage>().texture = curSelectedDraw;
        }

        curSelectedDraw = null;
    }

    public void SelectDraw(Texture2D texture)
    {
        curSelectedDraw = texture;
    }
}
