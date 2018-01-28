using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;


namespace JBBCounter
{
    public partial class Form1 : Form
    {

        public List<Product> listProd = new List<Product>();
        List<string> listDBCard = new List<string>();
        List<string> listLastCard = new List<string>();
        public List<string> lstTotalRfids = new List<string>();
        public List<string> lstLeaveRfids = new List<string>();
        int intInterval;
        string apiAddress;



        //webSocketServer wss = new webSocketServer();

        public UIntPtr hreader;
        public UIntPtr hTag;
        public Byte enableAFI;
        public Byte AFI;
        public Byte onlyNewTag;
        public UInt32 antennaCount;
        public bool _shouldStop;
        public Byte readerType;
        public Byte[] AntennaSel;
        public Byte AntennaSelCount;
        public ArrayList readerDriverInfoList;

        Thread InvenThread;

        delegate void HandleInterfaceReport(string uidStr, string blockDataStr); //委托处理接收数据
        delegate void HandleInterfaceReport123(string uidStr, string uptr, string blockDataStr); //委托处理接收数据

        HandleInterfaceReport tagReportHandler;
        HandleInterfaceReport123 tagReportHandler123;

        public Form1()
        {
            InitializeComponent();

            tagReportHandler = new HandleInterfaceReport(tagReport);//实例化委托对象（处理接收数据）
            tagReportHandler123 = new HandleInterfaceReport123(tagReport123);//实例化委托对象（处理接收数据）

            enableAFI = 0;
            AFI = 0;
            onlyNewTag = 0;
            antennaCount = 0;
            _shouldStop = true;
            readerType = 0;
            AntennaSel = new byte[16];
            AntennaSelCount = 0;
            readerDriverInfoList = new ArrayList();



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /* Load all reader driver dll, like "rfidlib_ANRD201.dll" */
            RFIDLIB.rfidlib_reader.RDR_LoadReaderDrivers("\\Drivers");
            /* enum and show loaded reader driver */
            UInt32 nCount;
            nCount = RFIDLIB.rfidlib_reader.RDR_GetLoadedReaderDriverCount();
            uint i;
            for (i = 0; i < nCount; i++)
            {
                UInt32 nSize;
                CReaderDriverInf driver = new CReaderDriverInf();
                StringBuilder strCatalog = new StringBuilder();
                strCatalog.Append('\0', 64);

                nSize = (UInt32)strCatalog.Capacity;
                RFIDLIB.rfidlib_reader.RDR_GetLoadedReaderDriverOpt(i, RFIDLIB.rfidlib_def.LOADED_RDRDVR_OPT_CATALOG, strCatalog, ref nSize);
                driver.m_catalog = strCatalog.ToString();
                if (driver.m_catalog == RFIDLIB.rfidlib_def.RDRDVR_TYPE_READER) // Only reader we need
                {
                    StringBuilder strName = new StringBuilder();
                    strName.Append('\0', 64);
                    nSize = (UInt32)strName.Capacity;
                    RFIDLIB.rfidlib_reader.RDR_GetLoadedReaderDriverOpt(i, RFIDLIB.rfidlib_def.LOADED_RDRDVR_OPT_NAME, strName, ref nSize);
                    driver.m_name = strName.ToString();

                    StringBuilder strProductType = new StringBuilder();
                    strProductType.Append('\0', 64);
                    nSize = (UInt32)strProductType.Capacity;
                    RFIDLIB.rfidlib_reader.RDR_GetLoadedReaderDriverOpt(i, RFIDLIB.rfidlib_def.LOADED_RDRDVR_OPT_ID, strProductType, ref nSize);
                    driver.m_productType = strProductType.ToString();

                    StringBuilder strCommSupported = new StringBuilder();
                    strCommSupported.Append('\0', 64);
                    nSize = (UInt32)strCommSupported.Capacity;
                    RFIDLIB.rfidlib_reader.RDR_GetLoadedReaderDriverOpt(i, RFIDLIB.rfidlib_def.LOADED_RDRDVR_OPT_COMMTYPESUPPORTED, strCommSupported, ref nSize);
                    driver.m_commTypeSupported = (UInt32)int.Parse(strCommSupported.ToString());

                    readerDriverInfoList.Add(driver);
                }
            }

            for (i = 0; i < readerDriverInfoList.Count; i++)
            {
                CReaderDriverInf drv = (CReaderDriverInf)(readerDriverInfoList[(int)i]);
                cb_Drivers.Items.Add(drv.m_name);
            }
            if (cb_Drivers.Items.Count > 0) cb_Drivers.SelectedIndex = 0;

            /* enum PC serial ports */
            cb_ports.Items.Clear();
            UInt32 nCOMCnt = RFIDLIB.rfidlib_reader.COMPort_Enum();
            for (i = 0; i < nCOMCnt; i++)
            {
                StringBuilder comName = new StringBuilder();
                comName.Append('\0', 64);
                RFIDLIB.rfidlib_reader.COMPort_GetEnumItem(i, comName, (UInt32)comName.Capacity);
                cb_ports.Items.Add(comName);
            }


            initCom();



        }

