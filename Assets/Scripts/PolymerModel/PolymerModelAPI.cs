using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;
using Util;
using PolymerModel.Data;
using System;

/*
蛋白质从上至下的数据层次结构划分为：蛋白质-链-残基-原子
分别对应数据类型：Protein-Chain-AminoacidInProtein-AtomInAminoacid
以上四种数据类型只有在某个蛋白质内才有确切含义
而由于构成蛋白质的氨基酸种类和原子种类个数是一定的，为了防止在残基层和原子层构造大量重复数据的对象
就有了Aminoacid和Atom两个类，表示氨基酸和原子独立于蛋白质之外的本身的性质，每种只构造一个对象
AminoacidInProtein持有对应Aminoacid对象的引用，AtomInAminoacid持有对应Atom对象的引用
*/

namespace PolymerModel {

    internal class PolymerModelException : Exception {
        public PolymerModelException(string message, Exception innerException) : base(message, innerException) { }
        public PolymerModelException(string message) : base(message) { }
        public PolymerModelException() : base() { }
    }

    public static class PolymerModelAPI {

        private static bool loaded;

        private static DataTable AminoacidDt;
        private static DataTable ConnectionDt;

        static PolymerModelAPI() {
            loaded = false;
        }

        /// <summary>从本地加载氨基酸数据 </summary>
        public static async Task LoadDataAsync() {
            if (loaded) {
                throw new PolymerModelException("PolymerModel has been loaded");
            }
            string aminoacidData = await IOUtil.ReadInStreamingAssetsAsync(string.Empty, "Aminoacid.csv");
            AminoacidDt = CSVHelper.ReadCSVStr(aminoacidData);
            string connectionData = await IOUtil.ReadInStreamingAssetsAsync(string.Empty, "Connection.csv");
            ConnectionDt = CSVHelper.ReadCSVStr(connectionData);

            //将数据表转化为字典提高查询效率
            Dictionary<string, List<DataRow>> connectionDic = new Dictionary<string, List<DataRow>>();
            foreach (DataRow connectionDataRow in ConnectionDt.Rows) {
                string type = connectionDataRow["Type"].ToString();
                if (!connectionDic.ContainsKey(type)) {
                    connectionDic.Add(type, new List<DataRow>());
                }
                connectionDic[type].Add(connectionDataRow);
            }

            //遍历氨基酸数据表构建氨基酸数据结构
            foreach (DataRow aminoacidDataRow in AminoacidDt.Rows) {
                string type = aminoacidDataRow["Type"].ToString();
                AminoacidType aminoacidType = (AminoacidType)Enum.Parse(typeof(AminoacidType), type);
                string chinese = aminoacidDataRow["Chinese"].ToString();
                bool isStandard = aminoacidDataRow["IsStandard"].ToString() == "1";
                string[] atomNames = aminoacidDataRow["Atoms"].ToString().Trim('[', ']').Split(' ');
                Dictionary<KeyValuePair<string, string>, BondType> connection = new Dictionary<KeyValuePair<string, string>, BondType>();
                if (connectionDic.ContainsKey(type)) {
                    //若该残基包含键
                    foreach (DataRow connectionDataRow in connectionDic[type]) {
                        string firstAtom = connectionDataRow["First"].ToString();
                        string secondAtom = connectionDataRow["Second"].ToString();
                        BondType bondType = (BondType)Enum.Parse(typeof(BondType), connectionDataRow["BondType"].ToString());
                        connection.Add(new KeyValuePair<string, string>(firstAtom, secondAtom), bondType);
                    }
                }
                Aminoacid aminoacid = new Aminoacid(aminoacidType, chinese, isStandard, atomNames, connection);
                Aminoacid.Aminoacids.Add(aminoacid.Type, aminoacid);
            }
            loaded = true;
        }

    }

}
