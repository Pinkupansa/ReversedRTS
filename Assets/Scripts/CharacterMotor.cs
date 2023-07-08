using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMotor : MonoBehaviour
{
    [SerializeField] float speed, dashSpeed;
    private Rigidbody rb;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    public void MovePlayer(float horizontal, float vertical, PlayerAnimationManager animationManager)
    {
        rb.AddForce(new Vector3(horizontal, 0, vertical).normalized * speed, ForceMode.Impulse);

        animationManager.UpdateMovementAnimation(horizontal, vertical);
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void Dash(float horizontal, float vertical, PlayerAnimationManager animationManager)
    {
        rb.AddForce(new Vector3(horizontal, 0, vertical).normalized * dashSpeed, ForceMode.Impulse);
        animationManager.UpdateMovementAnimation(horizontal, vertical);
    }

}
