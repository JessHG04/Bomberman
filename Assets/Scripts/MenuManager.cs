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
    public void StartGame(){
        //Debug.Log("Play");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void OpenControlsPanel(){
        initial.gameObject.SetActive(false);
        controls.gameObject.SetActive(true);
    }

    public void BackFromControlPanel(){
        controls.gameObject.SetActive(false);
        initial.gameObject.SetActive(true);
    }

    public void QuitGame(){
        //Debug.Log("Quit");
        Application.Quit();        
    }
}
