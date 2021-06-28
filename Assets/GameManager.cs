using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class GameManager : MonoBehaviour{
    public GameObject bomb;
    void Start(){
        //Debug.Log("Start game manager");
    }

    void Update(){
        //Debug.Log(pos.position.x + " " + pos.position.z);

        if(Input.GetKeyDown("space")){
            //Debug.Log("Space pressed");
            Transform posPlayer = GameObject.Find("Player(Clone)").transform;
            float posX = (float)Math.Round(posPlayer.position.x);
            float posZ = (float)Math.Round(posPlayer.position.z);

            GameObject[] piedras = GameObject.FindGameObjectsWithTag("Piedra");
            bool isRock = false;

            for(int x = 0; x < piedras.Length && !isRock; x++){
                if(piedras[x].transform.position.x == posX && piedras[x].transform.position.z == posZ){
                    isRock = true;
                }
            }

            if(!isRock){
                GameObject clone = (GameObject) Instantiate(bomb, new Vector3 (posX, 1, posZ), Quaternion.identity);
                Destroy(clone, 1.5f);
            }
        }
        if(Input.GetKeyDown("p")){
            Debug.Log("P pressed");            
        }
    }
}
