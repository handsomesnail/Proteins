using PolymerModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Util;
using System.Reflection;
using ZCore;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using Windows.Storage;
using Windows.UI.Popups;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.Storage.Pickers;
#endif

public class App : MonoBehaviour {

    public static App Instance {
        get; private set;
    }

    public Transform GameObjectPoolRoot {
        get; private set;
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public Text ErrorText;

    private void Start() {
        if(GameObjectPoolRoot == null) {
            GameObject go = new GameObject("GameObjectPoolRoot");
            GameObjectPoolRoot = go.transform;
            GameObjectPoolRoot.SetParent(this.gameObject.transform);
        }
        Application.targetFrameRate = 60;
        Init(); 
    }


    private async void Init() {
        await PolymerModelAPI.LoadDataAsync();
        Debug.Log("加载PolymerModel完成");
        CoreAPI.SendCommand<MainConsoleModule, RegisterHoldHandlerCommand>(new RegisterHoldHandlerCommand());
        CoreAPI.SendCommand<MainConsoleModule, ShowMainConsoleCommand>(new ShowMainConsoleCommand());
        StartCoroutine(LaunchHelpDialog()); 
    }

    private IEnumerator LaunchHelpDialog() {
        yield return new WaitForSeconds(1.0f); //等待MainConsole载入完成
        CoreAPI.SendCommand<MainConsoleModule, ShowHelpDialogCommand>(new ShowHelpDialogCommand());
    }

    [ContextMenu("Test")]
    public void TestCombine() {
        CoreAPI.SendCommand<PdbLoaderModule, LoadNetworkPdbFileCommand>(new LoadNetworkPdbFileCommand("2nc3", () => {
            Debug.Log("ok");
        }));
    }


}
