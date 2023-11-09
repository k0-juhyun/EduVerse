using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Quiz_
{
    public string question;
    public string answer;
}
public class MyQuizStorage : MonoBehaviour
{
    public static MyQuizStorage Instance;
    
    public List<Quiz_> quizList;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

}
