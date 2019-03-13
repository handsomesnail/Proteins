using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class ProteinDisplayController : Controller {

    /// <summary>显示蛋白质</summary>
    public void ShowProtein() {
        ProteinDisplayModel model = GetModel<ProteinDisplayModel>();
        Protein protein = CoreAPI.PostCommand<PdbLoaderModule>(new GetProteinDataCommand()) as Protein;
        if (model.DisplayedProteinData != null) {
            if (model.DisplayedProteinData.ID == protein.ID) {
                return;
            }
            else DestroyProtein();
        }
        model.DisplayedProteinData = protein;
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        view.ShowProtein(protein);
    }

    /// <summary>销毁蛋白质分子模型</summary>
    private void DestroyProtein() {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        view.DestroyProtein();
    }

}
