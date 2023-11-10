using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StudentDataItem : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text studentNumText;

    public void SetStdentData(string name, string studentNum)
    {
        nameText.text = name;
        studentNumText.text = studentNum.PadLeft(2,'0');
    }
}
