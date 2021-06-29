using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour{
    Text time;
    float timerHUD = 60.0f;

    void Start() {
        time = GetComponent<Text>();
    }

    void Update() {
        time.text = "TIME: " + timerHUD.ToString();
        //timerHUD -= Time.deltaTime;
    }
    public void play(){
        //Debug.Log("Play");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        timerHUD = 60.0f;
    }

    public void quit(){
        //Debug.Log("Quit");
        Application.Quit();        
    }

    public void aud(){
        Debug.Log("Audio");
    }

    public void restart(){
        //Debug.Log("Restart");
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        timerHUD = 60.0f;
    }

    public void backToMenu(){
        //Debug.Log("Back");
        Time.timeScale = 1;
        SceneManager.LoadScene("InitialScene", LoadSceneMode.Single);
    }
}
