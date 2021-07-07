using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour{
    GameObject gameManager;
    bool canAttack = true;
    GameObject player;
    private NavMeshAgent agentEnemy;
    //private Animator anim;
    void Start(){
        gameManager = GameObject.Find("GameManager");
        agentEnemy = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update(){
        float distance = Vector3.Distance(agentEnemy.nextPosition, player.transform.position);
        agentEnemy.SetDestination(player.transform.position);

        if(agentEnemy.pathStatus == NavMeshPathStatus.PathComplete && distance < 2 && canAttack){
            float bombX = (float)Math.Round(agentEnemy.nextPosition.x);
            float bombZ = (float)Math.Round(agentEnemy.nextPosition.z);
            gameManager.GetComponent<GameManager>().enemyBomb(bombX, bombZ);
            canAttack = false;
            Invoke("attackAgain", 3f);
        }
        if(agentEnemy.pathStatus == NavMeshPathStatus.PathPartial && canAttack){
            float bombX = (float)Math.Round(agentEnemy.nextPosition.x);
            float bombZ = (float)Math.Round(agentEnemy.nextPosition.z);
            gameManager.GetComponent<GameManager>().enemyBomb(bombX, bombZ);
            canAttack = false;
            Invoke("attackAgain", 3f);
        }
    }

    void attackAgain(){
        canAttack = true;
    }
}
