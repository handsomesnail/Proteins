using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace PolymerModel.Data {

    /// <summary>蛋白质中的残基实例</summary>
    public class AminoacidInProtein {

        /// <summary>该残基所属蛋白质</summary>
        public Protein Protein { get; set; }

        /// <summary>所属链的ID</summary>
        public string ChainId { get; private set; }

        /// <summary>该残基在在该链中的顺序号</summary>
        public int ResidueSeq { get; private set; }

        /// <summary>残基所属氨基酸</summary>
        public Aminoacid Aminoacid { get; private set; }

        /// <summary>氨基酸内部各原子的相对于蛋白质的位置信息</summary>
        public ReadOnlyDictionary<AtomInAminoacid, Vector3> AtomInAminoacidPos { get; private set; }

        public AminoacidInProtein(Protein protein, string resName, string chainId, int residueSeq, IDictionary<string, Vector3> atomInAminoacidPos) {
            this.Protein = protein;
            this.ChainId = chainId;
            this.ResidueSeq = ResidueSeq;
            this.Aminoacid = Aminoacid.Generate(resName);
            Dictionary<AtomInAminoacid, Vector3> atomInAminoacidPosDic = new Dictionary<AtomInAminoacid, Vector3>();
            foreach (var child in atomInAminoacidPos) {
                atomInAminoacidPosDic.Add(Aminoacid[child.Key], child.Value);
            }
            AtomInAminoacidPos = new ReadOnlyDictionary<AtomInAminoacid, Vector3>(atomInAminoacidPosDic);
        }

        public override bool Equals(object obj) {
            AminoacidInProtein aminoacid = obj as AminoacidInProtein;
            if (aminoacid == null)
                return false;
            else return this.Protein.ID == aminoacid.Protein.ID && this.ChainId == aminoacid.ChainId && this.ResidueSeq == aminoacid.ResidueSeq;
        }

        public override int GetHashCode() {
            return Protein.GetHashCode() + ChainId.GetHashCode() + ResidueSeq.GetHashCode();
        }

    }

}
