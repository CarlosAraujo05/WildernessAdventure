using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turning : StateMachineBehaviour
{
    public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player.GetComponent<PlayerController>().isLookingRight)
            player.GetComponent<SpriteRenderer>().flipX = false;
        else
            player.GetComponent<SpriteRenderer>().flipX = true;
    }
}