        private void b_open_Click(object sender, EventArgs e)
        {
            int iret = 0;

            readerType = (Byte)cb_Drivers.SelectedIndex;
            string readerDriverName = ((CReaderDriverInf)(readerDriverInfoList[readerType])).m_name;
            string connstr = "";
            connstr = RFIDLIB.rfidlib_def.CONNSTR_NAME_RDTYPE + "=" + readerDriverName + ";" +
                        RFIDLIB.rfidlib_def.CONNSTR_NAME_COMMTYPE + "=" + RFIDLIB.rfidlib_def.CONNSTR_NAME_COMMTYPE_COM + ";" +
                        RFIDLIB.rfidlib_def.CONNSTR_NAME_COMNAME + "=" + cb_ports.Text + ";" +
                        RFIDLIB.rfidlib_def.CONNSTR_NAME_COMBARUD + "=" + "38400" + ";" +
                        RFIDLIB.rfidlib_def.CONNSTR_NAME_COMFRAME + "=" + "8E1" + ";" +
                        RFIDLIB.rfidlib_def.CONNSTR_NAME_BUSADDR + "=" + "255";

            iret = RFIDLIB.rfidlib_reader.RDR_Open(connstr, ref hreader);
            if (iret != 0)
            {
                MessageBox.Show("fail");
            }
            else
            {
                //antennaCount = RFIDLIB.rfidlib_reader.RDR_GetAntennaInterfaceCount(hreader);
                //antennaCount = (uint)(antennaCount > 1 ? 2 : 0);//如果天线多于1个，则开1、2口
                b_Inventory.Enabled = true;
                b_stopInventory.Enabled = true;
                b_open.Enabled = false;
                b_close.Enabled = true;
            }
        }

        private void b_close_Click(object sender, EventArgs e)
        {
            int iret = 0;
            if (hTag != (UIntPtr)0)
            {
                MessageBox.Show("disconnect from tag first!");
                return;
            }

            iret = RFIDLIB.rfidlib_reader.RDR_Close(hreader);
            if (iret == 0)
            {
                hreader = (UIntPtr)0;
                b_open.Enabled = true;
                b_close.Enabled = false;
            }
            else
            {
                MessageBox.Show("fail");
            }
        }

        private void b_Inventory_Click(object sender, EventArgs e)
        {
            _shouldStop = false;
            //AFI
            enableAFI = 0;
            //Only new tag
            onlyNewTag = 0; /* 0 or 1*/

            //InvenThread = new Thread(DoInventory);
            InvenThread = new Thread(DoInventory123);
            InvenThread.Start();
        }

