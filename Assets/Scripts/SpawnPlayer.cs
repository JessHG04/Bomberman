using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour{
    public GameObject player;

    public Transform position;
    
    void Start(){
        position = this.GetComponent<Transform>();
        
        Instantiate(player, position);
    }
}
