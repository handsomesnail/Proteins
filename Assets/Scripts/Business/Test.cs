using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    [ContextMenu("TestMethod")]
    public async void TestMethod() {
        ZCore.CoreAPI.SendCommand<PdbLoaderModule>(new LoadLocalPdbFileCommand());
    }

}
