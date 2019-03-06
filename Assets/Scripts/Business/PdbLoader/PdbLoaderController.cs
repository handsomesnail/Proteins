using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Util;
using ZCore;

public class PdbLoaderController : Controller {

    protected override void Start() {
        base.Start();
    }

    public Protein GetProteinData() {
        return GetModel<PdbLoaderModel>().ProteinData;
    }

    public async void LoadLocalPdbFile() {
        byte[] data = await IOUtil.PickFile();
        using(StringReader sr = new StringReader(Encoding.UTF8.GetString(data))) {
            string record = null;//当前读取的记录

            //蛋白质作用域***********/
            string id = null;
            string classification = null;
            string publishDate = null;
            Dictionary<string, Chain> chains = new Dictionary<string, Chain>();
            //*************************/

            //肽链作用域*************/
            Dictionary<int, AminoacidInProtein> seqAminoacids = new Dictionary<int, AminoacidInProtein>();
            //*************************/


            //氨基酸作用域***********/
            char altloc = ' ';
            string resName = null;
            string chainId = null;
            int residueSeq = 0;
            Dictionary<AtomInAminoacid, Vector3> atomInAminoacidPos = new Dictionary<AtomInAminoacid, Vector3>();
            Dictionary<AtomInAminoacid, int> atomInAminoacidSerial = new Dictionary<AtomInAminoacid, int>();
            //*************************/

            AminoacidInProtein currentAminoacidInProtein = null;
            bool completeLastAminoacid = true;//已完成上一个残基的读取

            while ((record = sr.ReadLine()) != null) {
                //原子作用域
                string title = record.Substring(0, 6);
                if(title.StartsWith("HEADER")) {
                    classification = record.Substring(10, 40).Trim();//11-50
                    publishDate = record.Substring(50, 9);//51-59
                    id = record.Substring(62, 4);//63-66
                }
                else if (title.StartsWith("ATOM")) {

                    //氨基酸残基相关部分
                    int lastResidueSeq = residueSeq;
                    residueSeq = int.Parse(record.Substring(22, 4).Trim()); //23-26 残基序列号作为判断标志
                    if (lastResidueSeq != residueSeq && !completeLastAminoacid) {
                        //若当前record为新的氨基酸残基的第一条记录(或记录为TER)
                        currentAminoacidInProtein = new AminoacidInProtein(altloc, resName, chainId, lastResidueSeq, atomInAminoacidPos, atomInAminoacidSerial);
                        seqAminoacids.Add(lastResidueSeq, currentAminoacidInProtein);
                        atomInAminoacidPos = new Dictionary<AtomInAminoacid, Vector3>();
                        atomInAminoacidSerial = new Dictionary<AtomInAminoacid, int>();
                        completeLastAminoacid = true;
                    }
                    else {
                        completeLastAminoacid = false;
                        if (record[16] != altloc) {
                            continue; //若当前可替换标识符不是默认则跳过当前记录行
                        }
                    }

                    chainId = record.Substring(21, 1); //22
                    altloc = record[16]; //17
                    resName = record.Substring(17, 3); //18-20

                    //原子相关部分
                    string atomName = record.Substring(12, 4).Trim(); //13-16
                    int atomSerial = int.Parse(record.Substring(6, 5).Trim()); //7-11
                    float x = float.Parse(record.Substring(30, 8).Trim()); //31-38
                    float y = float.Parse(record.Substring(38, 8).Trim()); //39-46
                    float z = float.Parse(record.Substring(46, 8).Trim()); //47-54
                    Vector3 pos = new Vector3(x, y, z);
                    atomInAminoacidPos.Add(Aminoacid.Generate(resName)[atomName], pos);
                    atomInAminoacidSerial.Add(Aminoacid.Generate(resName)[atomName], atomSerial);
                }
                else if (title.StartsWith("TER")) { //链结束
                    //氨基酸残基结算
                    residueSeq = int.Parse(record.Substring(22, 4).Trim()); //23-26 残基序列号作为判断标志
                    currentAminoacidInProtein = new AminoacidInProtein(altloc, resName, chainId, residueSeq, atomInAminoacidPos, atomInAminoacidSerial);
                    seqAminoacids.Add(residueSeq, currentAminoacidInProtein);
                    atomInAminoacidPos = new Dictionary<AtomInAminoacid, Vector3>();
                    completeLastAminoacid = true;

                    //肽链结算
                    Chain chain = new Chain(chainId, seqAminoacids);
                    seqAminoacids = new Dictionary<int, AminoacidInProtein>();
                    chains.Add(chain.ID, chain);
                }
                else if (title.StartsWith("HETATM")) { //非标准残基

                }
            }

            Protein protein = new Protein(id, classification, publishDate, chains);
            GetModel<PdbLoaderModel>().ProteinData = protein;
            Debug.Log(protein);
        }

    }

}
