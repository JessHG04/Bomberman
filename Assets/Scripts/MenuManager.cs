using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour{
    public Canvas initial;
    public Canvas controls;
    public Button button;

    void Start(){
        controls.gameObject.SetActive(false);
        button = GameObject.FindGameObjectWithTag("Bomb").GetComponent<Button>();
    }
    public void play(){
        //Debug.Log("Play");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void contr(){
        initial.gameObject.SetActive(false);
        controls.gameObject.SetActive(true);
    }

    public void back(){
        controls.gameObject.SetActive(false);
        initial.gameObject.SetActive(true);
    }

    public void quit(){
        //Debug.Log("Quit");
        Application.Quit();        
    }
}
