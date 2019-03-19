using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZCore;

public class ProteinDisplayView : View {

    private const string AtomSpherePrefebPath = "Model/AtomSphere";

    public DisplayMode displayMode;

    //内置球模型的原本直径为1单位
    private const float AtomFillSize = 0.25f; // 球模型中原子的实际直径大小(0.25m)
    private const float AtomBallStickSize = 0.06f; //球棍模型中原子的实际直径大小(0.06m)

    //内置圆柱模型的原本高度为2单位 直径为1单位
    private const float BondPrefebLength = 0.1f; //默认棍的预制体长度为0.1m

    private const float BallStickPosScale = 1f / 16f;

    [SerializeField]
    private GameObject Canvas2D;

    [SerializeField]
    private GameObject Displayer3DRoot;

    [SerializeField]
    private GameObject SingleBondPrefeb;

    [SerializeField]
    private GameObject DoubleBondPrefeb;

    private Dictionary<AminoacidType, GameObject> AtomPrefebsDic;

    [Header("UIReference")]
    [SerializeField]
    private Text proteinInfoText;
    [SerializeField]
    private Text chaininInfoText;
    [SerializeField]
    private Text aminoacidInfoText;
    [SerializeField]
    private Text atomInfoText;

    protected override void OnCreated() {
        base.OnCreated();
        Canvas2D.GetComponent<Canvas>().worldCamera = Camera.main;
        AtomPrefebsDic = new Dictionary<AminoacidType, GameObject>();
    }

    private GameObject GetAtomPrefeb(AminoacidType aminoacidType) {
        GameObject prefeb = null;
        if (!AtomPrefebsDic.TryGetValue(aminoacidType, out prefeb)) {
            prefeb = Resources.Load<GameObject>(string.Format("{0}/{1}", AtomSpherePrefebPath, aminoacidType.ToString()));
            AtomPrefebsDic.Add(aminoacidType, prefeb);
        }
        return AtomPrefebsDic[aminoacidType];
    }

    private Material GetMaterial(AminoacidType aminoacidType) {
        return GetAtomPrefeb(aminoacidType).GetComponent<Renderer>().sharedMaterial;
    }

    public void ShowProtein(Protein protein) {
        GameObject proteinDisplayerGo = new GameObject(protein.ID, typeof(ProteinDisplayer));
        proteinDisplayerGo.GetComponent<ProteinDisplayer>().Protein = protein;
        proteinDisplayerGo.transform.SetParent(Displayer3DRoot.transform);
        foreach (var chainKvp in protein.Chains) {
            Chain chain = chainKvp.Value;
            GameObject chainDisplayerGo = new GameObject(chain.ID, typeof(ChainDisplayer));
            chainDisplayerGo.GetComponent<ChainDisplayer>().Chain = chain;
            chainDisplayerGo.transform.SetParent(proteinDisplayerGo.transform);
            //创建每个原子模型
            foreach (var aminoacidKvp in chain.SeqAminoacids) {
                AminoacidInProtein aminoacidInProtein = aminoacidKvp.Value;
                GameObject aminoacidDisplayerGo = new GameObject(aminoacidInProtein.ResidueSeq.ToString(), typeof(AminoacidDisplayer));
                aminoacidDisplayerGo.GetComponent<AminoacidDisplayer>().AminoacidInProtein = aminoacidInProtein;
                aminoacidDisplayerGo.transform.SetParent(chainDisplayerGo.transform);
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
                //创建每个键的模型
                if (displayMode == DisplayMode.BallStick) {
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
                        GenerateBondGameObject(name1, pos1, pos2, bondType, parent, aminoacidType1);
                        GenerateBondGameObject(name2, pos2, pos3, bondType, parent, aminoacidType2);
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
                        GenerateBondGameObject(name, pos1, pos2, bondType, parent, aminoacidType);
                    }
                }
            }
        }
    }

    public void SetBoardInfo(AtomDisplayer atomDisplayer) {
        if(atomDisplayer == null) {
            proteinInfoText.text = "";
            chaininInfoText.text = "";
            aminoacidInfoText.text = "";
            atomInfoText.text = "";
            return;
        }
        AtomInAminoacid atomInAminoacid = atomDisplayer.AtomInAminoacid;
        AminoacidInProtein aminoacidInProtein = atomDisplayer.AminoacidInProtein;
        Chain chain = aminoacidInProtein.Chain;
        Protein protein = chain.Protein;
        proteinInfoText.text = string.Format("-Protein-\nID: {0}\nClassification: {1}\nPublishDate: {2}",
            protein.ID,protein.Classification,protein.PublishDate);
        chaininInfoText.text = string.Format("-Chain-\nID: {0}\nOXT: {1}",
            chain.ID, chain.OXT != null);
        aminoacidInfoText.text = string.Format("-Aminoacid-\nSeq: {0}\nType: {1}\nIsStandard: {2}",
            aminoacidInProtein.ResidueSeq, aminoacidInProtein.Aminoacid.Type.ToString(), aminoacidInProtein.Aminoacid.IsStandard);
        atomInfoText.text = string.Format("-Atom-\nName: {0}\nSerial: {1}\nPosition: {2}",
            atomInAminoacid.Name, aminoacidInProtein.AtomInAminoacidSerial[atomInAminoacid], aminoacidInProtein.AtomInAminoacidPos[atomInAminoacid].ToString("F3"));
    }


    public void DestroyProtein() {
        Destroy(Displayer3DRoot.transform.GetChild(0).gameObject);
    }

    //这里的position是真正坐标而不是pdb文件中读取的坐标
    private GameObject GenerateBondGameObject(string name, Vector3 pos1, Vector3 pos2, BondType bondType, Transform parent, AminoacidType aminoacidType) {
        GameObject bondGo = null;
        Material material = GetMaterial(aminoacidType);
        float bondLength = (pos1 - pos2).magnitude;//键长
        Vector3 bondDirection = (pos1 - pos2).normalized; //键方向
        if (bondType == BondType.Single) {
            bondGo = Instantiate(SingleBondPrefeb, parent, false);
        }
        else if (bondType == BondType.Double) {
            bondGo = Instantiate(DoubleBondPrefeb, parent, false);
        }
        else throw new System.Exception("Unhandled bondType :" + bondType);

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

    [ImplementedInController("OnSliderChanged")]
    public void OnSliderChanged(float value) { }

    [ImplementedInController("OnBallStickToggleChanged")]
    public void OnBallStickToggleChanged(bool value) { }


}
