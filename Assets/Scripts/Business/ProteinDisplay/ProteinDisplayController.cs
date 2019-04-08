using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class ProteinDisplayController : Controller {

    #region Public Methods

    /// <summary>显示蛋白质</summary>
    public void ShowProtein() {
        ProteinDisplayModel model = GetModel<ProteinDisplayModel>();
        Protein protein = CoreAPI.PostCommand<PdbLoaderModule, GetProteinDataCommand, Protein>(new GetProteinDataCommand());
        DisplayMode displayMode = CoreAPI.PostCommand<MainConsoleModule, GetDisplayModeCommand, DisplayMode>(new GetDisplayModeCommand());
        //若当前ProteinData与DisplayMode均是最新则直接返回，否则销毁重新Create
        if (model.DisplayedProteinData != null && model.DisplayedProteinData.ID == protein.ID && model.DisplayedDisplayMode == displayMode) {
            return;
        }
        else {
            DestroyProtein();
        }
        model.DisplayedProteinData = protein;
        model.DisplayedDisplayMode = displayMode;
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        view.CreateProtein(protein, displayMode);
    }

    public void ShowDisplayView() {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
    }

    public void OnBallStickToggleChanged(bool value) {
        //ProteinDisplayModel model = GetModel<ProteinDisplayModel>();
        //ProteinDisplayView view = GetView<ProteinDisplayView>();
        //Protein protein = model.DisplayedProteinData;
        //view.displayMode = value ? DisplayMode.BallStick : DisplayMode.Spacefill;
        //DestroyProtein();
        //view.CreateProtein(protein);
    }

    public void ShowInfoInBoard(AtomDisplayer atomDisplayer) {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        view.SetBoardInfo(atomDisplayer);
    }

    public void SetDisplayMode(DisplayMode displayMode) {
        ProteinDisplayModel model = GetModel<ProteinDisplayModel>();
        ProteinDisplayView view = GetView<ProteinDisplayView>();
    }

    #endregion

    #region Private/Protected Methods

    /// <summary>销毁蛋白质分子模型</summary>
    private void DestroyProtein() {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        view.DestroyProtein();
    }

    #endregion


}
