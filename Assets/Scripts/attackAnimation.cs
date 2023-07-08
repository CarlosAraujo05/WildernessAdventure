using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackAnimation : StateMachineBehaviour
{   
    
    Transform newTransform;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.FindWithTag("Player").GetComponent<HandItemHandler>().isAttacking = true;
        newTransform = GameObject.FindWithTag("HandItem").GetComponent<Transform>();
        char direction = GameObject.FindWithTag("Player").GetComponent<PlayerController>().direction;
        switch(direction){
            case 'u':
                newTransform.position = GameObject.FindWithTag("Player").GetComponent<Transform>().position + new Vector3(0f,.8f,-1f);
                break;
            case 'd':
                newTransform.position = GameObject.FindWithTag("Player").GetComponent<Transform>().position + new Vector3(0f,-0.5f,-1f);
                newTransform.rotation = Quaternion.Euler(0f,0f,180f);
                break;
            case 'l':
                newTransform.position = GameObject.FindWithTag("Player").GetComponent<Transform>().position + new Vector3(-0.6f,0f,-1f);
                newTransform.rotation = Quaternion.Euler(0f,0f,90f);
                break;
            case 'r':
                newTransform.position = GameObject.FindWithTag("Player").GetComponent<Transform>().position + new Vector3(0.6f,0f,-1f);
                newTransform.rotation = Quaternion.Euler(0f,0f,270f);
                break;
        }
        Transform itemTransform = GameObject.FindWithTag("HandItem").GetComponent<Transform>();
        itemTransform = newTransform;
    }


   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.FindWithTag("Player").GetComponent<HandItemHandler>().isAttacking = false;
        GameObject player = GameObject.FindWithTag("Player");
        if (player.GetComponent<PlayerController>().isLookingRight){
            newTransform.position = GameObject.FindWithTag("Player").GetComponent<Transform>().position + new Vector3(0.6f,0.2f,-1f);
            newTransform.rotation = Quaternion.Euler(0f,0f,0f);
            Transform itemTransform = GameObject.FindWithTag("HandItem").GetComponent<Transform>();
            itemTransform = newTransform;
        }
        else{
            newTransform.position = GameObject.FindWithTag("Player").GetComponent<Transform>().position + new Vector3(-0.6f,0.2f,-1f);
            newTransform.rotation = Quaternion.Euler(0f,0f,0f);
            Transform itemTransform = GameObject.FindWithTag("HandItem").GetComponent<Transform>();
            itemTransform = newTransform;
        }
    }
}
