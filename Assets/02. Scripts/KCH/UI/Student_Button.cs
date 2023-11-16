using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student_Button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 학생 UID를 줘야함.
        GetComponent<Button>().onClick.AddListener(() => QuizToFireBase.instance.GetQuizData("27KHHFa2SWcs9Yo5L4A8zKOEls52"));

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
