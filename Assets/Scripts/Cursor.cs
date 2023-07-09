using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorState
{
    Base,
    Sword
}
public class Cursor : MonoBehaviour
{

    [SerializeField] float baseSpeed = 500f;
    [SerializeField] Transform selectionZone;
    [SerializeField] GameObject baseSprite, swordSprite, deleteMenu;
    [SerializeField] float acceleration = 1f;

    CursorState state = CursorState.Base;
    bool isSelecting = false;
    float currentSpeed = 0f;
    void Start()
    {
        selectionZone.gameObject.SetActive(false);
        HideDeleteMenu();
        SwitchToBase();
    }
    public void Click()
    {
        baseSprite.GetComponent<Animator>().SetTrigger("Clic");
    }

    public void Angry()
    {
        baseSprite.GetComponent<Animator>().SetTrigger("Angry");
    }
    public void Release()
    {
        Debug.Log("Released");
    }
    public void MoveTowards(Vector3 target)
    {
        float speedMult = (Vector3.Distance(transform.position, target) / 3f);
        if (currentSpeed < baseSpeed * speedMult)
        {
            currentSpeed += Time.deltaTime * acceleration;
        }
        else
        {
            currentSpeed -= Time.deltaTime * acceleration;
        }
        Vector3 direction = (target - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        transform.position += (Vector3)direction * currentSpeed * Time.deltaTime;

    }

    public void StartSelecting()
    {
        isSelecting = true;

        selectionZone.position = transform.position;
    }

    public void StopSelecting()
    {
        isSelecting = false;
        selectionZone.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isSelecting)
        {
            selectionZone.gameObject.SetActive(true);
            selectionZone.transform.localScale = new Vector3(transform.position.x - selectionZone.position.x, 1, selectionZone.position.z - transform.position.z);
        }

    }


    public void SwitchToSword()
    {
        swordSprite.SetActive(true);
        baseSprite.SetActive(false);
        state = CursorState.Sword;
    }

    public void SwitchToBase()
    {
        swordSprite.SetActive(false);
        baseSprite.SetActive(true);
        state = CursorState.Base;
    }

    public bool IsAnimOn(string anim)
    {
        //check if the current animation is the one we want
        return baseSprite.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(anim);
    }

    public void ShowDeleteMenu()
    {
        deleteMenu.transform.position = transform.position;
        deleteMenu.SetActive(true);

    }

    public void HideDeleteMenu()
    {
        deleteMenu.SetActive(false);
    }
}
