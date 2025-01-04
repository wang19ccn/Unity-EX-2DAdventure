using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;

    [Header("Check Params")]
    public bool manual;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRaduis;
    public LayerMask groundLayer;

    [Header("State")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;


    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if (!manual)
        {
            rightOffset = new Vector2(coll.offset.x + coll.bounds.size.x / 2, coll.offset.y);
            leftOffset = new Vector2(coll.offset.x - coll.bounds.size.x / 2, coll.offset.y);
        }
    }

    private void Update()
    { 
        if (!manual)
        {
            // �� transform.localScale.x ����������ת���������
            rightOffset = new Vector2(coll.offset.x * transform.localScale.x + coll.bounds.size.x / 2, coll.offset.y);
            leftOffset = new Vector2(coll.offset.x * transform.localScale.x - coll.bounds.size.x / 2, coll.offset.y);
        }
        Check();
    }

    public void Check()
    {
        // ������ �� transform.localScale�������� ����������ת���������
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset * transform.localScale, checkRaduis, groundLayer);

        // ǽ���ж�
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        // ��Ļ�и��뷨�������̫�ã�Ӧ������Բ�ģ���վ����Ӧ��Ҳ����
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset * transform.localScale, checkRaduis);

        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
