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

        // ��Ծ
        inputControl.Gameplay.Jump.started += Jump; // C#�󶨻ص��ķ���

        // �Լ�д���л���·
        inputControl.Gameplay.Control.performed += CtrlKeydown;
        inputControl.Gameplay.Control.canceled += CtrlKeyup;

        // ����
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

    // ����
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log(collision);
    //}

    public void Move()
    {
        // Time.deltaTime�� Unity �е�һ����Ҫʱ�������
        // ����ʾ����һ֡����ǰ֡��������ʱ�䣬��λ���루s����
        // ����Ϸ���й����У�֡���ǻ�仯�ģ����磬����Ϸ��������ʱ��֡�ʿ����� 60fps��ÿ�� 60 ֡������ʱTime.deltaTimeԼΪ �룻����Ϸ���ֿ��٣�֡���½��� 30fps ʱ��Time.deltaTimeԼΪ �롣
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        // rb.velocity = new Vector2(inputDirection.x * speed, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;

        // int faceDir = inputDirection.x > 0 ? 1 : -1; // ������0��״̬����������Ĭ����Զ������

        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }
        if (inputDirection.x < 0)
        {
            faceDir = -1;
        }

        // ���﷭ת1
        transform.localScale = new Vector3(faceDir, 1, 1);

        // ���﷭ת2 ������Ļ˵�ᵼ��UI���⣿�����۲�һ�£�
        //sr.flipX = faceDir < 0;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        // Debug.Log("jump");
        // 1.��Ϊ��ʦд�ķ����Ǹ�����ʩ��һ�����ϵ��������������ʩ�ӵ������ӣ���Ҫ��ÿ�ΰ�����Ծʱ�����ٶ�
        // 2.�����ٶȿ���ʹ��xxx.velocity = Vector2.zero; ��ʵ��
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
