using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace PolymerModel.Data {

    /// <summary>蛋白质</summary>
    public class Protein {

        /// <summary>蛋白质ID号</summary>
        public string ID { get; private set; }

        /// <summary>分子类</summary>
        public string Type { get; private set; }

        /// <summary>公布日期</summary>
        public string PublishDate { get; private set; }

        /// <summary>标准残基集合</summary>
        public ReadOnlyCollection<AminoacidInProtein> Aminoacids { get; private set; }

        //非标准残基

        //其它信息

        //读取文件 传入相应参数 构造一个蛋白质对象
        public Protein(string id, IList<AminoacidInProtein> aminoacids) {
            this.ID = id;
            Aminoacids = new ReadOnlyCollection<AminoacidInProtein>(aminoacids);
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
