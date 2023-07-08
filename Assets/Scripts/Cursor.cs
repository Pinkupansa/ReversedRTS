using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    float baseSpeed = 500f;
    public void Click()
    {
        Debug.Log("Clicked");
    }

    public void Release()
    {
        Debug.Log("Released");
    }

    public void MoveTowards(Vector2 target)
    {

        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.position += (Vector3)direction * baseSpeed * Time.deltaTime;

    }
}
