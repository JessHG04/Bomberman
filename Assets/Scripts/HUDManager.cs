using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour{
    private Text text;

    void Start() {
        text = GetComponent<Text>();
    }

    void Update() {
        if(text.name == "Time"){
            text.text = "TIME: " + ((int)GameObject.Find("GameManager").GetComponent<GameManager>().gameCountdown).ToString();
        }else if(text.name == "Score"){
            text.text = "SCORE: " + GameObject.Find("GameManager").GetComponent<GameManager>().score.ToString();
        }else if(text.name == "Distance"){
            text.text = GameObject.Find("GameManager").GetComponent<GameManager>().bombRadius.ToString();
        }
    }
}
