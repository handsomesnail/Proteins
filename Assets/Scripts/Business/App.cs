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
        LoadData();
    }


    private async void LoadData() {
        await PolymerModelAPI.LoadDataAsync();
        Debug.Log("加载PolymerModel完成");
        //CoreAPI.SendCommand<MainConsoleModule, RegisterHoldHandlerCommand>(new RegisterHoldHandlerCommand());
        //CoreAPI.SendCommand<MainConsoleModule, ShowMainConsoleCommand>(new ShowMainConsoleCommand());
        //OnButtonClick();
    }

    public async void OnButtonClick() {
        CoreAPI.SendCommand<PdbLoaderModule, LoadDefaultPdbFileCommand>(new LoadDefaultPdbFileCommand("6dce", () => {
            CoreAPI.SendCommand<ProteinDisplayModule, ShowProteinCommand>(new ShowProteinCommand());
        }));
    }


}
