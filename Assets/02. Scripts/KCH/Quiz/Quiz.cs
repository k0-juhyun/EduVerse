using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz : MonoBehaviour
{

    #region �̱���
    static public Quiz instance;
    private void Awake()
    {
        instance = this; 
    }
    #endregion

    // �Լ� ���� ��������Ʈ
    public event System.Action QuizEnded;

    public float setTime;
    float originTime;

    // 
    public bool isQuiz = false;

    private void Start()
    {
        originTime = setTime;
    }

    void Update()
    {
        if (isQuiz)
        {
            setTime -= Time.deltaTime;
            if (setTime <= 0)
            {
                isQuiz = false;
                // ���� ���� �̺�Ʈ
                QuizEnded?.Invoke();
            }
        }
        else
        {
            setTime = originTime;
        }

    }
}
