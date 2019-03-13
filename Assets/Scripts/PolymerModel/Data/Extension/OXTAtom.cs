using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolymerModel.Data {

    /// <summary>OXT原子(一般在链末尾)</summary>
    public class OXTAtom {

        /// <summary>OXT原子的序号</summary>
        public int Serial { get; private set; }

        /// <summary>位置坐标</summary>
        public Vector3 Pos { get; private set; }

        /// <summary>所属链</summary>
        public Chain Chain { get; internal set; }
        
        public OXTAtom(int serial, Vector3 pos) {
            this.Serial = serial;
            this.Pos = pos;
        }

    }

}
