using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class StudentDataItem : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text studentNumText;                                                                                                                                                                                                                                            
    public string uid;

    private void Start()
    {


        //GetComponent<Button>().onClick.AddListener(() => SoundManager.instance.PlaySFX(SoundManager.SFXClip.Button));
    }

    public void SetStdentData(string name, string studentNum)
    {
        nameText.text = name;
        studentNumText.text = studentNum.PadLeft(2,'0');
    }

    public void ShowMyData()
    {
        //StudentDB.instance.ShowPersonalDB();
    }
}
