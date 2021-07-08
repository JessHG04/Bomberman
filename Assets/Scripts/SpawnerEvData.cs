using UnityEngine;

public class SpawnerEvData{
    public SpawnObject.SpawnerType type;
    public GameObject entityGO;

    public SpawnerEvData(SpawnObject.SpawnerType type, GameObject entityGO){
        this.type = type;
        this.entityGO = entityGO;
    }
}