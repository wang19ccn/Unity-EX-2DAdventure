using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// 听到此处讲到事件  和  前面学的新的InputSyetem按键输入系统中 +=委托(方法) 之间感觉迷糊的同学，可以暂停 查一下 事件 和 委托的区别，

public class Character : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO newGameEvent;

    [Header("Base Params")]
    public float maxHealth;
    public float currentHealth;

    [Header("受伤无敌")]
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
            // 死亡、更新血量
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

            // 执行受伤
            OnTakeDamage?.Invoke(attacker.transform);
            // 前面学到的InputSystem新输入系统，用的是委托也就是+=方法、这里用的是事件、事件基于委托。感兴趣的搜一下委托事件区别，反正也是面试题。这里用事件的好处是想调用哪些方法操作放在unity界面了
            // 总的来说事件是对委托的封装，【事件】这里执行受伤只用一行代码调用其余方法: 人物反弹、动画、音乐、ui显示等方法，而具体则在unity里方便加减待调用方法，如用【委托】那就要这里用好几行代码是写死的哦

            TriggerInvulnerable();
        }
        else
        {
            currentHealth = 0;
            // 触发死亡
            OnDie?.Invoke();
        }

        OnHealthChange?.Invoke(this);
    }

    /// <summary>
    /// 触发受伤无敌
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
