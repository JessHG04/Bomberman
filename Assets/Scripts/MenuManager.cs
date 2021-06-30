using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour{
    public void play(){
        //Debug.Log("Play");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
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
    }

    public void backToMenu(){
        //Debug.Log("Back");
        Time.timeScale = 1;
        SceneManager.LoadScene("InitialScene", LoadSceneMode.Single);
    }
}
