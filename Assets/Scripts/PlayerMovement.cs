using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour{
    private NavMeshAgent agent;
    private Animator anim;
    public float Speed = 0.5f;
    //public GameObject bomb;
    
    void Start(){
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        //Debug.Log("Start player");
    }
    
    void Update(){
        //Debug.Log("Update player");
        RaycastHit hit;
        
        if(Input.GetMouseButtonDown(0)){
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)){
                agent.destination = hit.point;
                Speed = 0.6f;
            }else{
                Speed = 0f;
            }
        }

        //Update animation speed
        float distance = Vector3.Distance(agent.destination, agent.nextPosition);

        if(distance == 0f){
            Speed = 0f;
        }else if(distance <= 5f){
            Speed = 0.3f;
        }else{
            Speed = 0.6f;
        }
        //Debug.Log(distance + " " + Speed);
        anim.SetFloat ("Blend", Speed);
    }
}