        public void DoInventory()
        {
            Byte AIType = RFIDLIB.rfidlib_def.AI_TYPE_NEW;
            UIntPtr InvenParamSpecList = UIntPtr.Zero;
            InvenParamSpecList = RFIDLIB.rfidlib_reader.RDR_CreateInvenParamSpecList();
            if (InvenParamSpecList.ToUInt64() != 0)
            {
                RFIDLIB.rfidlib_aip_iso15693.ISO15693_CreateInvenParam(InvenParamSpecList, 0, enableAFI, AFI, 0);
            }

            DateTime startTime;
            DateTime endTime;
            TimeSpan t;

            
            while (!_shouldStop)
            {
                startTime = DateTime.Now;
                // antenna selection
                /*if (iCount > antennaCount) { iCount = 0; }
                AntennaSel[0] = (Byte)(iCount + 1);
                AntennaSelCount = (Byte)1;
                iCount++;
                */
                /*iCount = 0; // 0：天线1，1：天线2
                AntennaSel[0] = (Byte)(iCount + 1);
                AntennaSelCount = (Byte)1;
                */


                UInt32 nTagCount = 0;
                int iret;
                LABEL_TAG_INVENTORY:
                iret = RFIDLIB.rfidlib_reader.RDR_TagInventory(hreader, AIType, AntennaSelCount, AntennaSel, InvenParamSpecList);
                if (iret == 0 || iret == -21)
                {
                    object[] pList0 = { null, null };
                    Invoke(tagReportHandler, pList0);

                    nTagCount = RFIDLIB.rfidlib_reader.RDR_GetTagDataReportCount(hreader);
                    object[] pList1 = { "count", nTagCount.ToString() };
                    Invoke(tagReportHandler, pList1);

                    UIntPtr TagDataReport;
                    TagDataReport = (UIntPtr)0;
                    TagDataReport = RFIDLIB.rfidlib_reader.RDR_GetTagDataReport(hreader, RFIDLIB.rfidlib_def.RFID_SEEK_FIRST); //first
                    while (TagDataReport.ToUInt64() > 0)
                    {
                        UInt32 aip_id = 0;
                        UInt32 tag_id = 0;
                        UInt32 ant_id = 0;
                        Byte dsfid = 0;
                        Byte[] uid = new Byte[16];
                        iret = RFIDLIB.rfidlib_aip_iso15693.ISO15693_ParseTagDataReport(TagDataReport, ref aip_id, ref tag_id, ref ant_id, ref dsfid, uid);
                        if (iret == 0)
                        {
                            string uidStr = BitConverter.ToString(uid).Replace("-", string.Empty).Substring(0, 16);
                            if (cb_blockToRead.Checked || cb_blockToWrite.Checked)// 读写块数据
                            {
                                Byte addrMode = 1;
                                iret = RFIDLIB.rfidlib_aip_iso15693.ISO15693_Connect(hreader, RFIDLIB.rfidlib_def.RFID_ISO15693_PICC_ICODE_SLI_ID, addrMode, uid, ref hTag);
                                if (iret == 0)
                                {
                                    // 读块 -----------------------------------------------
                                    if (cb_blockToRead.Checked)
                                    {
                                        int idx;
                                        UInt32 blockAddr;
                                        UInt32 blockToRead;
                                        UInt32 blocksRead = 0;

                                        idx = 0;
                                        blockAddr = (UInt32)idx;
                                        //idx = 2; // 1 <= idx <= 8
                                        idx = (int)nud_blockToRead.Value;
                                        blockToRead = (UInt32)(idx);

                                        UInt32 nSize;
                                        Byte[] BlockBuffer = new Byte[40];

                                        nSize = (UInt32)BlockBuffer.GetLength(0);
                                        UInt32 bytesRead = 0;
                                        iret = RFIDLIB.rfidlib_aip_iso15693.ISO15693_ReadMultiBlocks(hreader, hTag, 1, blockAddr, blockToRead, ref blocksRead, BlockBuffer, nSize, ref bytesRead);
                                        if (iret == 0)
                                        {
                                            // blocksRead: blocks read                                     
                                            string blockDataStr = BitConverter.ToString(BlockBuffer, 0, (int)bytesRead).Replace("-", string.Empty);
                                            object[] pList = { uidStr, "R:" + blockDataStr };
                                            Invoke(tagReportHandler, pList);
                                        }
                                        else
                                        {
                                            object[] pList = { "error", iret.ToString() };
                                            Invoke(tagReportHandler, pList);
                                        }
                                    }
                                    // 写块 ------------------------------------------------
                                    if (cb_blockToWrite.Checked)
                                    {
                                        int idx;
                                        UInt32 blkAddr;
                                        UInt32 numOfBlks;
                                        idx = (int)nud_blockToWrite.Value;
                                        blkAddr = (UInt32)idx;
                                        idx = tb_blockToWrite.Text.Length / 8;
                                        numOfBlks = (UInt32)(idx + 1);
                                        byte[] newBlksData = StringToByteArrayFastest(tb_blockToWrite.Text + "00000000");

                                        iret = RFIDLIB.rfidlib_aip_iso15693.ISO15693_WriteMultipleBlocks(hreader, hTag, blkAddr, numOfBlks, newBlksData, (uint)newBlksData.Length);
                                        if (iret == 0)
                                        {
                                            object[] pList = { uidStr, "W:" + tb_blockToWrite.Text };
                                            Invoke(tagReportHandler, pList);
                                        }
                                        else
                                        {
                                            object[] pList = { "error", iret.ToString() };
                                            Invoke(tagReportHandler, pList);
                                        }
                                    }
                                    //-------------------------------------------------------
                                }
                                iret = RFIDLIB.rfidlib_reader.RDR_TagDisconnect(hreader, hTag);
                                if (iret == 0)
                                {
                                    hTag = (UIntPtr)0;
                                }
                            }
                            else// 不读写块数据
                            {

                                object[] pList = { uidStr, null };
                                Invoke(tagReportHandler, pList);
                            }
                        }
                        TagDataReport = RFIDLIB.rfidlib_reader.RDR_GetTagDataReport(hreader, RFIDLIB.rfidlib_def.RFID_SEEK_NEXT); //next
                    }
                }
                if (iret == -21) // stop trigger occur,need to inventory left tags
                {
                    AIType = RFIDLIB.rfidlib_def.AI_TYPE_CONTINUE;//use only-new-tag inventory 
                    goto LABEL_TAG_INVENTORY;
                }
                iret = 0;

                // 计算消耗时间 -------------------------------------------
                endTime = DateTime.Now;
                t = endTime - startTime;
                object[] pList2 = { "耗时", t.TotalSeconds.ToString() + "   标签数量：" + nTagCount.ToString() };
                Invoke(tagReportHandler, pList2);
            }
            RFIDLIB.rfidlib_reader.RDR_ResetCommuImmeTimeout(hreader);
        }


