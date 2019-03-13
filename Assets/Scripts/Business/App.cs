using PolymerModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

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

    private void Start() {
        if(GameObjectPoolRoot == null) {
            GameObject go = new GameObject("GameObjectPoolRoot");
            GameObjectPoolRoot = go.transform;
            GameObjectPoolRoot.SetParent(this.gameObject.transform);
        }
        LoadData();
    }

    private async void LoadData() {
        await PolymerModelAPI.LoadDataAsync();
        Debug.Log("加载数据完成");
    }

}
