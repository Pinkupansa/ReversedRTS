using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMotor : MonoBehaviour
{
    [SerializeField] float speed, dashSpeed;
    private Rigidbody2D rb;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    public void MovePlayer(float horizontal, float vertical, PlayerAnimationManager animationManager)
    {
        rb.AddForce(new Vector2(horizontal, vertical).normalized * speed, ForceMode2D.Impulse);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        animationManager.UpdateMovementAnimation(horizontal, vertical);
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void Dash(float horizontal, float vertical, PlayerAnimationManager animationManager)
    {
        rb.AddForce(new Vector3(horizontal, vertical, vertical).normalized * dashSpeed, ForceMode2D.Impulse);
        animationManager.UpdateMovementAnimation(horizontal, vertical);
    }

}
