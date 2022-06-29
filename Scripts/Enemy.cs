using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum EnemyState
    {
        Move,
        Attack,
        Die
    }

    EnemyState enemyState;  
    public int enemyDamage = 2;    
    //private int enemyMaxHp = 10;
    public int enemyCurrHp = 10;
    private float enemyMoveSpeed = 2f;
    private float attackTimeInterval = 2f;
    private float lastAttackTime = 0;
    private int enemyExp = 20;
    private Transform playerTr;
    private Animator enemyAnimator;
    //public GameObject attackPoint;

    private void Start()
    {
        playerTr = GameObject.Find("Player").transform;
        enemyAnimator = GetComponentInChildren<Animator>();       
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState == EnemyState.Die) return;
        
        StateCheck();

        if (enemyState == EnemyState.Move) MoveToPlayer();
        else if (enemyState == EnemyState.Attack) AttackToPlayer();
       
    }

    void StateCheck()
    {
        if ((playerTr.position - transform.position).magnitude > 2.5f) enemyState = EnemyState.Move;
        else if ((playerTr.position - transform.position).magnitude <= 2.5f) enemyState = EnemyState.Attack;       
    }

    void MoveToPlayer()
    {        
        transform.position = Vector3.MoveTowards(transform.position, playerTr.position, enemyMoveSpeed * Time.deltaTime);
        transform.LookAt(playerTr.position);
    }

    void AttackToPlayer()
    {
        if(Time.time >= lastAttackTime + attackTimeInterval)
        {
            lastAttackTime = Time.time;           
            enemyAnimator.SetTrigger("Attack");
            Invoke("Attack", 0.6f);
        }
    }

    void Attack()
    {
        playerTr.GetComponent<Player>().OnDamage(enemyDamage, this);
    }
   
    void Die()
    {        
        enemyAnimator.SetTrigger("Die");
        GameManager.Instance.PlayerExpUp(enemyExp);
        GameManager.Instance.enemyCount.Remove(this.gameObject);

        Invoke("EnemyDestory", 3.5f);
       
    }

    public void OnDamage(int dam)
    {
        if (enemyState == EnemyState.Die) return;
        enemyCurrHp -= dam;
        if (enemyCurrHp <= 0)
        {
            enemyState = EnemyState.Die;
            Die();
        }
    }

    void EnemyDestory()
    {
        Destroy(gameObject);
    }
}
