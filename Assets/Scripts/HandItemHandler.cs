using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItemHandler : MonoBehaviour
{
    public bool isAttacking = false;
    const int AXE = 0;
    const int PICKAXE = 1;
    const int SWORD = 2;

    public int axeXP, pickaxeXP, swordXP, currentItem;
    public GameObject stoneAxe, goldAxe, diamondAxe, stonePickaxe,goldPickaxe, diamondPickaxe,
        stoneSword, goldSword, diamondSword;
    private bool isLookingRight;
    private Transform itemTransform;
    void Start(){
        currentItem = 2;
        Instantiate(stoneSword, transform);
        ChangeDamage(2,2,0,0);
        itemTransform = GameObject.FindWithTag("HandItem").GetComponent<Transform>();
        itemTransform.position = GameObject.FindWithTag("Player").GetComponent<Transform>().position + new Vector3(0.6f,0.2f,-1f);
    }
    void Update()
    {
        isLookingRight = GetComponent<PlayerController>().isLookingRight;
        if (Input.GetKeyDown("u"))
        {
            if (currentItem== 0)
                currentItem = 2;
            else
                currentItem--;
            ChangeItem();
        }
        else if (Input.GetKeyDown("i"))
        {
            if (currentItem== 2)
                currentItem = 0;
            else
                currentItem++;
            ChangeItem();
        }
        itemTransform = GameObject.FindWithTag("HandItem").GetComponent<Transform>();
        if(isAttacking == false){
            if (isLookingRight)
            {
                itemTransform.position = GameObject.FindWithTag("Player").GetComponent<Transform>().position + new Vector3(0.6f,0.2f,-1f);
                GameObject.FindWithTag("HandItem").GetComponent<SpriteRenderer>().flipX = false;
            }
            else 
            {
                itemTransform.position = GameObject.FindWithTag("Player").GetComponent<Transform>().position + new Vector3(-0.6f,0.2f,-1f);
                GameObject.FindWithTag("HandItem").GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        
    }
    void ChangeItem(){
        Destroy(GameObject.FindWithTag("HandItem"));
        switch(currentItem){
            case AXE:
                if (axeXP < 100){
                    Instantiate(stoneAxe, transform);
                    ChangeDamage(2,2,0,0);
                }
                else if (axeXP < 300){
                    Instantiate(goldAxe, transform);
                    ChangeDamage(4,3,0,0);
                }
                else{
                    Instantiate(diamondAxe, transform);
                    ChangeDamage(6,4,0,0);
                }
                break;
            case PICKAXE:
                if (pickaxeXP < 100){
                    Instantiate(stonePickaxe, transform);
                    ChangeDamage(2,0,2,0);
                }
                else if (pickaxeXP < 300){
                    Instantiate(goldPickaxe, transform);
                    ChangeDamage(4,0,3,0);
                }
                else{
                    Instantiate(diamondPickaxe, transform);
                    ChangeDamage(6,0,4,0);
                }
                break;
            case SWORD:
                if (swordXP < 100){
                    Instantiate(stoneSword, transform);
                    ChangeDamage(2,0,0,2);
                }
                else if (swordXP < 300){
                    Instantiate(goldSword, transform);
                    ChangeDamage(4,0,0,3);
                }
                else{
                    Instantiate(diamondSword, transform);
                    ChangeDamage(6,2,0,4);
                }
                break;    
        }
    }
    void ChangeDamage(int _playerAttackDamage, int _axeBonusDamage, int _pickaxeBonusDamage, int _swordBonusDamage){
        GameObject.FindWithTag("Player").GetComponent<Player_Attack>().playerAttackDamage = _playerAttackDamage;
        GameObject.FindWithTag("Player").GetComponent<Player_Attack>().axeBonusDamage = _axeBonusDamage;
        GameObject.FindWithTag("Player").GetComponent<Player_Attack>().pickaxeBonusDamage = _pickaxeBonusDamage;
        GameObject.FindWithTag("Player").GetComponent<Player_Attack>().swordBonusDamage = _swordBonusDamage;
    }
}
