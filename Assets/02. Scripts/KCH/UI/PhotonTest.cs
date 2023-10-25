using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonTest : MonoBehaviour
{
    static public PhotonTest instance;
    public Text photon_testtext;
    private void Awake()
    {
        instance= this;
        DontDestroyOnLoad(gameObject);
    }



}
