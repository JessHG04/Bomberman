using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour{
    private GameManager gameManager;
    private bool canAttack = true;
    private NavMeshAgent agentEnemy;
    private Transform playerTransform;
    public Transform PlayerTransform { private get => playerTransform; set => playerTransform = value; }

    void Start(){
        agentEnemy = this.GetComponent<NavMeshAgent>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Update(){
        float distance = Vector3.Distance(agentEnemy.nextPosition, PlayerTransform.position);
        agentEnemy.SetDestination(PlayerTransform.position);

        if(canAttack){
            float bombX = (float)Math.Round(agentEnemy.nextPosition.x);
            float bombZ = (float)Math.Round(agentEnemy.nextPosition.z);
            gameManager.OnSpawnBomb(new Vector3(bombX, 0, bombZ));
            canAttack = false;
            Invoke("attackAgain", 3f);
        }
        /*
        if(agentEnemy.pathStatus == NavMeshPathStatus.PathPartial && distance < 2&& canAttack){
            float bombX = (float)Math.Round(agentEnemy.nextPosition.x);
            float bombZ = (float)Math.Round(agentEnemy.nextPosition.z);
            //gameManager.GetComponent<GameManager>().enemyBomb(bombX, bombZ);
            canAttack = false;
            Invoke("attackAgain", 3f);
        }*/
    }

    void attackAgain(){
        canAttack = true;
    }
}