        public void DoInventory123()
        {
            Byte AIType = RFIDLIB.rfidlib_def.AI_TYPE_NEW;
            UIntPtr InvenParamSpecList = UIntPtr.Zero;
            InvenParamSpecList = RFIDLIB.rfidlib_reader.RDR_CreateInvenParamSpecList();
            if (InvenParamSpecList.ToUInt64() != 0)
            {
                RFIDLIB.rfidlib_aip_iso15693.ISO15693_CreateInvenParam(InvenParamSpecList, 0, enableAFI, AFI, 0);
            }

            DateTime startTime;
            DateTime endTime;
            TimeSpan t;

            //int iCount = 0;
            while (!_shouldStop)
            {
                startTime = DateTime.Now;
                // antenna selection
                /*if (iCount > antennaCount) { iCount = 0; }
                AntennaSel[0] = (Byte)(iCount + 1);
                AntennaSelCount = (Byte)1;
                iCount++;
                */
                /*iCount = 0; // 0：天线1，1：天线2
                AntennaSel[0] = (Byte)(iCount + 1);
                AntennaSelCount = (Byte)1;
                */

                
                UInt32 nTagCount = 0;
                int iret;
                LABEL_TAG_INVENTORY:
                iret = RFIDLIB.rfidlib_reader.RDR_TagInventory(hreader, AIType, AntennaSelCount, AntennaSel, InvenParamSpecList);
                if (iret == 0 || iret == -21)
                {
                    //object[] pList0 = { null, null };
                    object[] pList0 = { null, null, null };
                    Invoke(tagReportHandler123, pList0);

                    nTagCount = RFIDLIB.rfidlib_reader.RDR_GetTagDataReportCount(hreader);
                    //object[] pList1 = { "count", nTagCount.ToString()};
                    object[] pList1 = { "count", null, nTagCount.ToString() };
                    Invoke(tagReportHandler123, pList1);

                    UIntPtr TagDataReport;
                    TagDataReport = (UIntPtr)0;
                    TagDataReport = RFIDLIB.rfidlib_reader.RDR_GetTagDataReport(hreader, RFIDLIB.rfidlib_def.RFID_SEEK_FIRST); //first

                    List<string> lstCurrRecvRfids = new List<string>();

                    Thread.Sleep(intInterval); //间隔多长时间读一次标签。

                    while (TagDataReport.ToUInt64() > 0)
                    {
                        UInt32 aip_id = 0;
                        UInt32 tag_id = 0;
                        UInt32 ant_id = 0;
                        Byte dsfid = 0;
                        Byte[] uid = new Byte[16];
                        iret = RFIDLIB.rfidlib_aip_iso15693.ISO15693_ParseTagDataReport(TagDataReport, ref aip_id, ref tag_id, ref ant_id, ref dsfid, uid);
                        string uidStr = BitConverter.ToString(uid).Replace("-", string.Empty).Substring(0, 16);

                        lstCurrRecvRfids.Add(uidStr);

                        object[] pList = { uidStr, TagDataReport.ToUInt64().ToString(), null };
                        Invoke(tagReportHandler123, pList);

                        TagDataReport = RFIDLIB.rfidlib_reader.RDR_GetTagDataReport(hreader, RFIDLIB.rfidlib_def.RFID_SEEK_NEXT); //next
                    }

                    funPostDataAndDisplayHtml(lstCurrRecvRfids);

                }
                if (iret == -21) // stop trigger occur,need to inventory left tags
                {
                    AIType = RFIDLIB.rfidlib_def.AI_TYPE_CONTINUE;//use only-new-tag inventory 
                    goto LABEL_TAG_INVENTORY;
                }
                iret = 0;
                // 计算消耗时间 -------------------------------------------
                endTime = DateTime.Now;
                t = endTime - startTime;
                //object[] pList2 = { "耗时", t.TotalSeconds.ToString() + "   标签数量：" + nTagCount.ToString() };
                object[] pList2 = { "耗时", null, t.TotalSeconds.ToString() + "   标签数量：" + nTagCount.ToString() };
                Invoke(tagReportHandler123, pList2);
            }
            RFIDLIB.rfidlib_reader.RDR_ResetCommuImmeTimeout(hreader);
        }


