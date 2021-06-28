using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour{
    
    public void jugar(){
        //Debug.Log("Jugar");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void cerrar()
    {
        //Debug.Log("Cerrar");
        Application.Quit();        
    }
}
