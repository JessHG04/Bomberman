using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour{
    private Text text;

    private GameManager gameManager;

    void Start() {
        text = GetComponent<Text>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update() {
        if(text.name == "Time"){
            text.text = "TIME: " + (((int)gameManager.gameCountdown).ToString());
        }else if(text.name == "Score"){
            text.text = "SCORE: " + gameManager.score.ToString();
        }else if(text.name == "Distance"){
            text.text = gameManager.bombRadius.ToString();
        }
    }
}
