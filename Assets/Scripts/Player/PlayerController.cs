using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private SpriteRenderer sr;
    private CapsuleCollider2D coll;
    public Vector2 inputDirection;

    [Header("Base Params")]
    private float initSpeed;
    public float speed;
    public float jumpForce;
    public float hurtForce;
    public int combo;

    [Header("PhyiscsMaterials")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("State")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;


    private void Awake()
    {
        initSpeed = speed;

        rb = GetComponent<Rigidbody2D>();

        playerAnimation = GetComponent<PlayerAnimation>();

        sr = GetComponent<SpriteRenderer>();

        physicsCheck = GetComponent<PhysicsCheck>();

        coll = GetComponent<CapsuleCollider2D>();

        inputControl = new PlayerInputControl();

        // 跳跃
        inputControl.Gameplay.Jump.started += Jump; // C#绑定回调的方法

        // 自己写的切换走路
        inputControl.Gameplay.Control.performed += CtrlKeydown;
        inputControl.Gameplay.Control.canceled += CtrlKeyup;

        // 攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();

        CheckState();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isAttack)
        {
            Move();
        }
    }

    // 测试
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log(collision);
    //}

    public void Move()
    {
        // Time.deltaTime是 Unity 中的一个重要时间变量。
        // 它表示从上一帧到当前帧所经过的时间，单位是秒（s）。
        // 在游戏运行过程中，帧率是会变化的，例如，当游戏运行流畅时，帧率可能是 60fps（每秒 60 帧），此时Time.deltaTime约为 秒；当游戏出现卡顿，帧率下降到 30fps 时，Time.deltaTime约为 秒。
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        // rb.velocity = new Vector2(inputDirection.x * speed, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;

        // int faceDir = inputDirection.x > 0 ? 1 : -1; // 忽视了0的状态，导致人物默认永远是向左

        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }
        if (inputDirection.x < 0)
        {
            faceDir = -1;
        }

        // 人物翻转1
        transform.localScale = new Vector3(faceDir, 1, 1);

        // 人物翻转2 （看弹幕说会导致UI问题？后续观察一下）
        //sr.flipX = faceDir < 0;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        // Debug.Log("jump");
        // 1.因为老师写的方法是给物体施加一个向上的力，多段跳会让施加的力叠加，需要在每次按下跳跃时重置速度
        // 2.重置速度可以使用xxx.velocity = Vector2.zero; 来实现
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        if (!physicsCheck.isGround) {
            return;
        }
        playerAnimation.PlayAttack();
        isAttack = true;
        combo++;
    }

    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    #endregion

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }

    public void CtrlKeydown(InputAction.CallbackContext context)
    {
        
        speed = (float)(initSpeed * 0.4);
    }

    public void CtrlKeyup(InputAction.CallbackContext context)
    {
        speed = initSpeed;
    }
}
