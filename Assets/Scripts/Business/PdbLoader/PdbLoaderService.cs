using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using ZCore;

public class PdbLoaderService : Service {

    public const string DownloadUrl = "https://files.rcsb.org/download";

    public const string DefaultFilePath = "Pdb";

    public IEnumerator DownloadPdbFile(string IDCode, Action<HttpResponse> completeCallback, Action<float> progressCallback) {
        yield return Get(string.Format("{0}/{1}.pdb", DownloadUrl, IDCode), completeCallback, progressCallback);
    }

    public IEnumerator LoadDefaultPdbFile(string IDCode, Action<string> completeCallback) {
        ResourceRequest request = Resources.LoadAsync<TextAsset>(string.Format("{0}/{1}", DefaultFilePath, IDCode));
        yield return request;
        TextAsset textAsset = request.asset as TextAsset;
        string str = textAsset.text;
        completeCallback?.Invoke(str);
    }



}
