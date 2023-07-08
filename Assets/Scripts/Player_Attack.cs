using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public int playerAttackDamage = 1, axeBonusDamage = 0, pickaxeBonusDamage = 0, swordBonusDamage = 0;
    [SerializeField]
    private LayerMask hitboxesMask;
    private char direction;
    public bool attackBlocked = false;
    void Update()
    {
        direction = GetComponent<PlayerController>().direction;
        if (Input.GetKeyDown("j"))
        {
            Attack();
        }
    }
    void Attack()
    {      
        if (attackBlocked)
            return;  
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(GetAttackPosition(), 0.2f, hitboxesMask);
        foreach(Collider2D objeto in hitObjects)
        {
            if (objeto.GetComponent<Health>().type == Health.Type.tree)
                objeto.GetComponent<Health>().TakeDamage(playerAttackDamage + axeBonusDamage);
            else if (objeto.GetComponent<Health>().type == Health.Type.rock)
                objeto.GetComponent<Health>().TakeDamage(playerAttackDamage + pickaxeBonusDamage);
            else if (objeto.GetComponent<Health>().type == Health.Type.enemy)
                objeto.GetComponent<Health>().TakeDamage(playerAttackDamage + swordBonusDamage);
        }
        GameObject.FindWithTag("HandItem").GetComponent<Animator>().SetTrigger("attack");
        attackBlocked= true;
        StartCoroutine(DelayAttack());
    }
    Vector3 GetAttackPosition()
    {
        Vector3 dir = new Vector3();
        switch(direction){
            case 'u':
                dir = Vector3.up;
                break;
            case 'd':
                dir = Vector3.down;
                break;
            case 'l':
                dir = Vector3.left;
                break;
            case 'r':
                dir = Vector3.right;
                break;
        }
        return GetComponent<Transform>().position + dir;
    }
    private IEnumerator DelayAttack(){
        yield return new WaitForSecondsRealtime(.4f);
        attackBlocked= false;
    }
}
