using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace PolymerModel.Data {

    /// <summary>多肽链</summary>
    public class Chain {

        /// <summary>所属蛋白质</summary>
        public Protein Protein { get; internal set; }

        /// <summary>链所属ID</summary>
        public string ID { get; private set; }

        /// <summary>该链中的OXT原子(若没有则为null)</summary>
        public OXTAtom OXT { get; private set; }

        /// <summary>链内氨基酸标准残基序列</summary>
        public ReadOnlyDictionary<int, AminoacidInProtein> SeqAminoacids { get; private set; }

        public Chain(string id, IDictionary<int, AminoacidInProtein> seqAminoacids) : this(id, seqAminoacids, null) { }
            
        public Chain(string id, IDictionary<int, AminoacidInProtein> seqAminoacids, OXTAtom oxtAtom) {
            this.ID = id;
            this.OXT = oxtAtom;
            if (oxtAtom != null) {
                oxtAtom.Chain = this;
            }
            foreach (var child in seqAminoacids) {
                child.Value.Chain = this;
            }
            this.SeqAminoacids = new ReadOnlyDictionary<int, AminoacidInProtein>(seqAminoacids);
        }

    }

}
