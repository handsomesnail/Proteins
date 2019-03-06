using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace PolymerModel.Data {

    /// <summary>蛋白质</summary>
    public class Protein {

        /// <summary>蛋白质ID号(Header[63-66])</summary>
        public string ID { get; private set; }

        /// <summary>分子类(Header[11-50])</summary>
        public string Classification { get; private set; }

        /// <summary>公布日期(Header[51-59])</summary>
        public string PublishDate { get; private set; }

        /// <summary>链序列(链内为标准残基序列)</summary>
        public ReadOnlyDictionary<string, Chain> Chains { get; private set; }

        //TODO: 非标准残基等其它信息

        //读取文件 传入相应参数 构造一个蛋白质对象
        public Protein(string id, string classification,  string publishDate, IDictionary<string, Chain> chains) {
            this.ID = id;
            this.Classification = classification;
            this.PublishDate = publishDate;
            foreach(var aminoacid in chains) {
                aminoacid.Value.Protein = this;
            }
            Chains = new ReadOnlyDictionary<string, Chain>(chains);
        }

        public override bool Equals(object obj) {
            Protein protein = obj as Protein;
            if (protein == null)
                return false;
            else return this.ID == protein.ID;
        }

        public override int GetHashCode() {
            return ID.GetHashCode();
        }

    }

}
