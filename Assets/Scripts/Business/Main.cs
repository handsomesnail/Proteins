using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using Util;
using System;
using System.Reflection;
using UnityEngine.UI;
using ZCore;
using PolymerModel;
using PolymerModel.Data;

public class Main : MonoBehaviour {

	void Start () {
        LoadAsync();
    }
	
	void Update () {
		
	}

    public async void LoadAsync() {
        await PolymerModelAPI.LoadDataAsync();
        Debug.Log("加载数据完成");
    }

}



