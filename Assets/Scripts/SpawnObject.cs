using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour{
    public GameObject obj;

    public Transform position;
    
    void Start(){
        position = this.GetComponent<Transform>();
        Instantiate(obj, position);
    }
}
