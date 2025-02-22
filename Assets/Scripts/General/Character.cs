using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// �����˴������¼�  ��  ǰ��ѧ���µ�InputSyetem��������ϵͳ�� +=ί��(����) ֮��о��Ժ���ͬѧ��������ͣ ��һ�� �¼� �� ί�е�����

public class Character : MonoBehaviour
{
    [Header("�¼�����")]
    public VoidEventSO newGameEvent;

    [Header("Base Params")]
    public float maxHealth;
    public float currentHealth;

    [Header("�����޵�")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage; 
    public UnityEvent OnDie;


    private void NewGame()
    {
        currentHealth = maxHealth;

        OnHealthChange?.Invoke(this);
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
    }

    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            // ����������Ѫ��
            currentHealth = 0;
            OnHealthChange?.Invoke(this);
            OnDie?.Invoke();
        }
    }

    public void TakeDamage(Attack attacker)
    {
        if (invulnerable) return;

        // Debug.Log(attacker.damage);
        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;

            // ִ������
            OnTakeDamage?.Invoke(attacker.transform);
            // ǰ��ѧ����InputSystem������ϵͳ���õ���ί��Ҳ����+=�����������õ����¼����¼�����ί�С�����Ȥ����һ��ί���¼����𣬷���Ҳ�������⡣�������¼��ĺô����������Щ������������unity������
            // �ܵ���˵�¼��Ƕ�ί�еķ�װ�����¼�������ִ������ֻ��һ�д���������෽��: ���ﷴ�������������֡�ui��ʾ�ȷ���������������unity�﷽��Ӽ������÷��������á�ί�С��Ǿ�Ҫ�����úü��д�����д����Ŷ

            TriggerInvulnerable();
        }
        else
        {
            currentHealth = 0;
            // ��������
            OnDie?.Invoke();
        }

        OnHealthChange?.Invoke(this);
    }

    /// <summary>
    /// ���������޵�
    /// </summary>
    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
