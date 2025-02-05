using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Transform playerTrans;
    public Vector3 firstPosition;

    [Header("�¼�����")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;

    [Header("�㲥")]
    public VoidEventSO afterSceneLoadedEvent;

    [SerializeField] private GameSceneSO currentLoadScene; // ���л�����չʾ�ڽ���
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen; // ���뽥��
    private bool isLoading;

    public float fadeDuration;

    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadScene = firstLoadScene; // currentLoadSceneֻ�ǻ��ֵ������ firstLoadScene �ĵ�ַ

        //currentLoadScene = firstLoadScene;
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    // TODO: ����MainMenu�ٸ�
    private void Start()
    {
        NewGame();
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        OnLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    /// <summary>
    /// ���������¼�����
    /// </summary>
    /// <param name="localtionToLoad"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScreen"></param>
    private void OnLoadRequestEvent(GameSceneSO localtionToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading) return;
        isLoading = true;

        sceneToLoad = localtionToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
         }
        else
        {
            LoadNewScene(); 
        }
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            // TODO: ʵ�ֽ��뽥��
        }

        yield return new WaitForSeconds(fadeDuration);
        yield return currentLoadScene.sceneReference.UnLoadScene();

        // �ر�����
        playerTrans.gameObject.SetActive(false);

        // �����³���
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted; // �����������֮��
    }

    /// <summary>
    /// ����������ɺ�
    /// </summary>
    /// <param name="handle"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadScene = sceneToLoad;

        playerTrans.position = positionToGo;

        playerTrans.gameObject.SetActive(true);

        if (fadeScreen)
        {
            // TODO
        }

        isLoading = false;

        // ����������ɺ���¼�
        afterSceneLoadedEvent.RaiseEvent();
    }
}
