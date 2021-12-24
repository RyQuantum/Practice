using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FobRegister
{

    public partial class Main : Form
    {
        [DllImport("kernel32.dll")]
        static extern void Sleep(int dwMilliseconds);

        //=========================== System Function =============================
        [DllImport("hfrdapi.dll")]
        static extern int Sys_GetDeviceNum(UInt16 vid, UInt16 pid, ref UInt32 pNum);

        [DllImport("hfrdapi.dll")]
        static extern int Sys_GetHidSerialNumberStr(UInt32 deviceIndex,
                                                    UInt16 vid,
                                                    UInt16 pid,
                                                    [Out] StringBuilder deviceString,
                                                    UInt32 deviceStringLength);

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
        static extern int Sys_SetLight(IntPtr device, byte color);

        [DllImport("hfrdapi.dll")]
        static extern int Sys_SetBuzzer(IntPtr device, byte msec);

        [DllImport("hfrdapi.dll")]
        static extern int Sys_SetAntenna(IntPtr device, byte mode);

        [DllImport("hfrdapi.dll")]
        static extern int Sys_InitType(IntPtr device, byte type);

        //=========================== Auxiliary Function ===========================
        [DllImport("hfrdapi.dll")]
        static extern int Aux_SingleDES(byte desType,
                                        byte[] key,
                                        byte[] srcData,
                                        UInt32 srcDataLen,
                                        byte[] destData,
                                        ref UInt32 destDataLen);

        [DllImport("hfrdapi.dll")]
        static extern int Aux_TripleDES(byte desType,
                                        byte[] key,
                                        byte[] srcData,
                                        UInt32 srcDataLen,
                                        byte[] destData,
                                        ref UInt32 destDataLen);

        [DllImport("hfrdapi.dll")]
        static extern int Aux_SingleMAC(byte[] key,
                                        byte[] initData,
                                        byte[] srcData,
                                        UInt32 srcDataLen,
                                        byte[] macData);

        [DllImport("hfrdapi.dll")]
        static extern int Aux_TripleMAC(byte[] key,
                                        byte[] initData,
                                        byte[] srcData,
                                        UInt32 srcDataLen,
                                        byte[] macData);

        //=========================== M1 Card Function =============================
        [DllImport("hfrdapi.dll")]
        static extern int TyA_Request(IntPtr device, byte mode, ref UInt16 pTagType);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_Anticollision(IntPtr device,
                                            byte bcnt,
                                            byte[] pSnr,
                                            ref byte pLen);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_Select(IntPtr device,
                                     byte[] pSnr,
                                     byte snrLen,
                                     ref byte pSak);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_Halt(IntPtr device);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_CS_Authentication2(IntPtr device,
                                                 byte mode,
                                                 byte block,
                                                 byte[] pKey);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_CS_Read(IntPtr device,
                                      byte block,
                                      byte[] pData,
                                      ref byte pLen);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_CS_Write(IntPtr device, byte block, byte[] pData);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_CS_InitValue(IntPtr device, byte block, Int32 value);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_CS_ReadValue(IntPtr device, byte block, ref Int32 pValue);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_CS_Decrement(IntPtr device, byte block, Int32 value);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_CS_Increment(IntPtr device, byte block, Int32 value);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_CS_Restore(IntPtr device, byte block);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_CS_Transfer(IntPtr device, byte block);

        //======================= Ultralight(C) Card Function ========================= 
        [DllImport("hfrdapi.dll")]
        static extern int TyA_UL_Select(IntPtr device, byte[] pSnr, ref byte pLen);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_UL_Write(IntPtr device, byte page, byte[] pdata);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_UL_Authentication(IntPtr device, byte[] pKey);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_UL_ChangeKey(IntPtr device, byte[] pKey);

        //======================== ISO14443A-4 Card Function =======================
        [DllImport("hfrdapi.dll")]
        static extern int TyA_Reset(IntPtr device,
                                    byte mode,
                                    byte[] pData,
                                    ref byte pMsgLg);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_CosCommand(IntPtr device,
                                         byte[] pCommand,
                                         byte cmdLen,
                                         byte[] pData,
                                         ref byte pMsgLg);

        [DllImport("hfrdapi.dll")]
        static extern int TyA_Deselect(IntPtr device);

        //==========================================================================
        IntPtr g_hDevice = (IntPtr)(-1); //g_hDevice must init as -1

        static char[] hexDigits = {
            '0','1','2','3','4','5','6','7',
            '8','9','A','B','C','D','E','F'};

        public static byte GetHexBitsValue(byte ch)
        {
            byte sz = 0;
            if (ch <= '9' && ch >= '0')
                sz = (byte)(ch - 0x30);
            if (ch <= 'F' && ch >= 'A')
                sz = (byte)(ch - 0x37);
            if (ch <= 'f' && ch >= 'a')
                sz = (byte)(ch - 0x57);

            return sz;
        }
        //

        #region byteHEX
        /// <summary>
        /// 单个字节转字字符.
        /// </summary>
        /// <param name="ib">字节.</param>
        /// <returns>转换好的字符.</returns>
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

        public static string ToHexString(byte[] bytes)
        {
            String hexString = String.Empty;
            for (int i = 0; i < bytes.Length; i++)
                hexString += byteHEX(bytes[i]);

            return hexString;
        }

        public static byte[] ToDigitsBytes(string theHex)
        {
            byte[] bytes = new byte[theHex.Length / 2 + (((theHex.Length % 2) > 0) ? 1 : 0)];
            for (int i = 0; i < bytes.Length; i++)
            {
                char lowbits = theHex[i * 2];
                char highbits;

                if ((i * 2 + 1) < theHex.Length)
                    highbits = theHex[i * 2 + 1];
                else
                    highbits = '0';

                int a = (int)GetHexBitsValue((byte)lowbits);
                int b = (int)GetHexBitsValue((byte)highbits);
                bytes[i] = (byte)((a << 4) + b);
            }

            return bytes;
        }

        public List<Fob> Fobs;
        private string accessToken;
        private HttpClient httpClient = new HttpClient { Timeout = TimeSpan.FromMilliseconds(30000) };
        private DialogResult dialogResult;
        private Boolean isFlashing;
        private int displayCount = 20;
        private int lastUploadId = 0;
        private int preFirstDisplayFobId = 0;
        private DataTable uploadingFobs = new DataTable();
        private List<Fob> displayingFobs;
        private bool isDisplayUploading = true;
        private bool isLaunch = true;
        /**/
        public Main(string token)
        {
            accessToken = token;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            InitializeComponent();
            uploadingFobs.Columns.Add("Id");
            uploadingFobs.Columns.Add("UID");
            uploadingFobs.Columns.Add("Status");
            uploadingFobs.Columns.Add("Count");
            uploadingFobs.Columns.Add("remove");
            uploadingFobs.Columns.Add("fobNumber");
            uploadingFobs.Columns.Add("key");
            uploadingFobs.Columns.Add("isManually");
            uploadingFobs.PrimaryKey = new DataColumn[] { uploadingFobs.Columns["Id"] };
        }

        public void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (var db = new LocalDBContext())
            {
                var fobList = db.Fobs.Where(f => f.Uploaded == false).ToList();
                if (fobList.Count != 0)
                {
                    dialogResult = MessageBox.Show($"有{fobList.Count}张卡还没被上传，确定要退出吗？", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    e.Cancel = (dialogResult == DialogResult.Cancel);
                }
            }
        }

        private void FM1208_Load(object sender, EventArgs e)
        {
            textKey.MaxLength = 32;
            textKey.Text = "FFFFFFFFFFFFFFFF";
            LoadHistory();
            dataGridView2.DataSource = uploadingFobs;
        }

        private void LoadHistory()
        {
            using (var db = new LocalDBContext())
            {
                var lastFob = db.Fobs.OrderByDescending(f => f.Id).FirstOrDefault();
                if (lastFob == null) return;
                var firstUnuploadedFob = db.Fobs.FirstOrDefault(f => f.Uploaded == false);
                var lastUploadedFobId = firstUnuploadedFob == null ? lastFob.Id : firstUnuploadedFob.Id - 1;
                if (lastFob.Id - lastUploadedFobId >= displayCount)
                {
                    lastUploadId = preFirstDisplayFobId = lastUploadedFobId;
                }
                else
                {
                    var list = db.Fobs.OrderByDescending(f => f.Id).Take(displayCount).ToList();
                    list.Reverse();
                    preFirstDisplayFobId = list[0].Id - 1;
                    lastUploadId = lastUploadedFobId;
                }

                var fobList = db.Fobs.Where(fob => fob.Id > preFirstDisplayFobId).ToList();

                fobList.FindAll(fob => !fob.Uploaded).ForEach(fob => UploadKey(fob, null));
                BindDateSource(fobList);
            }
        }

        private async Task<JObject> UploadAPI(string UID, string key)
        {
            Console.WriteLine("uploading " + UID);
            try
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync("https://keyless.rentlyopensesame.com/api/fobs/uploadFob", new FobObj(UID, key));
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    do
                    {
                        SetBuzzer();
                        dialogResult = MessageBox.Show("Access token 过期，请重启应用程序。", "error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                    } while (dialogResult == DialogResult.Cancel);
                    Application.Exit();
                }
                var responseString = await response.Content.ReadAsStringAsync();
                JObject res = JObject.Parse(responseString);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async void UploadKey(Fob fob, DataRow dr)
        {
            var fobRecord = uploadingFobs.Rows.Find(fob.Id);
            if (fobRecord != null)
            {
                dr = fobRecord;
                if (dr["isManually"].ToString() == "True" || (string)dr["Count"] == "3")
                    return;
            }
            if (dr == null)
            {
                dr = uploadingFobs.NewRow();
                dr["Id"] = fob.Id;
                dr["UID"] = fob.DecID;
                dr["Status"] = "Sending...";
                dr["Count"] = 1;
                dr["fobNumber"] = fob.UID;
                dr["key"] = fob.DecryptedKey;
                uploadingFobs.Rows.Add(dr);
                dataGridView2.ClearSelection();
                dataGridView2.CurrentCell = null;
            }
            else
            {
                dr["Count"] = Int32.Parse(dr["Count"].ToString()) + 1;
            }
            try
            {
                Debug.WriteLine("uploading fobNumber: " + dr["fobNumber"].ToString() + ", Key: " + fob.Key);
                JObject res = await UploadAPI(dr["fobNumber"].ToString(), dr["key"].ToString());
                if ((bool)res["success"])
                {
                    UploadSuccessAndMarkIt(fob.Id);
                    dr.Delete();
                }
                else
                {
                    var msg = (string)res["message"];
                    if (!isLaunch && msg == "fobNumber " + dr["fobNumber"] + " already exists")
                    {
                        dr["Status"] = "服务器: " + msg;
                        FlashingLight();
                        do
                        {
                            SetBuzzer();
                            dataGridView2.ClearSelection();
                            dataGridView2.CurrentCell = null;
                            dialogResult = MessageBox.Show("服务器: " + (string)res["message"] + ". 扔掉并点击\"删除\"", "error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                        } while (dialogResult == DialogResult.Cancel);
                        isFlashing = false;
                        // RemoveFob(fob.Id);
                        return;
                    }
                    dr["Status"] = "服务器: " + msg + ((string)dr["Count"] == "3" ? "" : ";重试中...");

                    Console.WriteLine("upload " + fob.UID + " failed. Resending... ");
                    UploadKey(fob, dr);
                }
            }
            catch (TaskCanceledException ex)
            {
                dr["Status"] = "Upload timeout, retrying...";
                Console.WriteLine("Upload " + fob.UID + " timeout. Resending...");
                UploadKey(fob, dr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveFob(int id)
        {
            using (var db = new LocalDBContext())
            {
                Fob fob = new Fob() { Id = id };
                db.Fobs.Attach(fob);
                db.Fobs.Remove(fob);
                db.SaveChanges();

                var fobsList = db.Fobs.Where(f => f.Id > preFirstDisplayFobId).ToList();
                Console.WriteLine($"{DateTime.Now}, found {fobsList.Count}");
                BindDateSource(fobsList);
            }
            // uploadingFobIds.Remove(id);
        }

        private void UploadSuccessAndMarkIt(int id)
        {
            if (isDisplayUploading == true) dataGridView2.Focus();
            else dataGridView1.Focus();

            using (var db = new LocalDBContext())
            {
                var fob = db.Fobs.FirstOrDefault(f => f.Id == id);
                fob.Uploaded = true;
                int count = db.SaveChanges();
                Console.WriteLine($"{DateTime.Now}, mark {fob.UID} done");

                var newFobsList = db.Fobs.Where(f => f.Id > preFirstDisplayFobId).ToList();
                Console.WriteLine($"{DateTime.Now}, found {newFobsList.Count}");

                BindDateSource(newFobsList);
            }
        }
        private void BindDateSource(List<Fob> dataList)
        {
            if (dataList.Count == 0)
            {
                dataGridView1.DataSource = null;
                return;
            }
            Console.WriteLine("key: " + dataList[dataList.Count - 1].Key);
            dataGridView1.DataSource = displayingFobs = dataList;
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];
        }

        private void FlashingLight()
        {
            if (isFlashing) return;
            isFlashing = true;
            Task.Run(async () =>
            {
                int count = 0;
                while (isFlashing)
                {
                    SetLight((byte)((count % 20 >= 10) ? 1 : 0));
                    Console.WriteLine("flashing " + count++);
                    await Task.Delay(50);
                }
            });
        }

        //Open/Close antenna of the reader
        private void SetAntenna(byte v)
        {
            int status = Sys_SetAntenna(g_hDevice, v);
            if (0 != status)
            {
                string strError = "Sys_SetAntenna 失败! status: " + status;
                MessageBox.Show(strError, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //Set light - 0: no light; 1: red; 2: green; 3: yellow
        private void SetLight(byte light)
        {
            lock (this)
            {
                int status = Sys_SetLight(g_hDevice, light);
                if (0 != status)
                {
                    string strError = "Sys_SetLight 失败! status: " + status;
                    MessageBox.Show(strError, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        //Beep 200 ms
        private void SetBuzzer()
        {
            lock (this)
            {
                int status = Sys_SetBuzzer(g_hDevice, 20);
                if (0 != status)
                {
                    string strError = "Sys_SetBuzzer 失败! status: " + status;
                    MessageBox.Show(strError, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void OpenReader()
        {/*toolbar button[connect] clicked*/
            int status;
            string strError;
            //=========================== Open reader =========================
            //Check whether the reader is opened or not
            if (true == Sys_IsOpen(g_hDevice))
            {
                //If the reader is already open , close it firstly
                status = Sys_Close(ref g_hDevice);
                if (0 != status)
                {
                    strError = "Sys_Close 失败! status: " + status;
                    MessageBox.Show(strError, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //Open
            status = Sys_Open(ref g_hDevice, 0, 0x0416, 0x8020);
            if (0 != status)
            {
                strError = "Sys_Open 失败! status: " + status;
                MessageBox.Show(strError, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void tsbtnConnect_Click(object sender, EventArgs e)
        {
            isLaunch = false;
            int status;
            string strError;
            OpenReader();

            //============= Init the reader before operating the card ============
            SetAntenna(0);
            Sleep(5); //Appropriate delay after Sys_SetAntenna operating 

            //Set the reader's working mode
            status = Sys_InitType(g_hDevice, (byte)'A');
            if (0 != status)
            {
                strError = "Sys_InitType 失败! status: " + status;
                MessageBox.Show(strError, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Sleep(5); //Appropriate delay after Sys_InitType operating
            SetAntenna(1);
            Sleep(5); //Appropriate delay after Sys_SetAntenna operating
            SetBuzzer();
            SetLight(1);
            new AlertForm().showAlert("连接成功", AlertForm.enmType.Success);
            textUID.Text = "";
            if (isDisplayUploading == true) dataGridView2.Focus();
            else dataGridView1.Focus();
        }

        private void Loop()
        {
            Task.Run(() =>
            {
                int count = 0;
                while (!isFlashing)
                {
                    lock (this)
                    {

                        Console.WriteLine("fetch: " + count++);
                        int status;
                        byte[] dataBuffer = new byte[256];
                        byte msgLen = 255;
                        byte mode = 0x52; //WUPA mode, request cards of all status
                        //Reset card
                        status = TyA_Reset(g_hDevice, mode, dataBuffer, ref msgLen);
                        if (status != 0) break;

                        String str = String.Empty;
                        for (int i = 0; i < 4; i++)
                        {
                            str = byteHEX(dataBuffer[i]) + str;
                        }
                        str = Int64.Parse(str, System.Globalization.NumberStyles.HexNumber).ToString();
                        if (str != textUID.Text) break;

                        //Deselect card
                        status = TyA_Deselect(g_hDevice);
                        if (status != 0) break;
                    }
                    Task.Delay(100);
                }
                if (!isFlashing) SetLight(1);
                //textUID.Text = "";
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int status;
            byte[] dataBuffer = new byte[256];
            byte[] dataBuffer2 = new byte[256];
            byte[] arrCommand = new byte[26];
            byte cmdLen = 255;
            byte msgLen = 255;
            uint dwLen = 0;
            byte mode = 0x52; //WUPA mode, request cards of all status

            lock (this)
            {

                //Reset card
                status = TyA_Reset(g_hDevice, mode, dataBuffer, ref msgLen);
                if (status != 0)
                {
                    SetBuzzer();
                    MessageBox.Show("TyA_Reset 失败! status: " + status + " 请将门卡放到发卡器上。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Show  UID
                String str = String.Empty;
                for (int i = 0; i < 4; i++)
                {
                    str = byteHEX(dataBuffer[i]) + str;
                }
                Console.WriteLine("UID: " + Int64.Parse(str, System.Globalization.NumberStyles.HexNumber).ToString());
                textUID.Text = Int64.Parse(str, System.Globalization.NumberStyles.HexNumber).ToString();

                //using (var db = new LocalDBContext())
                //{
                //    var fob = db.Fobs.FirstOrDefault(f => f.UID == str);
                //    if (fob != null)
                //    {
                //        do
                //        {
                //            SetBuzzer();
                //            dialogResult = MessageBox.Show("卡号重复: " + textUID.Text + "! 取出并汇报", "error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                //        } while (dialogResult == DialogResult.Cancel);
                //        return;
                //    }
                //}

                //Get key
                byte[] arrKey = ToDigitsBytes(textKey.Text);

                //Get random number from card
                arrCommand[0] = 0x00;
                arrCommand[1] = 0x84;
                arrCommand[2] = 0x00;
                arrCommand[3] = 0x00;
                arrCommand[4] = 0x04;
                cmdLen = 5;

                status = TyA_CosCommand(g_hDevice, arrCommand, cmdLen, dataBuffer, ref msgLen);
                if (status != 0)
                {
                    MessageBox.Show("随机数获取失败! status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                dataBuffer[4] = 0x00;
                dataBuffer[5] = 0x00;
                dataBuffer[6] = 0x00;
                dataBuffer[7] = 0x00;

                //Encrypt random numbers
                if (textKey.TextLength == 16)
                {
                    Aux_SingleDES(0, arrKey, dataBuffer, 8, dataBuffer2, ref dwLen);
                }
                else if (textKey.TextLength == 32)
                {
                    Aux_TripleDES(0, arrKey, dataBuffer, 8, dataBuffer2, ref dwLen);
                }

                //External Authentication
                arrCommand[0] = 0x00;  //CLA
                arrCommand[1] = 0x82;  //INS
                arrCommand[2] = 0x00;  //P1
                arrCommand[3] = 0x00; //Key ID
                arrCommand[4] = 0x08;  //length of the data that follows
                for (int i = 0; i < 8; i++)
                {
                    arrCommand[i + 5] = dataBuffer2[i]; //8-byte encrypted random numbers
                }
                cmdLen = 5 + 8; //length of COS command

                status = TyA_CosCommand(g_hDevice, arrCommand, cmdLen, dataBuffer, ref msgLen);
                if (status != 0)
                {
                    MessageBox.Show("External authentication 失败! status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dataBuffer[0] != 0x90 && dataBuffer[1] != 0x00)
                {
                    do
                    {
                        SetBuzzer();
                        dialogResult = MessageBox.Show("密钥错误! 请取出并汇报。", "error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                    } while (dialogResult == DialogResult.Cancel);
                    return;
                }

                //Clean data
                arrCommand[0] = 0x80;
                arrCommand[1] = 0x0E;
                arrCommand[2] = 0x00;
                arrCommand[3] = 0x00;
                arrCommand[4] = 0x04;
                cmdLen = 5;

                status = TyA_CosCommand(g_hDevice, arrCommand, cmdLen, dataBuffer, ref msgLen);
                if (status != 0)
                {
                    MessageBox.Show("Clean data 失败! status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dataBuffer[0] != 0x90 && dataBuffer[1] != 0x00)
                {
                    MessageBox.Show("Clean data 不成功! dataBuffer: 0x" + BitConverter.ToString(dataBuffer).Replace("-", ""), "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Create key file
                arrCommand[0] = 0x80;
                arrCommand[1] = 0xE0;
                arrCommand[2] = 0x00;
                arrCommand[3] = 0x00;
                arrCommand[4] = 0x07;
                arrCommand[5] = 0x3F;
                arrCommand[6] = 0x00;
                arrCommand[7] = 0x50;
                arrCommand[8] = 0x01;
                arrCommand[9] = 0xF1;
                arrCommand[10] = 0xFF;
                arrCommand[11] = 0xFF;
                cmdLen = 12;

                status = TyA_CosCommand(g_hDevice, arrCommand, cmdLen, dataBuffer, ref msgLen);
                if (status != 0)
                {
                    MessageBox.Show("Create key file 失败! status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dataBuffer[0] != 0x90 && dataBuffer[1] != 0x00)
                {
                    MessageBox.Show("Create key file 不成功! dataBuffer: 0x" + BitConverter.ToString(dataBuffer).Replace("-", ""), "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Fill key
                Random random = new Random();
                random.NextBytes(arrCommand);
                arrCommand[0] = 0x80;
                arrCommand[1] = 0xD4;
                arrCommand[2] = 0x01;
                arrCommand[3] = 0x00;

                //arrCommand[4] = 0x0D;
                arrCommand[4] = 0x15;

                arrCommand[5] = 0x39;
                arrCommand[6] = 0xF0;
                arrCommand[7] = 0xF1;
                arrCommand[8] = 0xAA;
                arrCommand[9] = 0xFF;

                //arrCommand[10] = 0xFF;
                //arrCommand[11] = 0xFF;
                //arrCommand[12] = 0xFF;
                //arrCommand[13] = 0xFF;
                //arrCommand[14] = 0xFF;
                //arrCommand[15] = 0xFF;
                //arrCommand[16] = 0xFF;
                //arrCommand[17] = 0xFF;
                arrCommand[10] = 0x6E;
                arrCommand[11] = 0xE9;
                arrCommand[12] = 0xCE;
                arrCommand[13] = 0x33;
                arrCommand[14] = 0xDF;
                arrCommand[15] = 0xA0;
                arrCommand[16] = 0xA8;
                arrCommand[17] = 0x58;
                arrCommand[18] = 0x71;
                arrCommand[19] = 0x98;
                arrCommand[20] = 0xD4;
                arrCommand[21] = 0x1C;
                arrCommand[22] = 0xEC;
                arrCommand[23] = 0x66;
                arrCommand[24] = 0x13;
                arrCommand[25] = 0x28;
                //cmdLen = 18;
                cmdLen = 26;
                Console.WriteLine(BitConverter.ToString(arrCommand).Replace("-", ""));
                var key = BitConverter.ToString(arrCommand.Skip(10).Take(16).ToArray()).Replace("-", "");

                using (var db = new LocalDBContext())
                {
                    db.Fobs.Add(new Fob() { UID = str, Key = Fob.encrypt(key) });
                    int count = db.SaveChanges();
                    Console.WriteLine($"{DateTime.Now}, saved {count}");
                }
                status = TyA_CosCommand(g_hDevice, arrCommand, cmdLen, dataBuffer, ref msgLen);
                if (status != 0)
                {
                    MessageBox.Show("Fill key 失败! status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dataBuffer[0] != 0x90 && dataBuffer[1] != 0x00)
                {
                    MessageBox.Show("Fill key 不成功! dataBuffer: 0x" + BitConverter.ToString(dataBuffer).Replace("-", ""), "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                status = TyA_Deselect(g_hDevice);
                if (status != 0)
                {
                    MessageBox.Show("TyA_Deselect 失败! status: " + status, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                SetLight(2);
                new AlertForm().showAlert("分配" + Int64.Parse(str, System.Globalization.NumberStyles.HexNumber).ToString() + "密钥成功", AlertForm.enmType.Success);
            }

            if (isDisplayUploading == true) dataGridView2.Focus();
            else dataGridView1.Focus();
            LoadHistory();
            Loop();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                button1_Click(sender, e);
            }
            else if (e.KeyCode == Keys.C)
            {
                tsbtnConnect_Click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isDisplayUploading)
            {
                dataGridView1.DataSource = displayingFobs;
                dataGridView1.Show();
                dataGridView2.Hide();
            }
            else
            {
                dataGridView2.DataSource = uploadingFobs;
                dataGridView2.Show();
                dataGridView1.Hide();
            }
            isDisplayUploading = !isDisplayUploading;
            button2.Text = isDisplayUploading ? "查看卡" : "查看上传";
        }

        private async void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("row: " + e.RowIndex + " column: " + e.ColumnIndex);
            DataRow dr = uploadingFobs.Rows[e.RowIndex];
            if (e.ColumnIndex == 5)
            {
                RemoveFob(Int32.Parse(dr["Id"].ToString()));
                dr.Delete();
                dataGridView2.ClearSelection();
                dataGridView2.CurrentCell = null;
            }
            else if (e.ColumnIndex == 0)
            {
                dr["isManually"] = true;
                dr["Count"] = Int32.Parse(dr["Count"].ToString()) + 1;
                dr["Status"] = "重试中...";
                try
                {
                    int Id = Int32.Parse(dr["Id"].ToString());
                    string fobNumber = dr["fobNumber"].ToString();
                    string key = dr["key"].ToString();

                    JObject res = await UploadAPI(fobNumber, key);
                    if ((bool)res["success"])
                    {
                        UploadSuccessAndMarkIt(Id);
                        dr.Delete();
                    }
                    else
                    {
                        dr["Status"] = "服务器: " + res["message"];
                    }
                }
                catch (Exception ex)
                {
                    //TODO
                    dr["Status"] = "失败。ex.message:" + ex.Message;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form form = new VerifyForm();
            form.ShowDialog();
        }
    }
}
