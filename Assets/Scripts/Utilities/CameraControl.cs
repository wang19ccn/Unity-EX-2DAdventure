using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO afterSceneLoadedEvent;

    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;
    public VoidEventSO cameraShakeEvent;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        GetNewCameraBounds();
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    // 首先Awake方法通常用于初始化变量、其次Awake()和Start()他们两个区别是wake在【脚本启用或未启用后】均会调用,Start【只会在脚本启用后】调用， Awake在Start前调用
    // 前面的问的为什么 不自动获取bounds？你问的意思应该是 为什么调用GetNewCameraBounds(); 不放在Awake里面？然后觉得Awake和start一样都可以调用 为什么写两个是吧？
    // 并不是像你所想为什么不能偷懒调用GetNewCameraBounds()也放在Awake里面，首先是书写习惯，其次放在Start是使用该脚本时才调用该方法，Awake则是没使用到该脚本就调用了。。。
    // TODO: 场景切换后更改
    //private void Start()
    //{
    //    GetNewCameraBounds();
    //}

    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");

        if (obj == null) return;

        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        confiner2D.InvalidateCache();
    }
}
