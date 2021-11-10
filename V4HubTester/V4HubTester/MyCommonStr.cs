using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V4HubTester
{
    internal class MyCommonStr
    {
        public const string OpenSerialFirst = "请先点击\"打开串口\" ！！";
        public const string OpenString = "打开串口";
        public const string CloseString = "关闭串口";
        public const string StopSendString = "停止发送";
        public const string SendString = "发送数据";
        public const string SendMsgIsNull = "Please input the message to be sent!";
        public const string SerialCloseErrorMsg = "打开串口失败，请检查是否存在已连接串口";
        public static readonly Color NormalColor = Color.FromArgb(51, 71, 91);
        public static readonly Color BtnCancelColor = Color.FromArgb(144, 238, 144);
    }
}
