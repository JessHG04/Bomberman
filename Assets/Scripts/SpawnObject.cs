using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour{
    public enum SpawnerType{
        Player,
        Enemy
    }

    public SpawnerType spawnerType;
    public GameObject obj;
    public Transform position;
    public Transform pTransform;

    public static event EventHandler<SpawnerEvData> ObjectSpawned;

    private void Start(){
        position = this.GetComponent<Transform>();
        var go = Instantiate(obj, position);
        OnObjectSpawned(new SpawnerEvData(spawnerType, go));
        go.GetComponent<Enemy>().PlayerTransform = pTransform;
    }

    private static void OnObjectSpawned(SpawnerEvData evtData){
        ObjectSpawned?.Invoke(null, evtData);
    }
}