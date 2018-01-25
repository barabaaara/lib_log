using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace lib_log
{
    public class Log
    {
        #region 変数
        /// <summary>ログ出力バッファ</summary>
        private static StringBuilder buffer = new StringBuilder();
        
        /// <summary>バッファサイズ</summary>
        private static long bufferSize = Config.MaxFileSize / 3;

        /// <summary>ファイルサイズ取得時に使用</summary>
        private static FileInfo info;
        #endregion

        #region プロパティ
        // **********************
        // ***** プロパティ *****
        // **********************
        /// <summary>ログファイル名</summary>
        private static string FileName
        {
            get { return Config.BaseFileName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".log"; }
        }

        /// <summary>ログファイルパス</summary>
        /// <remarks>絶対パスを返す。</remarks>
        private static string FullPath
        {
            get { return Path.Combine(Config.LogFilePath, Log.FileName); }
        }
        #endregion

        #region コンストラクタ
        // **************************
        // ***** コンストラクタ *****
        // **************************
        static Log()
        {
        }
        #endregion

        #region Publicメソッド
        // **************************
        // ***** Publicメソッド *****
        // **************************

        /// <summary>
        /// アプリケーションのログにメッセージを書き込みます。
        /// </summary>
        /// <param name="message">出力するログメッセージ</param>
        /// <remarks>メッセージの種類は既定でTraceEventType.Information</remarks>
        public static void WriteEntry(string message)
        {
            WriteEntry(message, TraceEventType.Information);
        }

        /// <summary>
        /// アプリケーションのログにメッセージを書き込みます。
        /// </summary>
        /// <param name="message">出力するログメッセージ</param>
        /// <param name="severity">メッセージの種類</param>
        /// <remarks></remarks>
        public static void WriteEntry(string message, TraceEventType severity)
        {
            // メッセージの種類
            string StrSeverity = string.Empty;
            switch(severity)
            {
                case TraceEventType.Information:
                    StrSeverity = "Info";
                    break;
                case TraceEventType.Warning:
                    StrSeverity = "Warning";
                    break;
                case TraceEventType.Error:
                    StrSeverity = "Error";
                    break;
                case TraceEventType.Critical:
                    StrSeverity = "Critical";
                    break;
            }

            // 出力メッセージ作成
            StringBuilder outmessage = new StringBuilder();
            outmessage.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss "));
            outmessage.Append("[" + StrSeverity + "]");
            outmessage.Append("\t");
            outmessage.Append(message);

            // バッファに追加
            buffer.AppendLine(outmessage.ToString());

            // ログ出力
            if (buffer.Length > bufferSize || Config.IsAutoFlush == true)
            {
                Flush();
            }
        }

        /// <summary>
        /// バッファをログファイルへ出力
        /// </summary>
        /// <remarks>出力後バッファはクリアされる</remarks>
        public static void Flush()
        {
            // ファイルサイズの上限確認
            if (File.Exists(Log.FullPath))
            { 
                info = new FileInfo(Log.FullPath);
                long FileSize = info.Length + buffer.Length;
                if (FileSize > Config.MaxFileSize)
                {
                    string filename = Log.FullPath + ".bk";
                    int count = 1;
                    while (File.Exists(filename + count.ToString()))
                    {
                        count++;
                    }
                    info.MoveTo(filename + count.ToString());
                }
                info = null;
            }

            // ログ出力
            using (StreamWriter writer = new StreamWriter(Log.FullPath, Config.IsAppend))
            {
                writer.Write(buffer.ToString());
            }

            // バッファクリア
            buffer.Clear();
        }
        #endregion

        #region Privateメソッド
        // ***************************
        // ***** Privateメソッド *****
        // ***************************
        
        #endregion
    }
}
