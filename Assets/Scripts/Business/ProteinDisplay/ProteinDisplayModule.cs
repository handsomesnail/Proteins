using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class ProteinDisplayModule : Module {

    public void OnShowProteinCommand(ShowProteinCommand cmd) {
        GetController<ProteinDisplayController>().ShowProtein();
    }

}
