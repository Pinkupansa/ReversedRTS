using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour
{
    Animator animator;
    float lastXInput, lastYInput;
    bool lastDirectionWasHorizontal = false;
    public void Start()
    {
        animator = GetComponent<Animator>();
        UpdateMovementAnimation(0, -1);
    }

    private Vector2 RoundAnimationParameters(float horizontal, float vertical)
    {
        /*if (horizontal == 0 && vertical == 0)
        {
            return new Vector2(0, -1);
        }
        else if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            return new Vector2(Mathf.Sign(horizontal), 0);
        }
        else if (Mathf.Abs(horizontal) < Mathf.Abs(vertical))
        {
            return new Vector2(0, Mathf.Sign(vertical));
        }
        else
        {
            if (lastDirectionWasHorizontal)
            {
                return new Vector2(Mathf.Sign(horizontal), 0);
            }
            else
            {
                return new Vector2(0, Mathf.Sign(vertical));
            }
        }*/

        return new Vector2(horizontal > 0 ? 1 : -1, 0);


    }

    public void UpdateMovementAnimation(float xMovement, float yMovement)
    {

        if (Mathf.Abs(xMovement) > 0.1f)
        {
            Vector2 animationParameters = RoundAnimationParameters(xMovement, yMovement);

            animator.SetBool("isMoving", true);
            animator.SetFloat("Xinput", animationParameters.x);
            // animator.SetFloat("Yinput", animationParameters.y);

        }
        else if (Mathf.Abs(yMovement) > 0.01f)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (Mathf.Abs(xMovement - lastXInput) > 0.01f)
        {
            lastDirectionWasHorizontal = true;
            lastXInput = xMovement;
        }
        else if (Mathf.Abs(yMovement - lastYInput) > 0.01f)
        {
            lastDirectionWasHorizontal = false;
            lastYInput = yMovement;
        }

    }
    public void SetAnimatorTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
    public void SetAnimatorBool(string boolName, bool value)
    {
        animator.SetBool(boolName, value);
    }
    public void SetAnimatorAttackCount(int value)
    {
        animator.SetInteger("AttackNumber", value);
    }
}
