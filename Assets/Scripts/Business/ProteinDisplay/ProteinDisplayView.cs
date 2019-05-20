using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.UX;
using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZCore;

public class ProteinDisplayView : View {

    private const string AtomSpherePrefebPath = "Model/AtomSphere";


    //内置球模型的原本直径为1单位
    private const float AtomFillSize = 0.25f; // 球模型中原子的实际直径大小(0.25m)
    private const float AtomBallStickSize = 0.06f; //球棍模型中原子的实际直径大小(0.06m)

    //内置圆柱模型的原本高度为2单位 直径为1单位
    private const float BondPrefebLength = 0.1f; //默认棍的预制体长度为0.1m

    private const float BallStickPosScale = 1f / 16f;//球棍模型下Pos要适当缩放 1/16

    [SerializeField]
    private GameObject Displayer3DRoot;

    [SerializeField]
    private GameObject SingleBondPrefeb;

    [SerializeField]
    private GameObject DoubleBondPrefeb;

    [SerializeField]
    private GameObject proteinTemplatePrefeb;

    [SerializeField]
    private GameObject residueTemplatePrefeb;

    private Dictionary<AminoacidType, GameObject> AtomPrefebsDic;

    private GameObject proteinDisplayerGo = null;

    [Header("UIReference")]
    [SerializeField]
    private PolymerInfoDisplayer polymerInfoDisplayer;

    private bool conbineMesh = true;

    #region ImplementedInController

    [ImplementedInController]
    public void OnClickAdjustButton() { }

    [ImplementedInController]
    public void OnClickDoneButton() { }

    [ImplementedInController]
    public void OnClickDingButton() { }

    [ImplementedInController]
    public void OnClickCancelButton() { }

    #endregion

    protected override void OnCreated() {
        base.OnCreated();
        AtomPrefebsDic = new Dictionary<AminoacidType, GameObject>();
    }

    /// <summary>根据残基类型获取原子预制体</summary>
    private GameObject GetAtomPrefeb(AminoacidType aminoacidType) {
        GameObject prefeb = null;
        if (!AtomPrefebsDic.TryGetValue(aminoacidType, out prefeb)) {
            prefeb = Resources.Load<GameObject>(string.Format("{0}/{1}", AtomSpherePrefebPath, aminoacidType.ToString()));
            AtomPrefebsDic.Add(aminoacidType, prefeb);
        }
        return AtomPrefebsDic[aminoacidType];
    }

    /// <summary>根据残基类型获取材质</summary>
    private Material GetMaterial(AminoacidType aminoacidType) {
        return GetAtomPrefeb(aminoacidType).GetComponent<AtomDisplayer>().normal;
    }

    private Material GetHighlightMaterial(AminoacidType aminoacidType) {
        return GetAtomPrefeb(aminoacidType).GetComponent<AtomDisplayer>().highLight;
    }

