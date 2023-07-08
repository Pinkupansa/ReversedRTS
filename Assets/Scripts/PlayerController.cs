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
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        motor = GetComponent<CharacterMotor>();
        animationManager = GetComponent<PlayerAnimationManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            motor.MovePlayer(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), animationManager);
        }


    }
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            motor.Dash(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), animationManager);
        }

    }
}
