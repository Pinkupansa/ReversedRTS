using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] Weapon currentWeapon;
    [SerializeField] GameObject weaponHolder;
    [SerializeField] float weaponHolderDistance;

    [SerializeField] AudioClip[] attackSounds;
    float attackTimer = 0f;


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

    public void Attack()
    {
        Debug.Log("Attacking");
        SoundUtility.PlayRandomFromArrayOneShot(GetComponent<AudioSource>(), attackSounds, 0.2f, false);
        //Overlap sphere around the weapon
        Collider2D[] colliders = Physics2D.OverlapCircleAll(weaponHolder.transform.position, currentWeapon.BaseRange);
        //Check if the collider is an enemy
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                //Damage the enemy
                collider.GetComponent<Enemy>().TakeDamage(currentWeapon.BaseDamage, false);
            }
        }

        attackTimer = 1 / currentWeapon.BaseSpeed;
    }

    void Update()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mousePos = Input.mousePosition;
        Vector2 direction = (mousePos - screenPos).normalized;

        weaponHolder.transform.localPosition = direction * weaponHolderDistance;
        weaponHolder.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        weaponHolder.transform.localPosition = new Vector3(weaponHolder.transform.localPosition.x, weaponHolder.transform.localPosition.y, weaponHolder.transform.localPosition.y);


        attackTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && attackTimer <= 0f)
        {
            Attack();
        }

    }
}
