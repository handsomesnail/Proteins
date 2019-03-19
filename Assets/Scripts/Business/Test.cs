using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using ContextMenu = UnityEngine.ContextMenu;

public class A {
    public void TestMethod() {
        int a = 0;
        int b = 1;
        int c = a + b;
    }

}

public class Test : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {

	}

    public GameObject _GameObject;

    [ContextMenu("TestMethod")]
    public async void TestMethod() {
        Type type = typeof(void);
        Debug.Log(type.FullName);
    }
    

}