        private void b_stopInventory_Click(object sender, EventArgs e)
        {
            _shouldStop = true;
        }

        public void tagReport(string uidStr, string blockDataStr)
        {
            if (uidStr == null)
            {
                textBox1.Text = "";
            }
            else if (blockDataStr == null)
            {
                //textBox1.Text += uidStr + "\r\n";
                textBox1.Text += uidStr + "\r\n";

            }
            else if (uidStr == "count")
            {
                if (int.Parse(blockDataStr) < (int)nud_TagCount.Value)
                {
                    nud_TagLoseCount.Value = (int)nud_TagLoseCount.Value + 1;
                }
            }
            else if (uidStr == "耗时")
            {
                label5.Text = uidStr + " - " + blockDataStr;
            }
            else
            {
                textBox1.Text += uidStr + " - " + blockDataStr + "\r\n";
            }
        }


        public void tagReport123(string uidStr, string uptr, string blockDataStr)
        {
            if (uidStr == null)
            {
                textBox1.Text = "";
            }
            else if (blockDataStr == null)
            {
                //textBox1.Text += uidStr + "\r\n";
                textBox1.Text += uidStr + "----------" + uptr + "\r\n";

            }
            else if (uidStr == "count")
            {
                if (int.Parse(blockDataStr) < (int)nud_TagCount.Value)
                {
                    nud_TagLoseCount.Value = (int)nud_TagLoseCount.Value + 1;
                }
            }
            else if (uidStr == "耗时")
            {
                label5.Text = uidStr + " - " + blockDataStr;
            }
            else
            {
                textBox1.Text += uidStr + " - " + blockDataStr + "\r\n";
            }
        }


