using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    [SerializeField] float baseSpeed = 500f;
    [SerializeField] Transform selectionZone;
    bool isSelecting = false;
    void Start()
    {
        //selectionZone.gameObject.SetActive(false);
    }
    public void Click()
    {
        Debug.Log("Clicked");
    }

    public void Release()
    {
        Debug.Log("Released");
    }

    public void MoveTowards(Vector3 target)
    {

        Vector3 direction = (target - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        transform.position += (Vector3)direction * baseSpeed * Time.deltaTime;

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
}
