using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz : MonoBehaviour
{

    #region ΩÃ±€≈Ê
    static public Quiz instance;
    private void Awake()
    {
        instance = this; 
    }
    #endregion

    // «‘ºˆ ¥„¿ª µ®∏Æ∞‘¿Ã∆Æ
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
                // ƒ˚¡Ó ¡æ∑· ¿Ã∫•∆Æ
                QuizEnded?.Invoke();
            }
        }
        else
        {
            setTime = originTime;
        }

    }
}
