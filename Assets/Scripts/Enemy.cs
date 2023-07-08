using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyBehaviour
{
    Idle,
    Chasing,
    Attacking,
    TakingDamage,
}

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(PlayerAnimationManager))]

public class Enemy : MonoBehaviour, IDamageable
{
    Transform target;
    CharacterMotor motor;
    PlayerAnimationManager animationManager;
    AnimationClip attackAnimation;
    EnemyBehaviour state = EnemyBehaviour.Idle;

    [SerializeField] float attackRange, attackSpeed, damageImmoTime;
    [SerializeField] int attackDamage, maxHealth;
    [SerializeField] GameObject deathParticles;
    int currentHealth;
    float attackTimer;

    float immobilisationTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        target = null;
        motor = GetComponent<CharacterMotor>();
        animationManager = GetComponent<PlayerAnimationManager>();
        currentHealth = maxHealth;
    }

    public void Immobilise(float time)
    {
        immobilisationTimer = time;
        state = EnemyBehaviour.TakingDamage;
    }
    public void TakeDamage(int damage, bool isImmobilisation)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        if (isImmobilisation)
        {
            Immobilise(damageImmoTime);
        }
    }
    private void Die()
    {
        if (deathParticles != null)
            Instantiate(deathParticles, transform.position, Quaternion.identity);
        CamManager.instance.CamShake(0.2f);
        Destroy(gameObject);
    }

    void CalculateNewState()
    {

        switch (state)
        {
            case EnemyBehaviour.Chasing:
                if (target == null)
                {
                    state = EnemyBehaviour.Idle;
                    break;
                }

                if (Vector3.Distance(transform.position, target.position) < attackRange)
                {
                    state = EnemyBehaviour.Attacking;
                    motor.MovePlayer(0, 0, animationManager);
                }

                break;
            case EnemyBehaviour.Attacking:
                if (target == null)
                {
                    state = EnemyBehaviour.Idle;
                    break;
                }
                if (Vector3.Distance(transform.position, target.position) > attackRange)
                {
                    state = EnemyBehaviour.Chasing;
                }
                break;
            case EnemyBehaviour.TakingDamage:
                if (immobilisationTimer > 0)
                {
                    immobilisationTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    state = EnemyBehaviour.Chasing;
                }
                break;
            case EnemyBehaviour.Idle:
                if (target != null)
                {
                    state = EnemyBehaviour.Chasing;
                }
                break;
        }
    }
    void Chase()
    {
        if (target != null)
        {
            motor.MovePlayer((target.position - transform.position).normalized.x, (target.position - transform.position).normalized.z, animationManager);
        }

    }
    void Attack()
    {
        if (attackTimer > attackSpeed)
        {
            //Cake.instance.TakeDamage(attackDamage);
            animationManager.SetAnimatorTrigger("Eat");
            //SoundUtility.PlayRandomFromArrayLoop(GetComponent<AudioSource>(), attackSounds, 0.8f, false);

            attackTimer = 0;
        }
        else
        {
            attackTimer += Time.fixedDeltaTime;
        }
    }
    void TakeDamage()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //if (GameManager.instance.IsGameOver)
        //{
        // return;
        // }
        CalculateNewState();
        switch (state)
        {
            case EnemyBehaviour.Chasing:
                Chase();
                break;
            case EnemyBehaviour.Attacking:
                Attack();
                break;
            case EnemyBehaviour.TakingDamage:
                TakeDamage();
                break;
            case EnemyBehaviour.Idle:
                Idle();
                break;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        state = EnemyBehaviour.Chasing;
    }

    public EnemyBehaviour GetState()
    {
        return state;
    }
    void Idle()
    {
        motor.MovePlayer(0, 0, animationManager);
    }
}

