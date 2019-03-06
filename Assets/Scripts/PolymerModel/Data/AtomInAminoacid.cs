using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace PolymerModel.Data {

    /// <summary>氨基酸中的原子实例</summary>
    public class AtomInAminoacid {

        /// <summary>所属氨基酸</summary>
        public Aminoacid Aminoacid { get; internal set; }

        /// <summary>在该氨基酸中的名字(ATOM[13-16])</summary>
        public string Name { get; private set; }

        /// <summary>所属原子类型</summary>
        public Atom Atom { get; private set; }

        public AtomInAminoacid(string name) {
            this.Name = name;
            this.Atom = GetAtomByName(name);
        }

        /// <summary>根据名字规则返回原子类型</summary>
        private Atom GetAtomByName(string name) {
            //即首字母为原子类型
            switch (char.ToUpper(name.First())) {
                case 'C': return Atom.C;
                case 'N': return Atom.N;
                case 'O': return Atom.O;
                case 'S': return Atom.S;
                case 'P': return Atom.P;
                default: throw new ArgumentException("Unhandled Atom Name:" + name);
            }
        }

        public override bool Equals(object obj) {
            AtomInAminoacid atom = obj as AtomInAminoacid;
            if (atom == null)
                return false;
            else return this.Aminoacid == atom.Aminoacid && this.Name == atom.Name;
        }

        /// <summary>直接使用名字作为HashCode(保证在同一个氨基酸中作为Key存储哈希表) </summary>
        public override int GetHashCode() {
            return Name.GetHashCode() + Aminoacid.GetHashCode();
        }

    }

}
