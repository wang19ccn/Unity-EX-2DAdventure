using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.LowLevel.InputEventTrace;

public class PlayerAnimation : MonoBehaviour
{
    // 现在Animation这个东西是弃用的老系统，不要用它，只用AnimClip和Animator
    private Animator anim;
    private Rigidbody2D rb;
    public PlayerInputControl inputControl;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();

        inputControl = new PlayerInputControl();
        inputControl.Gameplay.Control.performed += CtrlKeydown;
        inputControl.Gameplay.Control.canceled += CtrlKeyup;

        Debug.Log("12222233");
    }

    private void Update()
    {
        SetAnimation();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable(); // 可以通过开/关 切换绑定按键的功能
    }

    public void SetAnimation()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
        anim.SetBool("isDead", playerController.isDead);
        anim.SetBool("isAttack", playerController.isAttack);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }

    public void PlayAttack()
    {
        anim.SetTrigger("attack");
    }

    public void CtrlKeydown(InputAction.CallbackContext context)
    {
        Debug.Log("CtrlKeydown method called");
        Debug.Log("Context value: " + context);

        //anim.SetBool("ctrlKeydown", context);


        anim.SetBool("ctrlKeydown", true);
    }

    public void CtrlKeyup(InputAction.CallbackContext context)
    {
        anim.SetBool("ctrlKeydown", false);
    }
}
