﻿using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class AtomDisplayer : MonoBehaviour {

    /// <summary>该原子位于蛋白质中的哪个氨基酸</summary>
    public AminoacidInProtein AminoacidInProtein {
        get; set;
    }

    /// <summary>该原子位于氨基酸中的哪个原子</summary>
    public AtomInAminoacid AtomInAminoacid {
        get; set;
    }

    private void OnMouseEnter() {
        CoreAPI.SendCommand<ProteinDisplayModule, ShowInfoInBoardCommand>(new ShowInfoInBoardCommand(this));
    }

    private void OnMouseExit() {
        CoreAPI.SendCommand<ProteinDisplayModule, ShowInfoInBoardCommand>(new ShowInfoInBoardCommand(null));
    }


}