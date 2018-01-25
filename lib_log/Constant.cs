using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_log
{
    /// <summary>
    /// トレースを発生させたイベントの種類を識別します。
    /// </summary>
    public enum TraceEventType : int
    {
        /// <summary>情報メッセージ</summary>
        Information = 0,

        /// <summary>重要でない問題</summary>
        Warning = 1,

        /// <summary>回復可能なエラー</summary>
        Error = 2,

        /// <summary>致命的なエラーまたはアプリケーションのクラッシュ</summary>
        Critical = 3
    };

    public class Constant
    {
        
    }
}
