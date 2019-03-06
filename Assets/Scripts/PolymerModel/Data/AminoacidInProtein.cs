using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace PolymerModel.Data {

    /// <summary>蛋白质中的残基实例</summary>
    public class AminoacidInProtein {

        /// <summary>该残基所属蛋白质</summary>
        //public Protein Protein { get; internal set; }

        public Chain Chain { get; internal set; }

        /// <summary>所属链的ID(ATOM[22])</summary>
        public string ChainId { get; private set; }

        /// <summary>该残基在在该链中的顺序号(ATOM[23-26])</summary>
        public int ResidueSeq { get; private set; }
        
        /// <summary>可替换标识符(ATOM[17])</summary>
        public char AltLoc { get; private set; }

        /// <summary>残基名字(ATOM[18-20])</summary>
        public string ResName { get; private set; }

        /// <summary>残基所属氨基酸</summary>
        public Aminoacid Aminoacid { get; private set; }

        /// <summary>氨基酸内部各原子的相对于蛋白质的位置信息</summary>
        /// X:ATOM[31-38] Y:ATOM[39-46] Z:ATOM[47-54] Real(8.3)
        public ReadOnlyDictionary<AtomInAminoacid, Vector3> AtomInAminoacidPos { get; private set; }

        /// <summary>氨基酸内部各原子在蛋白质中的序号</summary>
        public ReadOnlyDictionary<AtomInAminoacid, int> AtomInAminoacidSerial { get; private set; }

        public AminoacidInProtein(char altLoc, string resName, string chainId, int residueSeq, IDictionary<AtomInAminoacid, Vector3> atomInAminoacidPos, IDictionary<AtomInAminoacid, int> atomInAminoacidSerial) {
            this.ChainId = chainId;
            this.ResidueSeq = residueSeq;
            this.AltLoc = altLoc;
            this.ResName = resName;
            this.Aminoacid = Aminoacid.Generate(resName);
            this.AtomInAminoacidPos = new ReadOnlyDictionary<AtomInAminoacid, Vector3>(atomInAminoacidPos);
            this.AtomInAminoacidSerial = new ReadOnlyDictionary<AtomInAminoacid, int>(atomInAminoacidSerial);
        }

        public override bool Equals(object obj) {
            AminoacidInProtein aminoacid = obj as AminoacidInProtein;
            if (aminoacid == null)
                return false;
            else return this.Chain.Protein.ID == aminoacid.Chain.Protein.ID && this.ChainId == aminoacid.ChainId && this.ResidueSeq == aminoacid.ResidueSeq;
        }

        public override int GetHashCode() {
            return this.Chain.Protein.GetHashCode() + ChainId.GetHashCode() + ResidueSeq.GetHashCode();
        }

    }

}
