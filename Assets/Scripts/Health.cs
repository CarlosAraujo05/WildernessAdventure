using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public enum Type{
        tree,
        rock,
        enemy
    }
    public int maxHealth = 5, xp;
    [SerializeField]
    private int currentHealth;
    private Animator animator;
    public Type type;
    bool died = false;
    void Start()
    {
        currentHealth = maxHealth;
        animator = gameObject.GetComponent<Animator>();

    }

    public void TakeDamage(int damage)
    {
        if (died)
            return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        if (animator!= null){
            animator.SetTrigger("hurt");
        }
    }

    public void OnDestroy()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (type == Type.tree){
            player.GetComponent<HandItemHandler>().axeXP+= xp;
        }
        if (type == Type.rock){
            player.GetComponent<HandItemHandler>().pickaxeXP+=xp;
        }
        else if (type == Type.enemy){
            player.GetComponent<HandItemHandler>().swordXP+=xp;
        }
    }
    void Die()
    {
        died = true;
        if (type == Type.enemy){
            gameObject.GetComponent<EnemyController>().died = true;
        }
        if (animator!= null){
            animator.SetTrigger("died");
            StartCoroutine(DelayFor(2f));
        }
        else
            Destroy(gameObject);
    }
    IEnumerator DelayFor(float seconds){
        yield return new WaitForSecondsRealtime(seconds);
        Destroy(gameObject);
    }
}
