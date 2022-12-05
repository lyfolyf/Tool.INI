using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace Lead.Tool.INI
{
    public class INIHelper
    {
        public static INIHelper iniHelper = new INIHelper();

        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileStringA(string section, string key,
           string def, Byte[] retVal, int size, string filePath);

        #endregion

        #region 读Ini文件
        public string ReadIniData(string Section, string Key, string NoText, string iniFilePath)
        {
            if (File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFilePath);
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion

        #region 写Ini文件
        public bool WriteIniData( string iniFilePath,string Section, string Key, string Value)
        {
            if (File.Exists(iniFilePath))
            {
                long OpStation = WritePrivateProfileString(Section, Key, Value, iniFilePath);
                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                FileStream fs = File.Create(iniFilePath);
                fs.Close();
                long OpStation = WritePrivateProfileString(Section, Key, Value, iniFilePath);
                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        #endregion

        #region 遍历Section
        public List<string> ReadSections(string iniFilePath)
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringA(null, null, null, buf, buf.Length, iniFilePath);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }

        #endregion
        /// <summary>
        /// 根据Section，遍历子主键
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="iniFilename"></param>
        /// <returns></returns>
        public List<string> ReadSingleSection(string Section, string iniFilename)
        {
            List<string> result = new List<string>();

            Byte[] buf = new Byte[65536];

            uint lenf = GetPrivateProfileStringA(Section, null, null, buf, buf.Length, iniFilename);

            int j = 0;

            for (int i = 0; i < lenf; i++)

                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));

                    j = i + 1;

                }
            return result;

        }


    }

    public class INIhelp
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);


        public static string GetValue(string Path, string Section, string key)
        {
            StringBuilder s = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", s, 1024, Path);
            return s.ToString();
        }


        public static void SetValue(string Path, string Section, string key, string value)
        {
            try
            {
                WritePrivateProfileString(Section, key, value, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
