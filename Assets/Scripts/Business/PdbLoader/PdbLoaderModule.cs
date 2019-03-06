using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;
using PolymerModel.Data;

public class PdbLoaderModule : Module {

    /// <summary>读取本地pdb文件 </summary>
    public void OnLoadLocalPdbFileCommand(LoadLocalPdbFileCommand cmd) {
        GetController<PdbLoaderController>().LoadLocalPdbFile();
    }

    /// <summary>获取当前读取的蛋白质数据</summary>
    public Protein OnGetProteinDataCommand(GetProteinDataCommand cmd) {
        return GetController<PdbLoaderController>().GetProteinData();
    }

}
