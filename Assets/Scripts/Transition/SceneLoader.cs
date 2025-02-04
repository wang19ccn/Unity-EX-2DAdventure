using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("�¼�����")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;

    [SerializeField] private GameSceneSO currentLoadScene; // ���л�����չʾ�ڽ���
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;

    public float fadeDuration;

    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadScene = firstLoadScene; // currentLoadSceneֻ�ǻ��ֵ������ firstLoadScene �ĵ�ַ

        currentLoadScene = firstLoadScene;
        currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void OnLoadRequestEvent(GameSceneSO localtionToLoad, Vector3 posToGo, bool fadeScreen)
    {
        sceneToLoad = localtionToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if (currentLoadScene)
        {
            StartCoroutine(UnLoadPreviousScene());
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

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
    }
}
