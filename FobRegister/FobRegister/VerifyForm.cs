using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FobRegister
{

    public partial class VerifyForm : Form
    {
        [DllImport("kernel32.dll")]
        static extern void Sleep(int dwMilliseconds);

        //=========================== System Function =============================
        [DllImport("hfrdapi.dll")]
        static extern int Sys_Open(ref IntPtr device,
                                   UInt32 index,
                                   UInt16 vid,
                                   UInt16 pid);

        [DllImport("hfrdapi.dll")]
        static extern bool Sys_IsOpen(IntPtr device);

        [DllImport("hfrdapi.dll")]
        static extern int Sys_Close(ref IntPtr device);

        [DllImport("hfrdapi.dll")]
        static extern int Sys_SetBuzzer(IntPtr device, byte msec);

        [DllImport("hfrdapi.dll")]
        static extern int Sys_SetLight(IntPtr device, byte color);

        [DllImport("hfrdapi.dll")]
        static extern int Sys_SetAntenna(IntPtr device, byte mode);

        [DllImport("hfrdapi.dll")]
        static extern int Sys_InitType(IntPtr device, byte type);

        //======================== ISO14443A-4 Card Function =======================
        [DllImport("hfrdapi.dll")]
        static extern int TyA_Reset(IntPtr device,
                                    byte mode,
                                    byte[] pData,
                                    ref byte pMsgLg);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_Deselect(IntPtr device);

        //==========================================================================
        IntPtr g_hDevice = (IntPtr)(-1); //g_hDevice must init as -1

        #region byteHEX
        /// <summary>
        /// 单个字节转字字符.
        /// </summary>
        /// <param name="ib">字节.</param>
        /// <returns>转换好的字符.</returns>

        private bool isFlashing;
        public static String byteHEX(Byte ib)
        {
            String _str = String.Empty;
            try
            {
                char[] Digit = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A',
                'B', 'C', 'D', 'E', 'F' };
                char[] ob = new char[2];
                ob[0] = Digit[(ib >> 4) & 0X0F];
                ob[1] = Digit[ib & 0X0F];
                _str = new String(ob);
            }
            catch (Exception)
            {
                new Exception("byteHEX error !");
            }
            return _str;

        }
        #endregion

        private CancellationTokenSource cts;

        public VerifyForm()
        {
            InitializeComponent();
        }

        private void VerifyForm_Load(object sender, EventArgs e)
        {
            //Todo
        }

        private void stop(bool isError)
        {
            cts.Cancel();
            button1.Text = "连接 (C)";
            if (isError)
            {
                isFlashing = true;
                FlashingLight();
            }
        }

        private void FlashingLight()
        {
            Task.Run(async () =>
            {
                int count = 0;
                while (isFlashing)
                {
                    SetLight((byte)((count % 20 >= 10) ? 1 : 0));
                    Console.WriteLine("flashing " + count++);
                    await Task.Delay(50);
                }
                SetLight(0);
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "停止 (C)")
            {
                stop(false);
                return;
            }

            int status;
            string strError;
            button1.Text = "停止 (C)";
            new AlertForm().showAlert("连接成功", AlertForm.enmType.Success);

            //=========================== Open reader =========================
            //Check whether the reader is opened or not
            if (true == Sys_IsOpen(g_hDevice))
            {
                //If the reader is already open , close it firstly
                status = Sys_Close(ref g_hDevice);
                if (0 != status)
                {
                    strError = "Sys_Close 失败!";
                    MessageBox.Show(strError + " status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isFlashing = false;
                    return;
                }
            }

            //Open
            status = Sys_Open(ref g_hDevice, 0, 0x0416, 0x8020);
            if (0 != status)
            {
                strError = "Sys_Open 失败!";
                MessageBox.Show(strError + " status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //============= Init the reader before operating the card ============
            //Close antenna of the reader
            status = Sys_SetAntenna(g_hDevice, 0);
            if (0 != status)
            {
                strError = "Sys_SetAntenna 失败!";
                MessageBox.Show(strError + " status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Sleep(5); //Appropriate delay after Sys_SetAntenna operating 

            //Set the reader's working mode
            status = Sys_InitType(g_hDevice, (byte)'A');
            if (0 != status)
            {
                strError = "Sys_InitType 失败!";
                MessageBox.Show(strError + " status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Sleep(5); //Appropriate delay after Sys_InitType operating
            SetAntenna();
            Sleep(5); //Appropriate delay after Sys_SetAntenna operating


            //============================ Success Tips ==========================

            SetBuzzer();
            SetLight(1);
            cts = new CancellationTokenSource();
            Loop();
        }

        //Open antenna of the reader
        private void SetAntenna()
        {
            int status = Sys_SetAntenna(g_hDevice, 1);
            if (0 != status)
            {
                string strError = "Sys_SetAntenna 失败!";
                MessageBox.Show(strError + " status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //Set light - 0: no light; 1: red; 2: green; 3: yellow
        private void SetLight(byte light)
        {
            int status = Sys_SetLight(g_hDevice, light);
            if (0 != status)
            {
                string strError = "Sys_SetLight 失败!";
                MessageBox.Show(strError + " status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //Beep 200 ms
        private void SetBuzzer()
        {
            int status = Sys_SetBuzzer(g_hDevice, 20);
            if (0 != status)
            {
                string strError = "Sys_SetBuzzer 失败!";
                MessageBox.Show(strError + " status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void Loop()
        {
            Task.Run((Func<Task?>)(async () =>
            {
                int count = 0;
                while (true)
                {
                    if (cts.Token.IsCancellationRequested) break;
                    await Task.Delay(20);
                    Console.WriteLine("No. " + count++);

                    int status;
                    byte[] dataBuffer = new byte[256];
                    byte msgLen = 255;
                    byte mode = 0x52; //WUPA mode, request cards of all status

                    //Reset card
                    status = TyA_Reset(g_hDevice, mode, dataBuffer, ref msgLen);
                    if (status != 0)
                    {
                        SetLight(1);
                        this.textUID.Text = this.textKey.Text = "";
                        continue;
                    }

                    //Show  UID
                    String str = string.Empty;
                    for (int i = 0; i < 4; i++)
                    {
                        str = byteHEX(dataBuffer[i]) + str;
                    }
                    this.textUID.Text = long.Parse(str, System.Globalization.NumberStyles.HexNumber).ToString();

                    using (var db = new LocalDBContext())
                    {
                        var fob = db.Fobs.OrderByDescending<Fob, int>(f => f.Id).FirstOrDefault<Fob>(f => f.UID == str);
                        if (fob == null)
                        {
                            SetBuzzer();
                            stop(true);
                            MessageBox.Show("未找到密钥!请取出并汇报", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isFlashing = false;
                            return;
                        }
                        this.textKey.Text = "****************************" + Fob.decrypt(fob.Key).Substring(28);
                        if (!fob.Uploaded)
                        {
                            SetBuzzer();
                            stop(true);
                            MessageBox.Show("找到卡片密钥但没上传成功，请先上传", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isFlashing = false;
                            return;
                        }
                        SetLight(2);
                    }

                    status = TyA_Deselect(g_hDevice);
                    if (status != 0)
                    {
                        continue;
                        // MessageBox.Show("TyA_Deselect failed !", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // stop();
                        // return;
                    }
                }
            }));
        }

        private void VerifyForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
            {
                button1_Click(sender, e);
            }
        }

        private void VerifyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("cts:" + cts);
            if (cts != null)
                stop(false);
        }
    }
}
