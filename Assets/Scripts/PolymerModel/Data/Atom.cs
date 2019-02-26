using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolymerModel.Data {

    /// <summary>原子(定义一类原子的属性)</summary>
    public class Atom {

        #region 各类组成氨基酸的元素

        public static readonly Atom H;
        public static readonly Atom C;
        public static readonly Atom N;
        public static readonly Atom O;
        public static readonly Atom S;
        public static readonly Atom P;

        #endregion

        /// <summary>元素类型</summary>
        public AtomType Type { get; private set; }

        /// <summary>元素原子半径(范德华半径)</summary>
        public float Radius { get; private set; }

        /// <summary>私有构造函数</summary>
        private Atom(AtomType type) : this(type, 1.0f) { }

        private Atom(AtomType type, float radius) {
            this.Type = type;
            this.Radius = radius;
        }

        /// <summary>数据取自Bondi汇总</summary>
        static Atom() {

            #region 构造各类元素

            H = new Atom(AtomType.H, 0.12f);
            C = new Atom(AtomType.C, 0.17f);
            N = new Atom(AtomType.N, 0.155f);
            O = new Atom(AtomType.O, 0.152f);
            S = new Atom(AtomType.S, 0.18f);
            P = new Atom(AtomType.P, 0.18f);

            #endregion

        }

        public override bool Equals(object obj) {
            Atom atom = obj as Atom;
            if (atom == null)
                return false;
            else return this.Type == atom.Type;
        }

        public override int GetHashCode() {
            return (int)Type;
        }

    }

}