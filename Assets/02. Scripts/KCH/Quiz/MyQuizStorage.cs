using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyQuizStorage : MonoBehaviour
{
    public static MyQuizStorage Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
