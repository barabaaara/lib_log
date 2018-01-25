using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
using System.Reflection;

namespace lib_log
{
    public class Config
    {
        private static string XML_PATH = string.Empty;
        private static XDocument xdoc = null;

        // **********************
        // ***** プロパティ *****
        // **********************
        /// <summary>ログファイルパス</summary>
        /// <remarks>絶対パスで設定する</remarks>
        public static string LogFilePath
        {
            set
            {
                xdoc.XPathSelectElement("//Setting/LogFilePath").Value = value;
                xdoc.Save(XML_PATH);
            }
            get { return xdoc.XPathSelectElement("//Setting/LogFilePath").Value; }
        }

        /// <summary>ベースファイル名</summary>
        public static string BaseFileName
        {
            set
            {
                xdoc.XPathSelectElement("//Setting/BaseFileName").Value = value;
                xdoc.Save(XML_PATH);
            }
            get { return xdoc.XPathSelectElement("//Setting/BaseFileName").Value; }
        }

        /// <summary>上書き可／不可</summary>
        public static bool IsAppend
        {
            set
            {
                xdoc.XPathSelectElement("//Setting/IsAppend").Value = (value == true ? bool.TrueString : bool.FalseString);
                xdoc.Save(XML_PATH);
            }
            get { return bool.Parse(xdoc.XPathSelectElement("//Setting/IsAppend").Value); }
        }

        /// <summary>メッセージの種類</summary>
        /// <remarks>既定ではTraceEventType.Information</remarks>
        public static TraceEventType Severity
        {
            set
            {
                xdoc.XPathSelectElement("//Setting/Severity").Value = value.ToString();
                xdoc.Save(XML_PATH);
            }
            get { return (TraceEventType)int.Parse(xdoc.XPathSelectElement("//Setting/Severity").Value.ToString()); }
        }

        /// <summary>ログファイルサイズの上限</summary>
        /// <remarks>単位はキロバイト(KB)</remarks>
        public static long MaxFileSize
        {
            set
            {
                xdoc.XPathSelectElement("//Setting/MaxFileSize").Value = value.ToString();
                xdoc.Save(XML_PATH);
            }
            get { return long.Parse(xdoc.XPathSelectElement("//Setting/MaxFileSize").Value) * 1024; }
        }

        /// <summary>自動フラッシュ</summary>
        /// <remarks>明示的にFlashメソッドを呼び出さなくても自動的にフラッシュされるようにするにはtrueを設定します</remarks>
        public static bool IsAutoFlush
        {
            set
            {
                xdoc.XPathSelectElement("//Setting/IsAutoFlush").Value = (value == true ? bool.TrueString : bool.FalseString);
                xdoc.Save(XML_PATH);
            }
            get { return bool.Parse(xdoc.XPathSelectElement("//Setting/IsAutoFlush").Value); }
        }

        // **************************
        // ***** コンストラクタ *****
        // **************************
        static Config()
        {
            Config.ReadConfig();
            Config.Severity = TraceEventType.Information;
        }

        // ************************
        // ***** 定義読み込み *****
        // ************************
        public static void ReadConfig()
        {
            string CurrentPath = Directory.GetCurrentDirectory();
            string AssemblyName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location) + ".xml";
            XML_PATH = Path.Combine(CurrentPath, AssemblyName);

            xdoc = XDocument.Load(XML_PATH);
        }
    }
}
