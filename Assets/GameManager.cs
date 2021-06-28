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
            Transform posPlayer = GameObject.Find("Player(Clone)").transform;
            float posX = (float)Math.Round(posPlayer.position.x);
            float posZ = (float)Math.Round(posPlayer.position.z);
            GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");
            GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
            bool occuped = false;
            
            if(rocks != null){
                for(int x = 0; x < rocks.Length && !occuped; x++){
                    if(rocks[x].transform.position.x == posX && rocks[x].transform.position.z == posZ){
                        occuped = true;
                    }
                }
            }
            
            if(bombs != null){
                for(int x = 0; x < bombs.Length && !occuped; x++){
                    if(bombs[x].transform.position.x == posX && bombs[x].transform.position.z == posZ){
                        occuped = true;
                    }
                }
            }

            if(!occuped){
                GameObject clone = (GameObject) Instantiate(bomb, new Vector3(posX, 1, posZ), new Quaternion(0, 180, 0, 1));
                Destroy(clone, 1.5f);
            }
        }
        if(Input.GetKeyDown("p")){

        }
    }
}
