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
        SoundUtility.PlayRandomFromArrayOneShot(GetComponent<AudioSource>(), attackSounds, 0.2f);
        //Overlap sphere around the weapon
        Collider[] colliders = Physics.OverlapSphere(weaponHolder.transform.position, currentWeapon.BaseRange);
        //Show OverlapSphere

        //Check if the collider is an enemy
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                SoundUtility.PlayRandomFromArrayOneShot(GetComponent<AudioSource>(), hitSounds, 0.2f);
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

        weaponHolder.transform.localPosition = new Vector3(direction.x, 0, direction.y) * weaponHolderDistance + Vector3.up * 0.5f;

        attackTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && attackTimer <= 0f)
        {
            Attack();
        }

    }
}
