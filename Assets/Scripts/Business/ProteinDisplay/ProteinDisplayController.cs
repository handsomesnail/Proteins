using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class ProteinDisplayController : Controller {

    /// <summary>显示蛋白质</summary>
    public void ShowProtein() {
        ProteinDisplayModel model = GetModel<ProteinDisplayModel>();
        Protein protein = CoreAPI.PostCommand<PdbLoaderModule, GetProteinDataCommand, Protein>(new GetProteinDataCommand());
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

    public void ShowDisplayView() {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
    }

    public void OnSliderChanged(float value) {
        Debug.Log(value);
    }

    public void OnBallStickToggleChanged(bool value) {
        ProteinDisplayModel model = GetModel<ProteinDisplayModel>();
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        Protein protein = model.DisplayedProteinData;
        view.displayMode = value ? DisplayMode.BallStick : DisplayMode.Spacefill;
        DestroyProtein();
        view.ShowProtein(protein);
    }

    public void ShowInfoInBoard(AtomDisplayer atomDisplayer) {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        view.SetBoardInfo(atomDisplayer);
    }

    /// <summary>销毁蛋白质分子模型</summary>
    private void DestroyProtein() {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        view.DestroyProtein();
    }



}
