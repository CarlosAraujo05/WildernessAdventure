using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]private float speed = 2;
    [SerializeField]private LayerMask obstacleMask;
    [HideInInspector]public char direction = '.';
    [HideInInspector]public bool isLookingRight = true;

    public Transform transform;
    public Vector3 movePointPos;
    Animator animator;
    public bool died = false;

    void Start() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (died)
            return;
        float time = GameObject.FindWithTag("Timer").GetComponent<Timer>().time;
        float movementAmout = speed * Time.deltaTime;
        
        transform.position = Vector3.MoveTowards(transform.position, movePointPos, movementAmout);
        if (transform.position == movePointPos)
            animator.SetBool("is Running", false);
        else
            animator.SetBool("is Running", true);
        if (Vector3.Distance(transform.position, movePointPos) <= 0.05f && (int)time %3 == 0) {
            switch(UnityEngine.Random.Range(0, 4))
            {
                case 0: //Cima
                    direction = 'u';
                    Move(new Vector3(0,1,0));
                    break;
                case 1: //Direita
                    direction = 'r';
                    Move(new Vector3(1,0,0));
                    break;
                case 2: //Baixo
                    direction = 'd';
                    Move(new Vector3(0,-1,0));
                    break;                
                case 3: //Esquerda
                    direction = 'l';
                    Move(new Vector3(-1,0,0));
                    break;
            }
            
            if(direction == 'l'&& isLookingRight == true){
                
                isLookingRight = false;
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if(direction == 'r'&&isLookingRight == false){
                
                isLookingRight = true;
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            
        }
    }

    private void Move(Vector3 direction) {
        Vector3 newPosition = movePointPos + direction;
        if (!Physics2D.OverlapCircle(newPosition, 0.3f, obstacleMask)) {
            movePointPos = newPosition;
        }
    }
}