    private static Vector3 oriOffsetToMainCamera = new Vector3(0, 0, 1.5f);
    /// <summary>根据DisplayMode创建整个蛋白质模型 </summary>
    public void CreateProtein(Protein protein, DisplayMode displayMode) {
        Vector3 initPos = Vector3.zero;
        //若之前已创建Protein 则继承原来的位置 否则在摄像机的前2米初始化
        if (proteinDisplayerGo != null) {
            initPos = proteinDisplayerGo.transform.position;
        }
        else {
            Camera mainCamera = Camera.main;
            initPos = mainCamera.transform.position + oriOffsetToMainCamera.z * mainCamera.transform.forward
            + oriOffsetToMainCamera.y * mainCamera.transform.up + oriOffsetToMainCamera.x * mainCamera.transform.right; 
        }

        proteinDisplayerGo = Instantiate(proteinTemplatePrefeb, Displayer3DRoot.transform, true);
        proteinDisplayerGo.name = protein.ID;
        proteinDisplayerGo.GetComponent<ProteinDisplayer>().Protein = protein;
        foreach (var chainKvp in protein.Chains) {
            Chain chain = chainKvp.Value;
            GameObject chainDisplayerGo = new GameObject(chain.ID, typeof(ChainDisplayer));
            chainDisplayerGo.GetComponent<ChainDisplayer>().Chain = chain;
            chainDisplayerGo.transform.SetParent(proteinDisplayerGo.transform);
            //创建每个原子模型
            foreach (var aminoacidKvp in chain.SeqAminoacids) {
                AminoacidInProtein aminoacidInProtein = aminoacidKvp.Value;
                //GameObject aminoacidDisplayerGo = new GameObject(aminoacidInProtein.ResidueSeq.ToString(), typeof(AminoacidDisplayer));
                GameObject aminoacidDisplayerGo = Instantiate<GameObject>(residueTemplatePrefeb, chainDisplayerGo.transform);
                aminoacidDisplayerGo.name = aminoacidInProtein.ResidueSeq.ToString();
                AminoacidDisplayer aminoacidDisplayer = aminoacidDisplayerGo.GetComponent<AminoacidDisplayer>();
                aminoacidDisplayer.AminoacidInProtein = aminoacidInProtein;

                //棍状模型不显示原子球
                if (displayMode != DisplayMode.Sticks) {
                    foreach (var atomKvp in aminoacidInProtein.AtomInAminoacidSerial) {
                        AtomInAminoacid atomInAminoacid = atomKvp.Key;
                        GameObject atomDisplayerGoPrefeb = GetAtomPrefeb(atomInAminoacid.Aminoacid.Type);
                        switch (displayMode) {
                            case DisplayMode.Spacefill: atomDisplayerGoPrefeb.transform.localScale = new Vector3(AtomFillSize, AtomFillSize, AtomFillSize); break;
                            case DisplayMode.BallStick: atomDisplayerGoPrefeb.transform.localScale = new Vector3(AtomBallStickSize, AtomBallStickSize, AtomBallStickSize); break;
                            default: throw new System.Exception(string.Format("Unhandled displayMode: {0}", displayMode));
                        }
                        GameObject atomDisplayerGo = Instantiate(atomDisplayerGoPrefeb, aminoacidDisplayerGo.transform, false);
                        atomDisplayerGo.name = atomInAminoacid.Name;
                        atomDisplayerGo.transform.localPosition = (aminoacidInProtein.AtomInAminoacidPos[atomInAminoacid] - protein.CenterPos) * BallStickPosScale;
                        AtomDisplayer atomDisplayer = atomDisplayerGo.GetComponent<AtomDisplayer>();
                        atomDisplayer.AtomInAminoacid = atomInAminoacid;
                        atomDisplayer.AminoacidInProtein = aminoacidInProtein;
                    }
                }
                //创建每个键的模型
                if (displayMode == DisplayMode.BallStick || displayMode == DisplayMode.Sticks) {
                    AminoacidInProtein lastAminoacidInProtein = null;
                    //若在该链存在上一序列号的残基 则构造一个该残基的N连通上一残基的C形成的肽键
                    if(aminoacidInProtein.Chain.SeqAminoacids.TryGetValue(aminoacidInProtein.ResidueSeq-1, out lastAminoacidInProtein)) {
                        //两个Bond 两个颜色
                        string name1 = string.Format("{0}-{1}-Peptidebond", lastAminoacidInProtein.ResidueSeq, aminoacidInProtein.ResidueSeq);
                        string name2 = string.Format("{0}-{1}-Peptidebond", aminoacidInProtein.ResidueSeq, lastAminoacidInProtein.ResidueSeq);
                        Vector3 pos1 = (lastAminoacidInProtein.AtomInAminoacidPos[lastAminoacidInProtein.Aminoacid["C"]] - protein.CenterPos) * BallStickPosScale;
                        Vector3 pos3 = (aminoacidInProtein.AtomInAminoacidPos[aminoacidInProtein.Aminoacid["N"]] - protein.CenterPos) * BallStickPosScale;
                        Vector3 pos2 = (pos1 + pos3) / 2;
                        BondType bondType = BondType.Single;
                        Transform parent = aminoacidDisplayerGo.transform;
                        AminoacidType aminoacidType1 = lastAminoacidInProtein.Aminoacid.Type;
                        AminoacidType aminoacidType2 = aminoacidInProtein.Aminoacid.Type;
                        GenerateBondGameObject(name1, pos1, pos2, bondType, parent, aminoacidType1,displayMode);
                        GenerateBondGameObject(name2, pos2, pos3, bondType, parent, aminoacidType2,displayMode);
                    }
                    //氨基酸内的化学键(从本地Aminoacid数据中获取的连接关系)
                    foreach (var connection in aminoacidInProtein.Aminoacid.Connections) {
                        string name = string.Format("{0}-{1}-bond", connection.Key.Key.Name, connection.Key.Value.Name);
                        Vector3 pos1 = Vector3.zero; Vector3 pos2 = Vector3.zero;
                        //若读取的pdb文件中的某残基中没有连接关系中的某个原子 说明源pdb文件出现数据遗漏
                        try {
                            pos1 = (aminoacidInProtein.AtomInAminoacidPos[connection.Key.Key] - protein.CenterPos) * BallStickPosScale;
                            pos2 = (aminoacidInProtein.AtomInAminoacidPos[connection.Key.Value] - protein.CenterPos) * BallStickPosScale;
                        }
                        catch(KeyNotFoundException ex) {
                            //Debug.LogWarning(string.Format("The aminoacidInProtein: {0} is not complete, the atom in connection: {1}-{2} is losed", aminoacidInProtein, connection.Key.Key.Name, connection.Key.Value.Name));
                            continue;
                        }
                        BondType bondType = connection.Value;
                        Transform parent = aminoacidDisplayerGo.transform;
                        AminoacidType aminoacidType = aminoacidInProtein.Aminoacid.Type;
                        GenerateBondGameObject(name, pos1, pos2, bondType, parent, aminoacidType,displayMode);
                    }
                }
                //若合并Mesh 在Residue的mesh合并渲染 所有子物体不渲染
                if (conbineMesh) {
                    MeshFilter residueMeshFilter = aminoacidDisplayerGo.AddComponent<MeshFilter>();
                    MeshRenderer residueMeshRenderer = aminoacidDisplayerGo.AddComponent<MeshRenderer>();
                    residueMeshFilter.sharedMesh = aminoacidDisplayerGo.transform.CombineChildsMesh();
                    residueMeshRenderer.sharedMaterial = GetMaterial(aminoacidInProtein.Aminoacid.Type);
                    MeshRenderer[] childMeshRenderers = aminoacidDisplayerGo.GetComponentsInChildren<MeshRenderer>();
                    foreach(var renderer in childMeshRenderers) {
                        renderer.enabled = false;
                    }
                    residueMeshRenderer.enabled = true;
                    aminoacidDisplayer.Normal = GetMaterial(aminoacidInProtein.Aminoacid.Type);
                    aminoacidDisplayer.HighLight = GetHighlightMaterial(aminoacidInProtein.Aminoacid.Type);
                }
            }
        }
        proteinDisplayerGo.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); //缩放0.2倍
        proteinDisplayerGo.transform.position = initPos;
    }

    [ContextMenu("ActivateBoundingBoxRig")]
    public void ActivateBoundingBoxRig() {
        BoundingBoxRig boundingBoxRig = proteinDisplayerGo.GetComponent<BoundingBoxRig>();
        boundingBoxRig.Activate();
    }

    public void SetBoardInfo(AtomDisplayer atomDisplayer) {
        polymerInfoDisplayer.SetData(atomDisplayer);
    }

    public void SetBoardInfo(AminoacidDisplayer aminoacidDisplayer) {
        polymerInfoDisplayer.SetData(aminoacidDisplayer);
    }

    public void SetBoardInfo(ChainDisplayer chainDisplayer) {
        polymerInfoDisplayer.SetData(chainDisplayer);
    }

    public void SetBoardActive(bool active) {
        polymerInfoDisplayer.gameObject.Active(active);
    }

    public void ClearBoardInfo() {
        polymerInfoDisplayer.ClearData();
    }

    public void StartAdjustProtein() {
        if (proteinDisplayerGo == null)
            return;
        BoundingBoxRig boundingBoxRig = proteinDisplayerGo.GetComponent<BoundingBoxRig>();
        boundingBoxRig.Activate();
        proteinDisplayerGo.GetComponent<HandDraggable>().enabled = true;
    }

    public void CompleteAdjustProtein() {
        if (proteinDisplayerGo == null)
            return;
        BoundingBoxRig boundingBoxRig = proteinDisplayerGo.GetComponent<BoundingBoxRig>();
        boundingBoxRig.Deactivate();
        proteinDisplayerGo.GetComponent<HandDraggable>().enabled = false;
    }

    public void DingPolymerInfoDisplayer() {
        SolverHandler solverHandler = polymerInfoDisplayer.GetComponent<SolverHandler>();
        solverHandler.UpdateSolvers = false;
    }

    public void CancelDingPolymerInfoDisplayer() {
        SolverHandler solverHandler = polymerInfoDisplayer.GetComponent<SolverHandler>();
        solverHandler.UpdateSolvers = true;
    }

    /// <summary>销毁创建的蛋白质模型</summary>
    public void DestroyProtein() {
        if (proteinDisplayerGo != null) {
            Destroy(proteinDisplayerGo);
        }
    }

    //这里的position是真正坐标而不是pdb文件中读取的坐标
    /// <summary>创建键</summary>
    private GameObject GenerateBondGameObject(string name, Vector3 pos1, Vector3 pos2, BondType bondType, Transform parent, AminoacidType aminoacidType, DisplayMode displayMode) {
        GameObject bondGo = null;
        Material material = GetMaterial(aminoacidType);
        Material hightLightMaterial = GetHighlightMaterial(aminoacidType);
        float bondLength = (pos1 - pos2).magnitude;//键长
        Vector3 bondDirection = (pos1 - pos2).normalized; //键方向

        //Sticks模式只创建单键
        if (bondType == BondType.Single || displayMode == DisplayMode.Sticks) {
            bondGo = Instantiate(SingleBondPrefeb, parent, false);
        }
        else if (bondType == BondType.Double) {
            bondGo = Instantiate(DoubleBondPrefeb, parent, false);
        }
        else throw new System.Exception("Unhandled bondType :" + bondType);

        BondDisplayer bondDisplayer = bondGo.GetComponent<BondDisplayer>();
        bondDisplayer.BondType = bondType;
        bondDisplayer.normal = material;
        bondDisplayer.highLight = hightLightMaterial;

        bondGo.name = name;
        foreach (Transform child in bondGo.transform) {
            child.GetComponent<Renderer>().material = material;
        }
        bondGo.transform.localPosition = (pos1 + pos2) / 2;
        bondGo.transform.localScale = new Vector3(0.01f, 0.01f, bondLength / 2);
        Vector3 targetPos = Vector3.Cross(bondDirection, Vector3.up) + (pos1 + pos2) / 2;
        bondGo.transform.LookAt(pos1);
        return bondGo;
    }

}
