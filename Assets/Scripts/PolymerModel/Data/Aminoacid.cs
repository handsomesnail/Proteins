using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace PolymerModel.Data {

    /// <summary>氨基酸残基</summary>
    public class Aminoacid {

        #region 20种氨基酸

        public static readonly Aminoacid ALA;
        public static readonly Aminoacid ARG;
        public static readonly Aminoacid ASP;
        public static readonly Aminoacid CYS;
        public static readonly Aminoacid GLN;
        public static readonly Aminoacid GLU;
        public static readonly Aminoacid HIS;
        public static readonly Aminoacid ILE;
        public static readonly Aminoacid GLY;
        public static readonly Aminoacid ASN;
        public static readonly Aminoacid LEU;
        public static readonly Aminoacid LYS;
        public static readonly Aminoacid MET;
        public static readonly Aminoacid PHE;
        public static readonly Aminoacid PRO;
        public static readonly Aminoacid SER;
        public static readonly Aminoacid THR;
        public static readonly Aminoacid TRP;
        public static readonly Aminoacid TYR;
        public static readonly Aminoacid VAL;

        public static readonly Aminoacid HOH;

        #endregion

        /// <summary>氨基酸类型</summary>
        public AminoacidType Type { get; private set; }

        /// <summary>是否是标准残基</summary>
        public bool IsStandard { get; private set; }

        /// <summary>氨基酸类型(字符串英文缩写)</summary>
        public string TypeStr { get; private set; }

        /// <summary>包含的原子</summary>
        public ReadOnlyCollection<AtomInAminoacid> Atoms { get; private set; }

        /// <summary>使用字典进行O(1)查询</summary>
        private Dictionary<string, AtomInAminoacid> atomDic;

        /// <summary>索引器 </summary>
        public AtomInAminoacid this[string key] {
            get {
                return atomDic[key];
            }
        }

        /// <summary>原子间的化学键连接</summary>
        /// 不能用字典 因为有可能一对多连接
        public ReadOnlyCollection<KeyValuePair<AtomInAminoacid, AtomInAminoacid>> Connection { get; private set; }

        /// <summary>
        /// 私有构造函数
        /// </summary>
        /// <param name="type">氨基酸类型</param>
        /// <param name="isStandard">是否是标准残基</param>
        /// <param name="atoms">构成氨基酸得原子</param>
        /// <param name="connection">原子间的连接关系</param>
        private Aminoacid(AminoacidType type, bool isStandard, IList<AtomInAminoacid> atoms, IList<KeyValuePair<string, string>> connection) {
            this.Type = type;
            this.TypeStr = type.ToString();

            foreach (var child in atoms) {
                child.Aminoacid = this;
            }
            Atoms = new ReadOnlyCollection<AtomInAminoacid>(atoms);

            atomDic = new Dictionary<string, AtomInAminoacid>();
            foreach (var child in atoms) {
                atomDic.Add(child.Name, child);
            }

            List<KeyValuePair<AtomInAminoacid, AtomInAminoacid>> connectList = new List<KeyValuePair<AtomInAminoacid, AtomInAminoacid>>();
            foreach (var child in connection) {
                connectList.Add(new KeyValuePair<AtomInAminoacid, AtomInAminoacid>(this[child.Key], this[child.Value]));
            }
            Connection = new ReadOnlyCollection<KeyValuePair<AtomInAminoacid, AtomInAminoacid>>(connectList);
        }

        public override bool Equals(object obj) {
            Aminoacid atom = obj as Aminoacid;
            if (atom == null)
                return false;
            else return this.Type == atom.Type;
        }

        public override int GetHashCode() {
            return (int)Type;
        }

        /// <summary>获取某个氨基酸实例 </summary>
        public static Aminoacid Generate(string type) {
            return Generate((AminoacidType)Enum.Parse(typeof(AminoacidType), type));
        }

        public static Aminoacid Generate(AminoacidType type) {
            switch (type) {
                case AminoacidType.ALA: return ALA;
                case AminoacidType.ARG: return ARG;
                case AminoacidType.ASN: return ASN;
                case AminoacidType.ASP: return ASP;
                case AminoacidType.CYS: return CYS;
                case AminoacidType.GLN: return GLN;
                case AminoacidType.GLU: return GLU;
                case AminoacidType.GLY: return GLY;
                case AminoacidType.HIS: return HIS;
                case AminoacidType.ILE: return ILE;
                case AminoacidType.LEU: return LEU;
                case AminoacidType.LYS: return LYS;
                case AminoacidType.MET: return MET;
                case AminoacidType.PHE: return PHE;
                case AminoacidType.PRO: return PRO;
                case AminoacidType.SER: return SER;
                case AminoacidType.THR: return THR;
                case AminoacidType.TRP: return TRP;
                case AminoacidType.TYR: return TYR;
                case AminoacidType.VAL: return VAL;
                case AminoacidType.HOH: return HOH;
                default: throw new ArgumentException("Unhandled AminoacidType:" + type.ToString());
            }
        }

        /// <summary>通过atom在氨基酸中的名字序列返回Atom序列</summary>
        private static List<AtomInAminoacid> GetAtomInAminoacidList(IList<string> atomNames) {
            List<AtomInAminoacid> atomInAminoacidList = new List<AtomInAminoacid>();
            foreach (string name in atomNames) {
                atomInAminoacidList.Add(new AtomInAminoacid(name));
            }
            return atomInAminoacidList;
        }

        static Aminoacid() {

            #region 构造20种氨基酸

            ALA = new Aminoacid(
                AminoacidType.ALA, true,
                GetAtomInAminoacidList(new List<string>() { "N", "CA", "C", "O", "CB" }),
                new List<KeyValuePair<string, string>>() {

                });
            HOH = new Aminoacid(
                AminoacidType.HOH, false,
                GetAtomInAminoacidList(new List<string>() { "O" }),
                new List<KeyValuePair<string, string>>() {

                });


            #endregion

        }


    }

}
