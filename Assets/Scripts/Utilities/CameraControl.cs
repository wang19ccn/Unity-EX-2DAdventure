using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("�¼�����")]
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

    // ����Awake����ͨ�����ڳ�ʼ�����������Awake()��Start()��������������wake�ڡ��ű����û�δ���ú󡿾������,Start��ֻ���ڽű����ú󡿵��ã� Awake��Startǰ����
    // ǰ����ʵ�Ϊʲô ���Զ���ȡbounds�����ʵ���˼Ӧ���� Ϊʲô����GetNewCameraBounds(); ������Awake���棿Ȼ�����Awake��startһ�������Ե��� Ϊʲôд�����ǰɣ�
    // ��������������Ϊʲô����͵������GetNewCameraBounds()Ҳ����Awake���棬��������дϰ�ߣ���η���Start��ʹ�øýű�ʱ�ŵ��ø÷�����Awake����ûʹ�õ��ýű��͵����ˡ�����
    // TODO: �����л������
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
