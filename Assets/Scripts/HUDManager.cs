using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour{
    Text time;
    float timerHUD = 60.0f;

    void Start() {
        time = GetComponent<Text>();
        timerHUD = 60.0f;
    }

    void Update() {
        int timerAux = (int)timerHUD;
        time.text = "TIME: " + timerAux.ToString();
        
        timerHUD -= Time.deltaTime;
    }
}
