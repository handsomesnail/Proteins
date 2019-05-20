using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class ProteinDisplayController : Controller {

    #region ImplementedInController
    public void OnClickAdjustButton() {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        view.StartAdjustProtein();
        CoreAPI.SendCommand<MainConsoleModule, UnRegisterHoldHandlerCommand>(new UnRegisterHoldHandlerCommand());
    }

    public void OnClickDoneButton() {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        view.CompleteAdjustProtein();
        CoreAPI.SendCommand<MainConsoleModule, RegisterHoldHandlerCommand>(new RegisterHoldHandlerCommand());
    }

    public void OnClickDingButton() {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        view.DingPolymerInfoDisplayer();
    }

    public void OnClickCancelButton() {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        view.CancelDingPolymerInfoDisplayer();
    }

    #endregion

    #region Public Methods

    /// <summary>显示蛋白质</summary>
    public void ShowProtein() {
        ProteinDisplayModel model = GetModel<ProteinDisplayModel>();
        Protein protein = CoreAPI.PostCommand<PdbLoaderModule, GetProteinDataCommand, Protein>(new GetProteinDataCommand());
        DisplayMode displayMode = CoreAPI.PostCommand<MainConsoleModule, GetDisplayModeCommand, DisplayMode>(new GetDisplayModeCommand());
        //若蛋白质数据还未加载 则直接返回
        if(protein == null) {
            return;
        }
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

    public void SetSelectedDisplayer(IDisplayerSelected displayer) {
        ProteinDisplayModel model = GetModel<ProteinDisplayModel>();
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        //若是同一个displayer则跳过
        if(model.SelectedDisplayer !=null && model.SelectedDisplayer == displayer) {
            return;
        }
        //取消上一个Displayer的选中状态
        if (model.SelectedDisplayer != null) {
            model.SelectedDisplayer.OnUnSelected();
        }
        model.SelectedDisplayer = displayer;
        //设置当前选中的Displayer的选中状态
        displayer.OnSelected();
        //更新BoardInfo
        if (displayer is AtomDisplayer) {
            view.SetBoardInfo(displayer as AtomDisplayer);
        }
        else if (displayer is AminoacidDisplayer) {
            view.SetBoardInfo(displayer as AminoacidDisplayer);
        }
        else if (displayer is ChainDisplayer) {
            view.SetBoardInfo(displayer as ChainDisplayer);
        }
        else throw new System.Exception();
    }

    public IDisplayerSelected GetSelectedDisplayer() {
        ProteinDisplayModel model = GetModel<ProteinDisplayModel>();
        return model.SelectedDisplayer;
    }

    public void SetPolymerInfoDisplayerActive(bool active) {
        ProteinDisplayView view = GetView<ProteinDisplayView>();
        ProteinDisplayModel model = GetModel<ProteinDisplayModel>();
        if(model.DisplayedProteinData == null) {
            view.SetBoardActive(false);
            return;
        }
        view.SetBoardActive(active);
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
