using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Test : MonoBehaviour {

	void Start () {
        StartCoroutine(TestAsync());
	}
	
	void Update () {

	}

    public GameObject _GameObject;

    [ContextMenu("TestMethod")]
    public async void TestMethod() {
        UnityEngine.Object a = null;
        GameObject b = a as GameObject;
    }

    public IEnumerator TestAsync() {
        Task<string> task = ReadAsync("xxx");
        yield return new WaitUntil(() => {
            return task.IsCompleted;
        });
        string result = task.Result;
        yield return LoadResourceAsync(result);
        Debug.Log("执行完成");
    }

    public async void TestAsync2() {
        string result = await ReadAsync("xxx");
    }

    public async Task<string> ReadAsync(string path) {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
            using (StreamReader sw = new StreamReader(fs, Encoding.UTF8)) {
                return await sw.ReadToEndAsync();
            }
        }
    }

    public IEnumerator LoadResourceAsync(string path) {
        yield return Resources.LoadAsync<GameObject>(path);
    }

}

