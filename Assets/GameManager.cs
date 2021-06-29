using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class GameManager : MonoBehaviour{
    public GameObject bomb;
    public Canvas pause;
    float posX = 0;
    float posZ = 0;

    void Start() {
        pause.gameObject.SetActive(false);
    }

    void Update(){
        if(Input.GetKeyDown("space")){
            Transform posPlayer = GameObject.Find("Player(Clone)").transform;
            posX = (float)Math.Round(posPlayer.position.x);
            posZ = (float)Math.Round(posPlayer.position.z);
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
                Invoke("explode", 1.5f);
                Destroy(clone, 1.5f);
            }
        }

        if(Input.GetKeyDown("p")){
            Pause();
        }
    }

    void explode(){
        float posZ1 = posZ + 1;
        float posZ2 = posZ - 1;
        float posX1 = posX + 1;
        float posX2 = posX - 1;
        bool up    = true;
        bool down  = true;
        bool left  = true;
        bool right = true;
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");

        if(rocks != null){
            for(int x = 0; x < rocks.Length; x++){
                if(rocks[x].transform.position.x == posX && rocks[x].transform.position.z == posZ1){
                    up = false;
                }
                if(rocks[x].transform.position.x == posX && rocks[x].transform.position.z == posZ2){
                    down = false;
                }
                if(rocks[x].transform.position.x == posX1 && rocks[x].transform.position.z == posZ){
                    right = false;
                }
                if(rocks[x].transform.position.x == posX2 && rocks[x].transform.position.z == posZ){
                    left = false;
                }
            }
        }
        
        if(posZ1 ==  7)    up = false;
        if(posZ2 == -7)  down = false;
        if(posX1 ==  7) right = false;
        if(posX2 == -7)  left = false;

        if(up){
            GameObject exp = (GameObject) Instantiate(bomb, new Vector3(posX, 1, posZ1), new Quaternion(0, 180, 0, 1));
            Destroy(exp, 0.5f);
        }
        if(down){
            GameObject exp = (GameObject) Instantiate(bomb, new Vector3(posX, 1, posZ2), new Quaternion(0, 180, 0, 1));
            Destroy(exp, 0.5f);
        }
        if(left){
            GameObject exp = (GameObject) Instantiate(bomb, new Vector3(posX2, 1, posZ), new Quaternion(0, 180, 0, 1));
            Destroy(exp, 0.5f);
        }
        if(right){
            GameObject exp = (GameObject) Instantiate(bomb, new Vector3(posX1, 1, posZ), new Quaternion(0, 180, 0, 1));
            Destroy(exp, 0.5f);
        }

        GameObject exp2 = (GameObject) Instantiate(bomb, new Vector3(posX, 1, posZ), new Quaternion(0, 180, 0, 1));
        Destroy(exp2, 0.5f);
    }

    void Pause(){
        //Debug.Log("Pause");
        if(Time.timeScale == 1){
            Time.timeScale = 0;
            pause.gameObject.SetActive(true);
        }else if(Time.timeScale == 0){
            Time.timeScale = 1;
            pause.gameObject.SetActive(false);
        }
    }
}