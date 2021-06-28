using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour{
    public GameObject bomb;
    Transform posPlayer;
    Vector3 pos;
    void Start(){
        //Debug.Log("Start game manager");
    }

    void Update(){
        //Debug.Log(pos.position.x + " " + pos.position.z);

        if(Input.GetKeyDown("space")){
            //Debug.Log("Space pressed");
            posPlayer = GameObject.Find("Player(Clone)").transform;
            pos = new Vector3 (posPlayer.position.x, posPlayer.position.y, posPlayer.position.z);
            Instantiate(bomb, pos, Quaternion.identity);
        }
        if(Input.GetKeyDown("p")){
            Debug.Log("P pressed");            
        }
    }
}
