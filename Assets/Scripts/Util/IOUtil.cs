using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using Windows.Storage;
using Windows.Storage.Pickers;
#endif

namespace Util {

    public enum SelectFileError {
        OverSize = 0,//文件过大
        Cancel,//取消选择
    }

    public class SelectFileException : Exception {

        public SelectFileError Error { get; private set; }
        public SelectFileException(string message, Exception innerException) : base(message, innerException) { }
        public SelectFileException(string message) : base(message) { }
        public SelectFileException(SelectFileError error) : this() {
            Error = error;
        }
        public SelectFileException() : base() { }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class OpenFileName {
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;
        public String filter = null;
        public String customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;
        public String file = null;
        public int maxFile = 0;
        public String fileTitle = null;
        public int maxFileTitle = 0;
        public String initialDir = null;
        public String title = null;
        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;
        public String defExt = null;
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public String templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }

    /// <summary>P/Invoke链接Win32 Api</summary>
    public class LocalDialog {
        /// <summary>打开文件对话框</summary>
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Unicode)]
        public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
        public static bool GetOFN([In, Out] OpenFileName ofn) {
            return GetOpenFileName(ofn);
        }
        /// <summary>另存为对话框 </summary>
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Unicode)]
        public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
        public static bool GetSFN([In, Out] OpenFileName ofn) {
            return GetSaveFileName(ofn);
        }
    }

    /// <summary>文件读写Util</summary>
    public static class IOUtil {

        #region Write&Read

        public static void Write(string folderPath, string fileName, string content) {
            string fullPath = folderPath + "/" + fileName;
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
            //默认是UTF8编码的
            using (StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8)) {//覆盖该文件
                sw.Write(content);
            }
        }

        public static void Write(string folderPath, string fileName, byte[] content) {
            string fullPath = folderPath + "/" + fileName;
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write)) {
                fs.Write(content, 0, content.Length);//传入的byte是经过string编码转化后的 
            }
        }

        public static string Read(string folderPath, string fileName) {
            string fullPath = folderPath + "/" + fileName;
            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader sw = new StreamReader(fs, Encoding.UTF8)) {
                    return sw.ReadToEnd();
                }
            }
        }

        public static byte[] ReadBytes(string folderPath, string fileName) {
            string fullPath = folderPath + "/" + fileName;
            return ReadBytes(fullPath);
        }

        public static byte[] ReadBytes(string fullPath) {
            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader sw = new StreamReader(fs)) {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    return bytes;
                }
            }
        }

        #endregion

        #region Write&Read(Async)

        public async static Task WriteAsync(string folderPath, string fileName, string content) {
            string fullPath = folderPath + "/" + fileName;
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
            using (StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8)) {
                await sw.WriteAsync(content);
            }
        }

        public async static Task WriteAsync(string folderPath, string fileName, byte[] content) {
            string fullPath = folderPath + "/" + fileName;
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write)) {
                await fs.WriteAsync(content, 0, content.Length);
            }
        }

        public async static Task<string> ReadAsync(string folderPath, string fileName) {
            string fullPath = folderPath + "/" + fileName;
            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader sw = new StreamReader(fs, Encoding.UTF8)) {
                    return await sw.ReadToEndAsync();
                }
            }
        }

        public async static Task<byte[]> ReadBytesAsync(string folderPath, string fileName) {
            string fullPath = folderPath + "/" + fileName;
            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader sw = new StreamReader(fs)) {
                    byte[] bytes = new byte[fs.Length];
                    await fs.ReadAsync(bytes, 0, bytes.Length);
                    return bytes;
                }
            }
        }

        #endregion

        #region PresidentPath

        /// <summary>向PresidentPath路径下写入文本</summary>
        public async static Task WriteInPresidentAsync(string folderPathInPresident, string fileName, string content) {
            await WriteAsync(Application.persistentDataPath + folderPathInPresident, fileName, content);
        }

        /// <summary>向PresidentPath路径下写入二进制文件</summary>
        public async static Task WriteInPresidentAsync(string folderPathInPresident, string fileName, byte[] content) {
            await WriteAsync(Application.persistentDataPath + folderPathInPresident, fileName, content);
        }

        public static void WriteInPresident(string folderPathInPresident, string fileName, string content) {
            Write(Application.persistentDataPath + folderPathInPresident, fileName, content);
        }

        public static void WriteInPresident(string folderPathInPresident, string fileName, byte[] content) {
            Write(Application.persistentDataPath + folderPathInPresident, fileName, content);
        }


        /// <summary>从PresidentPath路径下读取文件的文本 </summary>
        public async static Task<string> ReadInPresidentAsync(string folderPathInPresident, string fileName) {
            return await ReadAsync(Application.persistentDataPath + folderPathInPresident, fileName);
        }

        /// <summary> 从PresidentPath路径下读取文件的bytes</summary>
        public async static Task<byte[]> ReadBytesInPresidentAsync(string folderPathInPresident, string fileName) {
            return await ReadBytesAsync(Application.persistentDataPath + folderPathInPresident, fileName);
        }

        public static string ReadInPresident(string folderPathInPresident, string fileName) {
            return Read(Application.persistentDataPath + folderPathInPresident, fileName);
        }

        public static byte[] ReadBytesInPresident(string folderPathInPresident, string fileName) {
            return ReadBytes(Application.persistentDataPath + folderPathInPresident, fileName);
        }

        #endregion

        #region StreamingAssets

        /// <summary>从StreamingAssets路径下读取文件的文本 </summary>
        public async static Task<string> ReadInStreamingAssetsAsync(string folderPathInStreamingAssets, string fileName) {
            return await ReadAsync(Application.streamingAssetsPath + folderPathInStreamingAssets, fileName);
        }

        public async static Task<byte[]> ReadBytesInStreamingAssetsAsync(string folderPathInStreamingAssets, string fileName) {
            return await ReadBytesAsync(Application.streamingAssetsPath + folderPathInStreamingAssets, fileName);
        }

        /// <summary>从StreamingAssets路径下读取文本 </summary>
        public static string ReadInStreamingAssets(string folderPathInStreamingAssets, string fileName) {
            return Read(Application.streamingAssetsPath + folderPathInStreamingAssets, fileName);
        }

        public static byte[] ReadBytesInStreamingAssets(string folderPathInStreamingAssets, string fileName) {
            return ReadBytes(Application.streamingAssetsPath + folderPathInStreamingAssets, fileName);
        }

        /// <summary>向StreamingAssets路径下写入文本</summary>
        public static void WriteInStreamingAssets(string folderPathInStreamingAssets, string fileName, string content) {
            Write(Application.streamingAssetsPath + folderPathInStreamingAssets, fileName, content);
        }

        public async static Task WriteInStreamingAssetsAsync(string folderPathInStreamingAssets, string fileName, string content) {
            await WriteAsync(Application.streamingAssetsPath + folderPathInStreamingAssets, fileName, content);
        }

        #endregion

        /// <summary>所选取pdb文件最大大小(3MB)</summary>
        private const int MAX_FILE_SIZE = 3145728;

        /// <summary>导航并读取用户选择的文件 </summary>
        public async static Task<byte[]> PickFile() {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            OpenFileName ofn = new OpenFileName();
            ofn.structSize = Marshal.SizeOf(ofn);
            ofn.filter = "pdb文件(*.pdb)\0*.pdb\0";
            ofn.file = new string(new char[256]);
            ofn.maxFile = ofn.file.Length;
            ofn.fileTitle = new string(new char[64]);
            ofn.maxFileTitle = ofn.fileTitle.Length;
            ofn.title = "选择文件";
            ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
            if (LocalDialog.GetOpenFileName(ofn)) {
                using (FileStream fs = new FileStream(ofn.file, FileMode.Open, FileAccess.Read)) {
                    if (fs.Length > MAX_FILE_SIZE) {
                        throw new SelectFileException(SelectFileError.OverSize);
                    }
                    byte[] file = new byte[fs.Length];
                    await fs.ReadAsync(file, 0, file.Length);
                    return file;
                }
            }
            throw new SelectFileException(SelectFileError.Cancel);
#elif UNITY_WSA_10_0 && !UNITY_EDITOR
        FileOpenPicker picker = new FileOpenPicker {
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            ViewMode = PickerViewMode.Thumbnail
        };
        picker.FileTypeFilter.Add(".pdb");
        StorageFile file = await picker.PickSingleFileAsync();
        if (file != null) {
            ulong size = (await file.GetBasicPropertiesAsync()).Size;//字节大小
            if (size > MAX_FILE_SIZE) {
                throw new SelectFileException(SelectFileError.OverSize);
            }
            else {
                string text = await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                return Encoding.UTF8.GetBytes(text);
            }
        }
        else throw new SelectFileException(SelectFileError.Cancel);
#endif
            throw new NotImplementedException(string.Format("[IOUtil.PickFile]Implemented Platform:{0}", Application.platform.ToString()));
        }

        /// <summary>获取路径下文件的MD5值</summary>
        public async static Task<string> GetFileMD5HashAsync(string filePath) {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            int len = (int)fs.Length;
            byte[] data = new byte[len];
            await fs.ReadAsync(data, 0, len);
            fs.Close();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string fileMD5 = "";
            foreach (byte b in result) {
                fileMD5 += Convert.ToString(b, 16);
            }
            return fileMD5;
        }

    }

}
