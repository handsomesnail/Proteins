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


public class Test : MonoBehaviour {

    private DataTable aminoacidDt;
    private DataTable connectionDt;

	void Start () {
        //TestMethod();
    }
	
	void Update () {
		
	}

    [ContextMenu("TestMethod")]
    public async void TestMethod() {
        
    }

    public async void LoadData() {
        string aminoacidData = await IOUtil.ReadInStreamingAssetsAsync(string.Empty, "Aminoacid.csv");
        aminoacidDt = CSVHelper.ReadCSVStr(aminoacidData);
        string connectionData = await IOUtil.ReadInStreamingAssetsAsync(string.Empty, "Connection.csv");
        connectionDt = CSVHelper.ReadCSVStr(connectionData);

    }

}



