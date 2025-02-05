using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Sign : MonoBehaviour
{
    private PlayerInputControl playerInput;
    private Animator anim;
    public Transform playerTrans;
    public GameObject signSprite;
    private IInteractable targetItem;
    private bool canPress;
    private String currentInput;

    private void Awake()
    {
        //anim = GetComponentInChildren<Animator>(); // 一开始子物体是关闭的
        anim = signSprite.GetComponent<Animator>();

        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.Gameplay.Confirm.started += OnConfirm;
    }

    private void OnDisable()
    {
        canPress = false;
    }

    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = playerTrans.localScale;
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    /// <summary>
    /// 切换设备同时切换动画
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="actionChange"></param>
    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if(actionChange == InputActionChange.ActionStarted)
        {
            //Debug.Log(((InputAction)obj).activeControl.device);

            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    currentInput = "keyboard";
                    anim.Play("keyboard");
                    break;
                case XInputController:
                    currentInput = "xbox";
                    anim.Play("xbox");
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = collision.GetComponent<IInteractable>();

            switch (currentInput)
            {
                case "keyboard":
                    anim.Play("keyboard");
                    break;
                case "xbox":
                    anim.Play("xbox");
                    break;
            }
        } 
        //else // 这里添加会导致进入场景之后还在传送点的时候没有按键提示
        //{
        //    canPress = false;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }
}
