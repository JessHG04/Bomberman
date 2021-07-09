using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour{
    
    #region Public Variables
    public GameObject bombPrefab;
    public GameObject firePrefab;
    public Canvas pauseMenuCanvas;
    public Canvas winScreenCanvas;
    public Canvas loseScreenCanvas;
    public float gameCountdown = 60.0f;
    public int bombRadius = 1;
    public int score;
    
    [Header("Audio Sources")] 
    public AudioSource backgroundMusic;

    public AudioSource explosionAudioSource;
   
    #endregion
    
    #region Private Variables

    private bool dead;
    private bool play = true;
    private List<GameObject> boxs = new List<GameObject>();
    private List<GameObject> powerUps = new List<GameObject>();
    private bool random;
    private List<GameObject> enemyList = new List<GameObject>();
    private Transform _playerTransform;
                        
    #endregion

    #region Events
    private event EventHandler<TimerEventData> TimerEnded;
    private event EventHandler<bool> FinishGame;
    
    #endregion

    #region Unity Methods

    private void OnEnable(){
        TimerEnded += OnTimerEndedBehaviour;
        SpawnObject.ObjectSpawned += SpawnObjectOnObjectSpawned;
        FinishGame += OnGameFinished;

    }
    
    private void OnDisable(){
        TimerEnded -= OnTimerEndedBehaviour;
        SpawnObject.ObjectSpawned -= SpawnObjectOnObjectSpawned;
        FinishGame -= OnGameFinished;
    }

    public void Start(){
        backgroundMusic.Play();
        InitializeBoxes();
        InitializePowerUps();
        Invoke(nameof(OnTimerEnded), gameCountdown);
    }

    
    public void Update(){        
        if(Input.GetKeyDown("space")){
            PutBomb();
        }
        UpdatePowerUps();
        if(!random){
            RandomBomb();
        }
        if(Input.GetKeyDown("p")){
            Pause();
        }
    }

    #endregion

    #region Utility Methods

    private void SpawnObjectOnObjectSpawned(object sender, SpawnerEvData e){
        //Debug.LogWarning($"He spawneado un objeto de tipo {e.type}");
        if (e.type == SpawnObject.SpawnerType.Enemy){
            enemyList.Add(e.entityGO);
        }
        
        if (e.type == SpawnObject.SpawnerType.Player){
            _playerTransform = e.entityGO.GetComponent<Transform>();
        }
    }
    
    private void OnTimerEndedBehaviour(object sender, TimerEventData ted){
        //Debug.LogWarning($"El valor que se le pasa al evento es, para el timer {ted.totaltime}, para las vidas {ted.lifes}, y el nombre del prefab a instanciar e {ted.bombPrefab.name}");
        winScreenCanvas.gameObject.SetActive(true);
        Invoke(nameof(goMenu), 0.5f);
    }

    private void OnGameFinished(object sender, bool win){
        //Debug.LogWarning("Ganar la partida: " + win);
        if(win){
            winScreenCanvas.gameObject.SetActive(true);
        }else{
            loseScreenCanvas.gameObject.SetActive(true);
        }
        Invoke(nameof(goMenu), 0.5f);
    }
    
    private void InitializeBoxes(){
        boxs = GameObject.FindGameObjectsWithTag("Box").ToList();
    }
    private void InitializePowerUps(){
        powerUps = GameObject.FindGameObjectsWithTag("PowerUp").ToList();
        foreach (var powerUp in powerUps)
        {
            powerUp.SetActive(false);
        }
    }
    
    public void PutBomb(){
        //Debug.Log("PutBomb");
        var bombX = (float)Math.Round(_playerTransform.position.x);
        var bombZ = (float)Math.Round(_playerTransform.position.z);
        var bombs = GameObject.FindGameObjectsWithTag("Bomb");
        var occuped = false;
        //No pongo 2 bombas en el mismo sitio
        if(bombs != null){
            for(int x = 0; x < bombs.Length && !occuped; x++){
                if(bombs[x].GetComponent<Transform>().position.x == bombX && bombs[x].GetComponent<Transform>().position.z == bombZ){
                    occuped = true;
                }
            }
        }

        if(!occuped){
            GameObject clone = (GameObject) Instantiate(bombPrefab, new Vector3(bombX, 1, bombZ), new Quaternion(0, 180, 0, 1));
            StartCoroutine(explode(bombX, bombZ, false));
            Destroy(clone, 1.5f);
        }
    }

    public void enemyBomb(float bombX, float bombZ){
        GameObject b = (GameObject) Instantiate(bombPrefab, new Vector3(bombX, 1, bombZ), new Quaternion(0, 180, 0, 1));
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
            explosionAudioSource.Play();
        }

        while(aux != bombRadius + 1){
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
                        GameObject exp = (GameObject) Instantiate(firePrefab, new Vector3(bombX, 1.5f, bombZ1), new Quaternion(0, 180, 0, 1));
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
                        GameObject exp = (GameObject) Instantiate(firePrefab, new Vector3(bombX, 1.5f, bombZ2), new Quaternion(0, 180, 0, 1));
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
                        GameObject exp = (GameObject) Instantiate(firePrefab, new Vector3(bombX1, 1.5f, bombZ), new Quaternion(0, 180, 0, 1));
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
                        GameObject exp = (GameObject) Instantiate(firePrefab, new Vector3(bombX2, 1.5f, bombZ), new Quaternion(0, 180, 0, 1));
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
        GameObject exp2 = (GameObject) Instantiate(firePrefab, new Vector3(bombX, 1.5f, bombZ), new Quaternion(0, 180, 0, 1));
        checkExplosion(bombX, bombZ);
        Destroy(exp2, 0.5f);
    }

    void checkExplosion(float X, float Z){
        var playerX = (float)Math.Round(_playerTransform.position.x);
        var playerZ = (float)Math.Round(_playerTransform.position.z);

        if(playerX == X && playerZ == Z){
            //Debug.Log("X: " + X + " " + playerX);
            //Debug.Log("Z: " + Z + " " + playerZ);
            //Debug.Log("me muero");
            OnFinishGame(false);
            /* LANZAR EVENTO DE LOSE */
            //loseScreenCanvas.gameObject.SetActive(true);
            //Invoke(nameof(goMenu), 2.0f);
        }
    }

    void checkExplosionEnemy(float X, float Z){
        for(int x = 0; x < enemyList.Count; x++){
            if(enemyList[x] != null){
                float enemyX = (float)Math.Round(enemyList[x].transform.position.x);
                float enemyZ = (float)Math.Round(enemyList[x].transform.position.z);
                if(enemyX == X && enemyZ == Z){
                    /* DESTRIUR ENEMIGO */
                    /* QUITAR ENEMIGO DE LA LISTA */
                    /* COMPROBAR SI LA LISTA ESTA VACIA */
                    /* SI ESTA VACIA, LANZO EVWENTO DE FIN DE PARTIDA */
                    Destroy(enemyList[x].gameObject);
                    //Debug.LogError("SE HA MUERTO UN ENEMIGO");
                    //Debug.LogWarning($"{enemyList.Capacity} || {enemyList.Count}");
                    
                    if(enemyList.Count <= 0){
                        OnFinishGame(true);
                    }
                }
            }
        }
    }

    void UpdatePowerUps(){
        if(powerUps.Count != 0){
            float playerX = (float)Math.Round(_playerTransform.position.x);
            float playerZ = (float)Math.Round(_playerTransform.position.z);

            if((int)gameCountdown == 50){
                powerUps[0].SetActive(true);
            }
            if((int)gameCountdown == 30){
                powerUps[1].SetActive(true);
            }

            if(powerUps[0] != null && powerUps[0].activeSelf){
                if(playerX == 0 && playerZ == (powerUps[0].transform.position.z) + 2){ //Posicion de donde esta el power Up
                    bombRadius++;
                    Destroy(powerUps[0]);
                }
            }
            if(powerUps[1] != null && powerUps[1].activeSelf){
                if(playerX == 0 && playerZ == (powerUps[1].transform.position.z) + 2){
                    bombRadius++;
                    Destroy(powerUps[1]);
                }
            }
        }
    }

    void randomAgain(){
        random = false;
    }

    void RandomBomb(){
        int num = Random.Range(0, 100);
        int randX = Random.Range(-11, 11);
        int randZ = Random.Range(-6, 6);
        bool occuped = false;
        
        if(num <= 1){
            if(boxs != null){
                for(int x = 0; x < boxs.Count && !occuped; x++){
                    if(boxs[x] != null){
                        if(boxs[x].transform.position.x == randX && boxs[x].transform.position.z == randZ){
                            occuped = true;
                        }
                    }
                }
            }

            if(!occuped){
                random = true;
                GameObject clone = (GameObject) Instantiate(bombPrefab, new Vector3(randX, 1, randZ), new Quaternion(0, 180, 0, 1));
                StartCoroutine(explode(randX, randZ, true));
                Destroy(clone, 1.5f);
                Invoke("randomAgain", 5f);
            }
        }
    }

    #region PauseMenu
    void Pause(){
        if(Time.timeScale == 1){
            Time.timeScale = 0;
            pauseMenuCanvas.gameObject.SetActive(true);
        }else if(Time.timeScale == 0){
            Time.timeScale = 1;
            pauseMenuCanvas.gameObject.SetActive(false);
        }
    }

    public void aud(){
        play = !play;
        if(play){
            backgroundMusic.Play();
        }else{
            backgroundMusic.Stop();
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
    
    #endregion
    void goMenu(){
        SceneManager.LoadScene("InitialScene", LoadSceneMode.Single);
    }

    protected void OnTimerEnded(){
        TimerEnded?.Invoke(this, new TimerEventData(15.2f,23, bombPrefab));
    }

    protected void OnFinishGame(bool state){
        FinishGame?.Invoke(this, state);
    }

    #endregion
}