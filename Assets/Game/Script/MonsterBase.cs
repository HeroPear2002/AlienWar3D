using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBase : MonoBehaviour
{
    protected string monsterName;
    [SerializeField]protected float health;
    protected float damage;
    [SerializeField]protected GameObject target;
    protected bool movetoplayer = false;
    protected float distance;
    protected float KC = 3;
    protected bool attack = false;
    protected bool isWaiting = false;
    protected bool gethit = false;
    protected float rotationSpeed = 20f;
    protected float moveSpeed = 4f;
    protected Animator animator;
    protected NavMeshAgent agent;
    protected HeadController headController;
    protected float coin = 10;
    protected int isCoin = 1;
    protected enum MovementState { idle, walk, run, attack1, attack2, gethit }
    protected virtual void Awake()
    {
        headController = FindObjectOfType<HeadController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.autoBraking = false;
    }
    public void Initialize(string name, float hp, float dmg, float ms, float rs)
    {
        monsterName = name;
        health = hp;
        damage = dmg;
        moveSpeed = ms;
        rotationSpeed = rs;
    }
    protected virtual void Update()
    {
        MoveToPlayer();
        AnimationState();
        MonsterDead();
    }
    public virtual void Attack(HeadController player)
    {
        player.Headdown(damage);
    }
    public virtual void PlayerAttack(float damage)
    {
        health -= damage;
        
    }
    public virtual void MoveToPlayer()
    {
        distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance < 100 && health != 0 && !isWaiting)
        {
            MoveToPosition(target.transform.position);
        }
    }
    public virtual void MoveToPosition(Vector3 position)
    {
        if (gethit)
        {
            StartCoroutine(WaitAndMove(0.8f));
        }
        if (distance > KC)
        {
            movetoplayer = true;
            attack = false;
            agent.isStopped = false;
            agent.SetDestination(position);
        }
        else if (distance < KC)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            movetoplayer = false;
            attack = true;
            StartCoroutine(WaitAndMove(1.6f));
            Vector3 directionToTarget = target.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    public virtual void AnimationState()
    {
        MovementState state = MovementState.idle;
        if (movetoplayer)
        {
            state = MovementState.run;
        }
        else if (attack)
        {
            state = MovementState.attack1;
        }
        if (gethit)
        {
            state = MovementState.gethit;
        }
        animator.SetInteger("State", (int)state);
    }
    IEnumerator WaitAndMove(float waittime)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waittime);
        isWaiting = false;
        gethit = false;
    }
    public virtual void MonsterDead()
    {
        if (health <= 0 && isCoin == 1)
        {
            animator.SetTrigger("death");
            agent.velocity = Vector3.zero;
            StartCoroutine(DestroyAfterAnimation());
            headController.Killcoin += coin;
            isCoin = 0;
        }

    }
    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    public virtual void OnChildTriggerEnter(Collider other, GameObject child)
    {
        if (other.CompareTag("bullet"))
        {
            BulletController bullet = other.GetComponent<BulletController>();
            if (bullet != null)
            {
                PlayerAttack(bullet.damage);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("Player"))
        {
            HeadController player = other.GetComponent<HeadController>();
            if (player != null && attack)
            {
                Attack(player);
            }
        }
    }
    public virtual void OnChildCollisionEnter(Collision collision, GameObject child)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HeadController player = collision.gameObject.GetComponent<HeadController>();
            if (player != null && attack)
            {
                Attack(player);
            }
        }
    }
}