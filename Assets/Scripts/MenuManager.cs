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
}
