using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Text;
using System.IO;
using System.Xml;

namespace Util {

    /// <summary>CSV格式读取</summary>
    public static class CSVHelper {

        /// <summary>读取CSV并转化为数据表 </summary>
        public static DataTable ReadCSVStr(string str) {
            DataTable dt = new DataTable();
            using (StringReader sr = new StringReader(str)) {
                string line;
                int lineIndex = 0; //当前读取的行
                int columnCount = 0; //列数
                while ((line = sr.ReadLine()) != null) {
                    if (lineIndex == 0) {
                        //添加列完成框架
                        string[] lineData = line.Split(',');
                        columnCount = lineData.Length;
                        foreach (string columnName in lineData) {
                            dt.Columns.Add(columnName, typeof(string));
                        }
                    }
                    else {
                        //添加行数据
                        string[] lineData = line.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < columnCount; i++) {
                            dr[i] = lineData[i];
                        }
                        dt.Rows.Add(dr);
                    }
                    lineIndex++;
                }
            }
            return dt;
        }

        public static DataTable ReadCSVBytes(byte[] bytes) {
            return ReadCSVStr(Encoding.UTF8.GetString(bytes));
        }

    }

}