        private void b_clear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            int len = hex.Length >> 1;
            byte[] arr = new byte[len];

            for (int i = 0; i < len; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            // return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _shouldStop = true;
            if (InvenThread != null)
                InvenThread.Abort();


        }

        public void funPostDataAndDisplayHtml(List<string> lstCurrRecvRfids)
        {

            Product curProd = new Product();
            
            int intCnt = 0;
            decimal decPriceTotal = 0;



            if (lstTotalRfids.Count() == 0)   //第一次初始化lstTotalRfids
                lstTotalRfids = lstCurrRecvRfids;

            if (Util.ListEqual(listLastCard, lstCurrRecvRfids))  // 前后两次一样，不刷新页面
                return;
            listLastCard = lstCurrRecvRfids;


            lstLeaveRfids = lstTotalRfids.Except(lstCurrRecvRfids).ToList();//差集,得到需要发往服务器（即需要显示的标签）

            lstTotalRfids = lstTotalRfids.Union(lstCurrRecvRfids).ToList<string>(); //收到的并入lstTotalRfids

            //上传在柜rfid到 盘点表
            string strCurrRecvRfids = JsonConvert.SerializeObject(lstCurrRecvRfids);
            if (HttpPost(apiAddress + "/api/ProductIO/SaveInventory", strCurrRecvRfids) == false)
            {
                // MessageBox.Show("数据上传失败。");
                return;
            }

            //上传离柜rfid到 离柜表
            string strLeaveRfids = JsonConvert.SerializeObject(lstLeaveRfids);
            if (HttpPost(apiAddress+ "/api/ProductIO/SaveData", strLeaveRfids) == false)
            {
               // MessageBox.Show("数据上传失败。");
                return;
            }

            bool isWelcome = webBrowser1.Url.ToString().Contains("welcome.html");
            if (lstLeaveRfids.Count == 0)
            {
                if (isWelcome)
                    return;
                else
                {
                    webBrowser1.Navigate(Application.StartupPath + @"\welcome.html");
                    return;
                }
            }

            lstLeaveRfids.Sort();
            int cols = 6;
            int rows = 0;
            rows = lstLeaveRfids.Count / cols;
            if (lstLeaveRfids.Count % cols > 0)
                rows = rows + 1;
            //font-family:'Microsoft YaHei';
            //
            string strHtml = "<html><head><meta charset='utf-8'/>  ";
            strHtml = strHtml + "<style type='text/css'> ";
            strHtml = strHtml + "  body{padding:0;margin:0; background:white;font-family:'Microsoft YaHei';}";
            strHtml = strHtml + "   div.bottom {width: 100%; position: absolute; bottom: 15; padding-top: 20px;  color:red; font-size: 35px;text-align: center;}";
            strHtml = strHtml + "</style>";
            strHtml = strHtml + " </head>  <body  > <TABLE WIDTH=100% HEIGHT=100% BORDER =1><TR><TD align=center>";

            //strHtml = strHtml + "<span style='width:100%;height:30px;color:#ff6700;font-size:30px;font-weight:bold'>我的购物车</span>";


            strHtml = strHtml + "<span id=time1 style='width:100%;height:30px;color:#ff6700;font-size:30px;font-weight:bold'>";
            strHtml = strHtml + "<SCRIPT>";
            strHtml = strHtml + " document.getElementById('time1').innerHTML='我的购物车，当前时间'+new Date().toLocaleTimeString();";
            strHtml = strHtml + " setInterval(\"document.getElementById('time1').innerHTML='我的购物车，当前时间'+new Date().toLocaleTimeString();\",1000);";
            strHtml = strHtml + "</SCRIPT>";
            strHtml = strHtml + "</span>";




            strHtml = strHtml + "<hr/>";
            strHtml = strHtml + "<table width=80%  align=center border=0 bgcolor=white>";

            int idx = 0;
            for (int row = 0; row < rows; row++)
            {
                strHtml = strHtml + " <tr>";
                for (int col = 0; col < cols; col++)
                {
                    if (idx > lstLeaveRfids.Count - 1)
                        break;
                    curProd = getProd(lstLeaveRfids[idx].ToString());
                    if (curProd == null)
                    {
                        //decPriceTotal = decPriceTotal + curProd.Price;
                        strHtml = strHtml + "   <td valign= middle align =center  >";
                        strHtml = strHtml + "     <img style='margin-top:60px' height='180' width='180' src='db\\noProduct.jpg'> ";
                        strHtml = strHtml + "     <p> ";
                        strHtml = strHtml + "    商品编号: " + lstLeaveRfids[idx].ToString();
                        strHtml = strHtml + "     <p> ";
                        strHtml = strHtml + "    商品名称: 不明";
                        strHtml = strHtml + "     <p> ";
                        strHtml = strHtml + "     <font color='red'>商品价格: 不明</font>";
                        strHtml = strHtml + "   </td>";
                    }
                    else {
                        decPriceTotal = decPriceTotal + curProd.Price;
                        strHtml = strHtml + "   <td valign= middle align =center  >";
                        strHtml = strHtml + "     <img style='margin-top:60px' height='180' width='180' src='db\\" + curProd.rfidNo + ".jpg'> ";
                        strHtml = strHtml + "     <p> ";
                        strHtml = strHtml + "    商品编号: " + curProd.rfidNo;
                        strHtml = strHtml + "     <p> ";
                        strHtml = strHtml + "    商品名称: " + curProd.prodName;
                        strHtml = strHtml + "     <p> ";
                        strHtml = strHtml + "     <font color='#ff6700'>商品价格: ￥" + curProd.Price + "</font>";
                        strHtml = strHtml + "   </td>";                    
                    }
                    intCnt = intCnt + 1;
                    idx = idx + 1;
                }
                strHtml = strHtml + " </tr>";
            }

            strHtml = strHtml + "</table>";
            strHtml = strHtml + "<hr/>";
            strHtml = strHtml + "<table width=100%  align=center border=0 ><tr><td width=60% align=right><span style='font-weight:bold;color:#ff6700;font-size:30px'>共计" + intCnt.ToString() + "件，结算金额￥" + decPriceTotal.ToString() + "元</span></td><td width=40% align=left><img src='db\\wxzf.jpg'><img src='db\\wxzfma.jpg'> </td></tr></table>";
            strHtml = strHtml + "</TD></TR></TABLE>";
            strHtml = strHtml + "</body></html>";
            genHtmlFile(Application.StartupPath + @"\Cart.html", strHtml);
            webBrowser1.Navigate(Application.StartupPath + @"\Cart.html");
        }



