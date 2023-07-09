using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMotor : MonoBehaviour
{
    [SerializeField] float speed, dashSpeed;
    bool isDashing;
    private Rigidbody rb;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        isDashing = false;
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
        if (isDashing)
        {
            return;
        }
        float oldDrag = rb.drag;
        rb.drag = 10;
        StartCoroutine(WaitBeforeStoppingDash(oldDrag));
        rb.AddForce(new Vector3(horizontal, 0, vertical).normalized * dashSpeed, ForceMode.Impulse);
        animationManager.UpdateMovementAnimation(horizontal, vertical);
        isDashing = true;
    }

    IEnumerator WaitBeforeStoppingDash(float oldDrag)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        rb.drag = oldDrag;
        isDashing = false;
    }
    public bool IsDashing()
    {
        return isDashing;
    }
}
