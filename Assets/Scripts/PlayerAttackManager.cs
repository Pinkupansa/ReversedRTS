using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] Weapon currentWeapon;
    [SerializeField] GameObject weaponHolder;
    [SerializeField] float weaponHolderDistance;

    [SerializeField] AudioClip[] attackSounds, hitSounds;
    float attackTimer = 0f;

    bool requestAttack = false;
    bool isAttacking = false;

    int attackCount = 0;

    void Start()
    {
        SwitchWeapon(currentWeapon);
    }

    public void SwitchWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        GameObject newWeaponObject = Instantiate(currentWeapon.Prefab, weaponHolder.transform);
        newWeaponObject.transform.localPosition = Vector3.zero;
        newWeaponObject.transform.localRotation = Quaternion.identity;
    }

    public void QueueAttack()
    {
        requestAttack = true;
    }

    void PlayAttack()
    {
        isAttacking = true;
        GetComponent<PlayerController>().SetCanMove(false);
        //move in the direction of the weapon holder projected on the ground
        Vector3 weaponHolderProjected = new Vector3(weaponHolder.transform.position.x, transform.position.y, weaponHolder.transform.position.z);
        Vector3 direction = (weaponHolderProjected - transform.position).normalized;
        GetComponent<CharacterMotor>().MovePlayer(direction.x, direction.z, GetComponent<PlayerAnimationManager>());
        GetComponent<PlayerAnimationManager>().SetAnimatorAttackCount(attackCount);
        GetComponent<PlayerAnimationManager>().SetAnimatorTrigger("Attack");
    }

    public void Attack()
    {
        isAttacking = false;

        SoundUtility.PlayRandomFromArrayOneShot(GetComponent<AudioSource>(), attackSounds, 0.2f);
        //Overlap sphere around the weapon
        Collider[] colliders = Physics.OverlapSphere(weaponHolder.transform.position, currentWeapon.BaseRange);
        //Show OverlapSphere

        //Check if the collider is an enemy
        foreach (Collider collider in colliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null && collider.gameObject != gameObject)
            {
                SoundUtility.PlayRandomFromArrayOneShot(GetComponent<AudioSource>(), hitSounds, 0.2f);
                damageable.TakeDamage(currentWeapon.BaseDamage, false);

            }
        }

        Vector3 weaponHolderProjected = new Vector3(weaponHolder.transform.position.x, transform.position.y, weaponHolder.transform.position.z);
        Vector3 direction = (weaponHolderProjected - transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(direction * attackCount * 40f, ForceMode.Impulse);
        attackTimer = 1 / currentWeapon.BaseSpeed;
        attackCount = (attackCount + 1) % 3;

        GetComponent<PlayerController>().SetCanMove(true);

    }

    void Update()
    {
        if (GameManager.instance.IsGameOver())
        {
            return;
        }
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mousePos = Input.mousePosition;
        Vector2 direction = (mousePos - screenPos).normalized;

        weaponHolder.transform.localPosition = new Vector3(direction.x, 0, direction.y) * weaponHolderDistance + Vector3.up * 0.5f;

        attackTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && (attackTimer <= 0f || attackCount == 1 || attackCount == 2) && !(isAttacking && attackCount == 2))
        {
            QueueAttack();
        }
        if (!isAttacking)
        {
            if (requestAttack)
            {
                PlayAttack();
                requestAttack = false;
            }
            else
            {
                attackCount = 0;
            }
        }

    }
}