        #region 生成htmlfile
        void genHtmlFile(string filename, string strHtml)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(strHtml);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
        #endregion


        #region 根据标签ID获得产品信息
        Product getProd(string strCardID)
        {
            foreach (var item in listProd)
            {
                if (item.rfidNo == strCardID)
                {
                    return item;
                }
            }
            return null;
        }
        #endregion


        public static string Obj2Json(List<string> data)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(data.GetType());

                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, data);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch
            {
                return null;
            }
        }


        //public static string HttpPost(string url, string body)
        public static bool HttpPost(string url, string body)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;

            HttpWebResponse res;
            try
            {
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException )
            {
                return false;
                //res = (HttpWebResponse)ex.Response;
            }

            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            string strHtml = sr.ReadToEnd();
            //return strHtml;
            return true;






        }

        public void initCom() {

            int iret = 0;
            readerType = (Byte)Convert.ToInt16(ConfigurationManager.AppSettings["readerType"]);
            string readerDriverName = ConfigurationManager.AppSettings["readerDriverName"].ToString(); ;
            string connstr = ConfigurationManager.AppSettings["connstr"].ToString();
            intInterval = Convert.ToInt16(ConfigurationManager.AppSettings["interval"]);
            apiAddress = ConfigurationManager.AppSettings["apiAddress"].ToString();


            try
            {
                iret = RFIDLIB.rfidlib_reader.RDR_Open(connstr, ref hreader);
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
            

            if (iret != 0)
            {
                MessageBox.Show("启动失败1。");
                Application.Exit();
            }
            else
            {
                //antennaCount = RFIDLIB.rfidlib_reader.RDR_GetAntennaInterfaceCount(hreader);
                //antennaCount = (uint)(antennaCount > 1 ? 2 : 0);//如果天线多于1个，则开1、2口
                b_Inventory.Enabled = true;
                b_stopInventory.Enabled = true;
                b_open.Enabled = false;
                b_close.Enabled = true;

                try
                {
                    string myjson = HttpClientHelper.GetResponseJson(apiAddress + "/api/ProductIO/GetList");

                    listProd = (List<Product>)JsonTools.JsonToObject(myjson, listProd);

                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;

                    webBrowser1.Top = 0;
                    webBrowser1.Left = 0;
                    webBrowser1.Width = this.Width;
                    webBrowser1.Height = this.Height;
                    webBrowser1.ScrollBarsEnabled = false;
                    webBrowser1.IsWebBrowserContextMenuEnabled = false;
                    webBrowser1.Navigate(Application.StartupPath + @"\welcome.html");
                    webBrowser1.ScriptErrorsSuppressed = true;
                    this.textBox1.Visible = false;


                    //启动盘点
                    _shouldStop = false;
                    //AFI
                    enableAFI = 0;
                    //Only new tag
                    onlyNewTag = 0; /* 0 or 1*/
                                    //InvenThread = new Thread(DoInventory);
                    InvenThread = new Thread(DoInventory123);
                    InvenThread.Start();
                }
                catch (Exception  e )
                {
                    MessageBox.Show("启动失败2。" + e.Message);
                    Application.Exit();
                }

            }
        }


        //public async void HttpClientDoGet(string URI)
        //{

        //    using (WebClient client = new WebClient())
        //    {
        //        Uri myurl = new Uri(URI);
        //        client.Headers["Type"] = "POST";
        //        client.Headers["Accept"] = "application/json";
        //        client.Encoding = Encoding.UTF8;
        //        client.DownloadStringCompleted += (senderobj, es) =>
        //        {
        //            var obj = es.Result;
        //        };
        //        client.DownloadStringAsync(myurl);
        //    }
        //}

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }



    }


    public class CReaderDriverInf
    {
        public string m_catalog;
        public string m_name;
        public string m_productType;
        public UInt32 m_commTypeSupported;
    }

    public class JsonTools
    {

        // 从一个对象信息生成Json串  
        public static string ObjectToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        
        // 从一个Json串生成对象信息  
        public static object JsonToObject(string jsonString, object obj)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                return serializer.ReadObject(mStream);

            }
            catch (Exception)
            {

                return null;
            }
        }

    }


    public class HttpClientHelper
    {
        public static string GetResponseJson(string url)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseJson = response.Content.ReadAsStringAsync().Result;
                return responseJson;
            }
            else
            {
                return "出错了,StatusCode:" + response.StatusCode.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">调用的Api地址</param>
        /// <param name="requestJson">表单数据（json格式）</param>
        /// <returns></returns>
        public static string PostResponseJson(string url, string requestJson)
        {
            HttpContent httpContent = new StringContent(requestJson);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseJson = response.Content.ReadAsStringAsync().Result;
                return responseJson;
            }
            else
            {
                return "出错了,StatusCode:" + response.StatusCode.ToString();
            }
        }
    }

}
