using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour{
    GameObject player;
    private NavMeshAgent agentEnemy;
    //private Animator anim;
    void Start(){
        agentEnemy = this.GetComponent<NavMeshAgent>();
        //anim = this.GetComponent<Animator>();
        player = GameObject.Find("Player(Clone)");
    }

    void Update(){
        agentEnemy.destination = player.transform.position;
        //float distance = Vector3.Distance(agentEnemy.destination, agentPlayer.destination);
    }
}
