using PolymerModel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class PdbLoaderCommand : Command { }

public class LoadLocalPdbFileCommand : PdbLoaderCommand {
    public Action<string> CompleteCallback { get; private set; }

    public LoadLocalPdbFileCommand(Action<string> completeCallback) {
        this.CompleteCallback = completeCallback;
    }
}

public class LoadNetworkPdbFileCommand : PdbLoaderCommand {
    public string IDCode { get; private set; }

    public Action CompleteCallback { get; private set; }

    public LoadNetworkPdbFileCommand(string idCode, Action completeCallback) {
        this.IDCode = idCode;
        this.CompleteCallback = completeCallback;
    }
}

public class GetProteinDataCommand : PdbLoaderCommand { }

public class LoadDefaultPdbFileCommand : PdbLoaderCommand {

    public string IDCode { get; private set; }

    public Action CompleteCallback { get; private set; }

    public LoadDefaultPdbFileCommand(string idCode, Action completeCallback) {
        this.IDCode = idCode;
        this.CompleteCallback = completeCallback;
    }
}


