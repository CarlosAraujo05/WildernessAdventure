using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    [SerializeField]private float speed = 5;
    [SerializeField]private LayerMask obstacleMask;
    [HideInInspector]public char direction = '.';
    [HideInInspector]public bool isLookingRight = true;
    private Transform movePoint;
    Animator animator;
    void Start() {
        movePoint = GameObject.FindWithTag("Move Point").GetComponent<Transform>();
        movePoint.parent = null; // Detach partent
        animator = GetComponent<Animator>();
    }

    void Update() {
        float movementAmout = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementAmout);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f) {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) {
                Move(new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0));
                if (Input.GetAxisRaw("Horizontal") < 0)
                    direction = 'l';
                else 
                    direction = 'r';

                animator.SetBool("is Running", true);
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f) {
                Move(new Vector3(0, Input.GetAxisRaw("Vertical"), 0));
                if (Input.GetAxisRaw("Vertical") < 0)
                    direction = 'd';
                else 
                    direction = 'u';
                animator.SetBool("is Running", true);
            }
            else
            {
                animator.SetBool("is Running", false);
            }
            if(direction == 'l'&& isLookingRight == true){
                animator.SetTrigger("turn");
                isLookingRight = false;
            }
            else if(direction == 'r'&&isLookingRight == false){
                animator.SetTrigger("turn");
                isLookingRight = true;
            }
        }
    }

    private void Move(Vector3 direction) {
        Vector3 newPosition = movePoint.position + direction;
        if (!Physics2D.OverlapCircle(newPosition, 0.2f, obstacleMask)) {
            movePoint.position = newPosition;
        }
    }
}

