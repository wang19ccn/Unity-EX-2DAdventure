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

    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;

    [Header("广播")]
    public VoidEventSO afterSceneLoadedEvent;

    [SerializeField] private GameSceneSO currentLoadScene; // 序列化将其展示在界面
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen; // 渐入渐出
    private bool isLoading;

    public float fadeDuration;

    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadScene = firstLoadScene; // currentLoadScene只是获得值，而非 firstLoadScene 的地址

        //currentLoadScene = firstLoadScene;
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    // TODO: 做完MainMenu再改
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
    /// 场景加载事件请求
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
            // TODO: 实现渐入渐出
        }

        yield return new WaitForSeconds(fadeDuration);
        yield return currentLoadScene.sceneReference.UnLoadScene();

        // 关闭人物
        playerTrans.gameObject.SetActive(false);

        // 加载新场景
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted; // 场景加载完成之后
    }

    /// <summary>
    /// 场景加载完成后
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

        // 场景加载完成后的事件
        afterSceneLoadedEvent.RaiseEvent();
    }
}
