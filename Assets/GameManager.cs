using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour{
    public GameObject bomb;
    public GameObject fire;
    public Canvas pause;
    public Canvas win;
    public Canvas lose;
    public float timer = 60.0f;
    public int distance = 1;
    bool dead = false;
    bool play = true;
    AudioSource[] audios;
    public int score = 0;
    List<GameObject> boxs = new List<GameObject>();
    List<GameObject> powerUps = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    bool random = false;
    void Start() {
        timer = 60.0f;
        pause.gameObject.SetActive(false);
        win.gameObject.SetActive(false);
        lose.gameObject.SetActive(false);
        audios = GetComponents<AudioSource>();
        audios[0].Play();
        score = 0;
        distance = 1;
        boxs = GameObject.FindGameObjectsWithTag("Box").ToList();
        powerUps = GameObject.FindGameObjectsWithTag("PowerUp").ToList();
        powerUps[0].SetActive(false);
        powerUps[1].SetActive(false);
        
    }

    void Update(){
        if(timer <= 0.0f){
            win.gameObject.SetActive(true);
            Invoke("goMenu", 0.5f);
        }else{
            timer -= Time.deltaTime;
            enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
            if(!dead && enemies.Count > 0){
                if(Input.GetKeyDown("space")){
                    Transform posPlayer = GameObject.FindGameObjectWithTag("Player").transform;
                    float bombX = (float)Math.Round(posPlayer.position.x);
                    float bombZ = (float)Math.Round(posPlayer.position.z);
                    GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
                    bool occuped = false;
                    //No pongo 2 bombas en el mismo sitio
                    if(bombs != null){
                        for(int x = 0; x < bombs.Length && !occuped; x++){
                            if(bombs[x].transform.position.x == bombX && bombs[x].transform.position.z == bombZ){
                                occuped = true;
                            }
                        }
                    }

                    if(!occuped){
                        GameObject clone = (GameObject) Instantiate(bomb, new Vector3(bombX, 1, bombZ), new Quaternion(0, 180, 0, 1));
                        StartCoroutine(explode(bombX, bombZ, false));
                        Destroy(clone, 1.5f);
                    }
                }
                UpdatePowerUps();
                if(!random){
                    RandomBomb();
                }
                if(Input.GetKeyDown("p")){
                    Pause();
                }
            }else{
                if(dead){
                    lose.gameObject.SetActive(true);
                }
                if(enemies.Count <= 0){
                    win.gameObject.SetActive(true);
                }
                Invoke("goMenu", 2.0f);
            }
        }
    }

    void goMenu(){
        SceneManager.LoadScene("InitialScene", LoadSceneMode.Single);
    }

    public void enemyBomb(float bombX, float bombZ){
        GameObject b = (GameObject) Instantiate(bomb, new Vector3(bombX, 1, bombZ), new Quaternion(0, 180, 0, 1));
        StartCoroutine(explode(bombX, bombZ, true));
        Destroy(b, 1.5f);
    }

    IEnumerator explode(float bombX, float bombZ, bool rand){
        yield return new WaitForSeconds(1.5f);

        boxs = GameObject.FindGameObjectsWithTag("Box").ToList();
        int aux = 1;
        bool up    = true;
        bool down  = true;
        bool right = true;
        bool left  = true;

        if(play){
            audios[1].Play();
        }

        while(aux != distance + 1){
            float bombZ1 = bombZ + aux;
            float bombZ2 = bombZ - aux;
            float bombX1 = bombX + aux;
            float bombX2 = bombX - aux;

            if(boxs != null){
                for(int x = 0; x < boxs.Count; x++){
                    //UP
                    if(up && boxs[x].transform.position.x == bombX && boxs[x].transform.position.z == bombZ1){ //Puedo mirar arriba y hay algo ahí
                        if(boxs[x].TryGetComponent(out Wall rock)){ //Si es roca dejo de mirar
                            up = false;
                        }else{ //Si no, es caja, la destruyo
                            Destroy(boxs[x]);
                            if(!rand) score += 100; //Si la bomba no es random, sumo puntos
                        }
                        
                    }
                    if(up){ //Si arriba no hay roca, creo la explosion y la compruebo
                        GameObject exp = (GameObject) Instantiate(fire, new Vector3(bombX, 1.5f, bombZ1), new Quaternion(0, 180, 0, 1));
                        checkExplosion(bombX, bombZ1);
                        if(!rand) checkExplosionEnemy(bombX, bombZ1);
                        Destroy(exp, 0.5f);
                    }
                    //DOWN
                    if(down && boxs[x].transform.position.x == bombX && boxs[x].transform.position.z == bombZ2){
                        if(boxs[x].TryGetComponent(out Wall rock)){
                            down = false;
                        }else{
                            Destroy(boxs[x]);
                            if(!rand) score += 100;
                        }
                        
                    }
                    if(down){
                        GameObject exp = (GameObject) Instantiate(fire, new Vector3(bombX, 1.5f, bombZ2), new Quaternion(0, 180, 0, 1));
                        checkExplosion(bombX, bombZ2);
                        if(!rand) checkExplosionEnemy(bombX, bombZ2);
                        Destroy(exp, 0.5f);
                    }
                    //RIGHT
                    if(right && boxs[x].transform.position.x == bombX1 && boxs[x].transform.position.z == bombZ){
                        if(boxs[x].TryGetComponent(out Wall rock)){
                            right = false;
                        }else{
                            Destroy(boxs[x]);
                            if(!rand) score += 100;
                        }
                        
                    }
                    if(right){
                        GameObject exp = (GameObject) Instantiate(fire, new Vector3(bombX1, 1.5f, bombZ), new Quaternion(0, 180, 0, 1));
                        checkExplosion(bombX1, bombZ);
                        if(!rand) checkExplosionEnemy(bombX1, bombZ);
                        Destroy(exp, 0.5f);
                    }
                    //LEFT
                    if(left && boxs[x].transform.position.x == bombX2 && boxs[x].transform.position.z == bombZ){
                        if(boxs[x].TryGetComponent(out Wall rock)){
                            left = false;
                        }else{
                            Destroy(boxs[x]);
                            if(!rand) score += 100;
                        }
                    }
                    if(left){
                        GameObject exp = (GameObject) Instantiate(fire, new Vector3(bombX2, 1.5f, bombZ), new Quaternion(0, 180, 0, 1));
                        checkExplosion(bombX2, bombZ);
                        if(!rand) checkExplosionEnemy(bombX, bombZ1);
                        Destroy(exp, 0.5f);
                    }
                }
            }
            if(rand){
                break;
            }
            aux++;
        }
        //Explosion central
        GameObject exp2 = (GameObject) Instantiate(fire, new Vector3(bombX, 1.5f, bombZ), new Quaternion(0, 180, 0, 1));
        checkExplosion(bombX, bombZ);
        Destroy(exp2, 0.5f);
    }

    void checkExplosion(float X, float Z){
        Transform posPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        float playerX = (float)Math.Round(posPlayer.position.x);
        float playerZ = (float)Math.Round(posPlayer.position.z);

        if(playerX == X && playerZ == Z){
            dead = true;
        }
    }

    void checkExplosionEnemy(float X, float Z){
        for(int x = 0; x < enemies.Count; x++){
            if(enemies[x] != null){
                float enemyX = (float)Math.Round(enemies[x].transform.position.x);
                float enemyZ = (float)Math.Round(enemies[x].transform.position.z);

                if(enemyX == X && enemyZ == Z){
                    Destroy(enemies[x]);
                }
            }
        }
    }

    void UpdatePowerUps(){
        if(powerUps.Count != 0){
            Transform posPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            float playerX = (float)Math.Round(posPlayer.position.x);
            float playerZ = (float)Math.Round(posPlayer.position.z);

            if((int)timer == 50){
                powerUps[0].SetActive(true);
            }
            if((int)timer == 30){
                powerUps[1].SetActive(true);
            }

            if(powerUps[0] != null && powerUps[0].activeSelf){
                if(playerX == 0 && playerZ == (powerUps[0].GetComponent<Transform>().position.z) + 2){ //Posicion de donde esta el power Up
                    distance++;
                    Destroy(powerUps[0]);
                }
            }
            if(powerUps[1] != null && powerUps[1].activeSelf){
                if(playerX == 0 && playerZ == (powerUps[1].GetComponent<Transform>().position.z) + 2){
                    distance++;
                    Destroy(powerUps[1]);
                }
            }
        }
    }

    void randomAgain(){
        random = false;
    }

    void RandomBomb(){
        boxs = GameObject.FindGameObjectsWithTag("Box").ToList();
        int num = Random.Range(0, 100);
        int randX = Random.Range(-11, 11);
        int randZ = Random.Range(-6, 6);
        bool occuped = false;
        
        if(num <= 1){
            if(boxs != null){
                for(int x = 0; x < boxs.Count && !occuped; x++){
                    if(boxs[x].transform.position.x == randX && boxs[x].transform.position.z == randZ){
                        occuped = true;
                    }
                }
            }

            if(!occuped){
                random = true;
                GameObject clone = (GameObject) Instantiate(bomb, new Vector3(randX, 1, randZ), new Quaternion(0, 180, 0, 1));
                StartCoroutine(explode(randX, randZ, true));
                Destroy(clone, 1.5f);
                Invoke("randomAgain", 5f);
            }
        }
    }

    void Pause(){
        if(Time.timeScale == 1){
            Time.timeScale = 0;
            pause.gameObject.SetActive(true);
        }else if(Time.timeScale == 0){
            Time.timeScale = 1;
            pause.gameObject.SetActive(false);
        }
    }

    public void aud(){
        play = !play;
        if(play){
            audios[0].Play();
        }else{
            audios[0].Stop();
        }
    }

    public void restart(){
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void backToMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene("InitialScene", LoadSceneMode.Single);
    }
}