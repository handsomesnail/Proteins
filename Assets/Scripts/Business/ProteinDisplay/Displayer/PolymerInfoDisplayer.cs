using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolymerInfoDisplayer : MonoBehaviour {

    [Header("UIReference")]
    [SerializeField]
    private TextMesh proteinIdCodeText;
    [SerializeField]
    private TextMesh proteinPublishDateText;
    [SerializeField]
    private TextMesh proteinClassificationText;
    [SerializeField]
    private TextMesh chainIDText;
    [SerializeField]
    private TextMesh chainLengthText;
    [SerializeField]
    private TextMesh chainHasOXTText;
    [SerializeField]
    private TextMesh residueSeqText;
     [SerializeField]
    private TextMesh residueTypeText;
    [SerializeField]
    private TextMesh residueIsStandardText;
    [SerializeField]
    private TextMesh atomNameText;
    [SerializeField]
    private TextMesh atomElementText;
    [SerializeField]
    private TextMesh atomSerialText;
    [SerializeField]
    private TextMesh atomCoordinateText;

    #region Public Method

    /// <summary>Atom模式选取</summary>
    public void SetData(AtomDisplayer atomDisplayer) {
        AtomInAminoacid atomInAminoacid = atomDisplayer.AtomInAminoacid;
        AminoacidInProtein aminoacidInProtein = atomDisplayer.AminoacidInProtein;
        Chain chain = aminoacidInProtein.Chain;
        Protein protein = chain.Protein;
        SetProteinData(protein);
        SetChainData(chain);
        SetAminoacidData(aminoacidInProtein);
        SetAtomData(aminoacidInProtein, atomInAminoacid);
    }

    /// <summary>aminoacid模式选取</summary>
    public void SetData(AminoacidDisplayer aminoacidDisplayer) {
        AminoacidInProtein aminoacidInProtein = aminoacidDisplayer.AminoacidInProtein;
        Chain chain = aminoacidInProtein.Chain;
        Protein protein = chain.Protein;
        SetProteinData(protein);
        SetChainData(chain);
        SetAminoacidData(aminoacidInProtein);
    }

    /// <summary>Chain模式选取 </summary>
    public void SetData(ChainDisplayer chainDisplayer) {
        Chain chain = chainDisplayer.Chain;
        Protein protein = chain.Protein;
        SetProteinData(protein);
        SetChainData(chain);
    }

    public void ClearData() {
    }

    #endregion

    #region Private Method

    private void SetProteinData(Protein protein) {
        proteinIdCodeText.text = protein.ID;
        proteinPublishDateText.text = protein.PublishDate;
        proteinClassificationText.text = protein.Classification;
    }

    private void SetChainData(Chain chain) {
        chainIDText.text = chain.ID;
        chainLengthText.text = chain.SeqAminoacids.Count.ToString();
        chainHasOXTText.text = (chain.OXT != null).ToString();
    }

    private void SetAminoacidData(AminoacidInProtein aminoacidInProtein) {
        residueSeqText.text = aminoacidInProtein.ResidueSeq.ToString();
        residueTypeText.text = aminoacidInProtein.Aminoacid.Type.ToString();
        residueIsStandardText.text = aminoacidInProtein.Aminoacid.IsStandard.ToString();
    }

    private void SetAtomData(AminoacidInProtein aminoacidInProtein, AtomInAminoacid atomInAminoacid) {
        atomNameText.text = atomInAminoacid.Name;
        atomSerialText.text = aminoacidInProtein.AtomInAminoacidSerial[atomInAminoacid].ToString();
        atomElementText.text = atomInAminoacid.Name[0].ToString();
        atomCoordinateText.text = aminoacidInProtein.AtomInAminoacidPos[atomInAminoacid].ToString("F3").TrimStart('(').TrimEnd(')').Replace(" ", "");
    }

    #endregion

}
