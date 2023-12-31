using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(PlayerAnimationManager))]
public class PlayerController : MonoBehaviour
{
    CharacterMotor motor;
    PlayerAnimationManager animationManager;

    [SerializeField] private bool canMove;
    [SerializeField] AudioClip dashSound;
    // Start is called before the first frame update

    void Start()
    {

        canMove = true;
        motor = GetComponent<CharacterMotor>();
        animationManager = GetComponent<PlayerAnimationManager>();
    }

    public void SetCanMove(bool canMove)
    {
        motor.MovePlayer(0, 0, animationManager);
        animationManager.SetAnimatorBool("isMoving", false);
        this.canMove = canMove;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.IsGameOver())
        {
            return;
        }
        if (canMove)
        {
            motor.MovePlayer(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), animationManager);
        }

    }
    void Update()
    {
        if (Input.GetButtonDown("Jump") && canMove && !motor.IsDashing())
        {
            SoundUtility.PlayOneShot(GetComponent<AudioSource>(), dashSound, 0.2f, 1.3f);

            motor.Dash(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), animationManager);
        }

    }
}
