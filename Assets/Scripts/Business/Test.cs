using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Test : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {

	}

    public GameObject _GameObject;

    [ContextMenu("TestMethod")]
    public async void TestMethod() {
        ZCore.CoreAPI.SendCommand<PdbLoaderModule>(new LoadLocalPdbFileCommand(() => {
            ZCore.CoreAPI.SendCommand<ProteinDisplayModule>(new ShowProteinCommand());
        }));
        //ZCore.CoreAPI.SendCommand<PdbLoaderModule>(new LoadNetworkPdbFileCommand("4hhb"));

        //Vector3 pos1 = new Vector3(0.03f, 0.04f, 0.05f);
        //Vector3 pos2 = Vector3.zero;
        //float bondLength = (pos1 - pos2).magnitude;//键长
        //Vector3 bondDirection = (pos1 - pos2).normalized; //键方向
        //_GameObject.transform.localPosition = (pos1 + pos2) / 2;
        //_GameObject.transform.localScale = new Vector3(0.01f, bondLength / 2, 0.01f);
        //Vector3 targetPos = Vector3.Cross(bondDirection, Vector3.up) + (pos1 + pos2) / 2;
        //_GameObject.transform.LookAt(targetPos);


    }
    

}
