using System;

using System.Windows.Forms;

using Modbus.Device;
using Modbus.Data;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;//debug msg在『輸出』視窗觀看
using System.Configuration; //app.config

namespace ModbusTCPSlave
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lv.View = View.Details;
            lv.GridLines = true;
            lv.LabelEdit = false;
            lv.FullRowSelect = true;
            lv.Columns.Add("message1", 50);
            lv.Columns.Add("message2", 270);
        }

        TcpListener slaveTcpListener;
        Modbus.Device.ModbusSlave slave;
        //IPAddress address = new IPAddress(new byte[] { 140, 118, 172, 152 });
        private byte slaveID = Convert.ToByte(ConfigurationManager.AppSettings["slaveID"]);
        private int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
        private ushort f_addressH = Convert.ToUInt16(ConfigurationManager.AppSettings["f_addressH"]);
        private void Form1_Load(object sender, EventArgs e)
        {
            //Get host IP

            //IPAddress addr;    
            //addr = new IPAddress(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].Address);
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;

            tm_D1.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["timer1_2_interval"]);
            tm_D2.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["timer1_2_interval"]);
            tm_D3.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["timer3_4_interval"]);
            tm_D4.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["timer3_4_interval"]);
            try
            {

                //if (addr[1] != null)
                //{
                //    lv_Print(lv, "addr[1]", addr[1].ToString());
                //}
                //if (addr[2] != null)
                //{
                //    lv_Print(lv, "addr[2]", addr[2].ToString());
                //}
                //if (addr[3] != null)
                //{
                //    lv_Print(lv, "addr[3]", addr[3].ToString());
                //}
                for (int i = 0; i < 50; i++)
                {
                    if (addr[i] != null)
                    {
                        lv_Print(lv, "addr[" + i.ToString() + "]", addr[i].ToString());
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ee)
            {

                lv_Print(lv, "error", ee.Message);

            }
            //txtServerName.Text = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            label3.Text = "IP_number :" + ConfigurationManager.AppSettings["IP_number"];
        }
        public delegate void Listview_Print(ListView list, string time, string type);//time type 沒改
        public static void lv_Print(ListView list, string message1, string message2)// 輸入listview ,兩個str
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (list.InvokeRequired)
            {
                Listview_Print ph = new Listview_Print(lv_Print);
                list.Invoke(ph, list, message1, message2);
            }
            else
            {
                String[] row = { message1, message2 };
                ListViewItem item = new ListViewItem(row);
                //ADD ITEMS
                list.Items.Add(item);
                if (list.Items.Count > 1000)
                {
                    list.Items.RemoveAt(1);
                }
            }
        }

        private void Modbus_Request_Event(object sender, Modbus.Device.ModbusSlaveRequestEventArgs e)
        {
            //request from master//disassemble packet from master
            byte fc = e.Message.FunctionCode;
            byte[] data = e.Message.MessageFrame;
            byte[] byteStartAddress = new byte[] { data[3], data[2] };
            byte[] byteNum = new byte[] { data[5], data[4] };
            Int16 StartAddress = BitConverter.ToInt16(byteStartAddress, 0);
            Int16 NumOfPoint = BitConverter.ToInt16(byteNum, 0);

            //Console.WriteLine(fc.ToString() + "," + StartAddress.ToString() + "," + NumOfPoint.ToString());
        }

        private void Modbus_DataStoreWriteTo(object sender, Modbus.Data.DataStoreEventArgs e)
        {
            //this.Text = "DataType=" + e.ModbusDataType.ToString() + "  StartAdress=" + e.StartAddress;
            int iAddress = e.StartAddress;//e.StartAddress;

            switch (e.ModbusDataType)
            {
                case ModbusDataType.HoldingRegister:

                    for (int i = 0; i < e.Data.B.Count; i++)
                    {
                        //Set AO                 

                        //e.Data.B[i] already write to slave.DataStore.HoldingRegisters[e.StartAddress + i + 1]
                        //e.StartAddress starts from 0
                        //You can set AO value to hardware here

                        DoAOUpdate(iAddress, e.Data.B[i].ToString());
                        iAddress++;
                    }
                    break;

                case ModbusDataType.Coil:
                    for (int i = 0; i < e.Data.A.Count; i++)
                    {
                        //Set DO
                        //e.Data.A[i] already write to slave.DataStore.CoilDiscretes[e.StartAddress + i + 1]
                        //e.StartAddress starts from 0
                        //You can set DO value to hardware here

                        DoDOUpdate(iAddress, e.Data.A[i]);
                        iAddress++;
                        if (e.Data.A.Count == 1)
                        {
                            break;
                        }
                    }
                    break;
            }
        }
        #region "Set AO"
        private delegate void UpdateAOStatusDelegate(int index, String message);
        private void DoAOUpdate(int index, String message)
        {
            if (this.InvokeRequired)
            {
                // we were called on a worker thread
                // marshal the call to the user interface thread
                this.Invoke(new UpdateAOStatusDelegate(DoAOUpdate),
                            new object[] { index, message });
                return;
            }

            // this code can only be reached
            // by the user interface thread
            //switch (index)
            //{
            //    case 0:
            //        this.txtAO1.Text = message;
            //        break;
            //    case 1:
            //        this.txtAO2.Text = message;
            //        break;
            //    case 2:
            //        this.txtAO3.Text = message;
            //        break;
            //    case 3:
            //        this.txtAO4.Text = message;
            //        break;
            //}

        }
        #endregion

        #region "Set DO"
        private delegate void UpdateDOStatusDelegate(int index, bool value);
        private void DoDOUpdate(int index, bool value)
        {
            if (this.InvokeRequired)
            {
                // we were called on a worker thread
                // marshal the call to the user interface thread
                this.Invoke(new UpdateDOStatusDelegate(DoDOUpdate),
                            new object[] { index, value });
                return;
            }

            // this code can only be reached
            // by the user interface thread
            //switch (index)
            //{
            //    case 0:
            //        this.chkDO1.Checked = value;
            //        break;
            //    case 1:
            //        this.chkDO2.Checked = value;
            //        break;
            //    case 2:
            //        this.chkDO3.Checked = value;
            //        break;
            //    case 3:
            //        this.chkDO4.Checked = value;
            //        break;
            //}

        }
        #endregion

        private void btnStart_Click(object sender, EventArgs e)
        {
            // create and start the TCP slave
            try
            {
                byte ip1 = Convert.ToByte(ConfigurationManager.AppSettings["ip1"]);
                byte ip2 = Convert.ToByte(ConfigurationManager.AppSettings["ip2"]);
                byte ip3 = Convert.ToByte(ConfigurationManager.AppSettings["ip3"]);
                byte ip4 = Convert.ToByte(ConfigurationManager.AppSettings["ip4"]);
                byte[] xip = new byte[] { ip1, ip2, ip3, ip4 };
                IPAddress aaa = new IPAddress(xip);
                //IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress[] addr = ipEntry.AddressList;
                //slaveTcpListener = new TcpListener(aaa, port);
                //slaveTcpListener.Start();

                IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] addr = ipEntry.AddressList;
                slaveTcpListener = new TcpListener(addr[1], port);
                slaveTcpListener.Start();
                slave = Modbus.Device.ModbusTcpSlave.CreateTcp(slaveID, slaveTcpListener);
                slave.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(Modbus_Request_Event);
                slave.DataStore = Modbus.Data.DataStoreFactory.CreateDefaultDataStore();
                slave.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>(Modbus_DataStoreWriteTo);
                slave.Listen();

            }
            catch (Exception ee)
            {
                LB_status.Text = ee.Message;
                label2.Text = ee.Message;
            }

            try
            {
                slave.DataStore.HoldingRegisters[2001] = 0;
                slave.DataStore.HoldingRegisters[2002] = 0;

                slave.DataStore.HoldingRegisters[2003] = 0;
                slave.DataStore.HoldingRegisters[2004] = 0;
                slave.DataStore.HoldingRegisters[2005] = 0;
                slave.DataStore.HoldingRegisters[2006] = 0;
            }
            catch (Exception ee)
            {
                Console.WriteLine("slave.DataStor" + ee.Message);
                throw;
            }
            timerPD.Enabled = true;
            timer1.Enabled = true;
            btnStart.Enabled = false;
            btStop.Enabled = true;
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            btnStart.Enabled = true;
            btStop.Enabled = false;
            slaveTcpListener.Stop();
            slaveTcpListener = null;
            //slave.Stop();            
            slave.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tm_D1.Enabled == true)
            {
                LB_status.Text = "D1測試中 第" + D1count.ToString() + "秒   頻率:" + f.ToString() + "Hz";
            }
            else if (tm_D2.Enabled == true)
            {
                LB_status.Text = "D2測試中 第" + D2count.ToString() + "秒   頻率:" + f.ToString() + "Hz";
            }
            else if (tm_D3.Enabled == true)
            {
                LB_status.Text = "D3測試中 第" + D3count.ToString() + "秒   頻率:" + f.ToString() + "Hz";
            }
            else if (tm_D4.Enabled == true)
            {
                LB_status.Text = "D4測試中 第" + D4count.ToString() + "秒   頻率:" + f.ToString() + "Hz";
            }
            else
            {
                LB_status.Text = "停止";
            }
            //update input values to datastore
            ////DI
            //slave.DataStore.InputDiscretes[1] = chkDI1.Checked;
            //slave.DataStore.InputDiscretes[2] = chkDI2.Checked;
            //slave.DataStore.InputDiscretes[3] = chkDI3.Checked;
            //slave.DataStore.InputDiscretes[4] = chkDI4.Checked;
            ////AI
            //slave.DataStore.InputRegisters[1] = Convert.ToUInt16(txtAI1.Text);
            //slave.DataStore.InputRegisters[2] = Convert.ToUInt16(txtAI2.Text);
            //slave.DataStore.InputRegisters[3] = Convert.ToUInt16(txtAI3.Text);
            //slave.DataStore.InputRegisters[4] = Convert.ToUInt16(txtAI4.Text);
            ////AO
            //slave.DataStore.HoldingRegisters[1] = Convert.ToUInt16(txtAO1.Text);
            //slave.DataStore.HoldingRegisters[2] = Convert.ToUInt16(txtAO2.Text);
            //slave.DataStore.HoldingRegisters[3] = Convert.ToUInt16(txtAO3.Text);
            //slave.DataStore.HoldingRegisters[4] = Convert.ToUInt16(txtAO4.Text);
            ////DO
            //slave.DataStore.CoilDiscretes[1] = chkDO1.Checked;
            //slave.DataStore.CoilDiscretes[2] = chkDO2.Checked;
            //slave.DataStore.CoilDiscretes[3] = chkDO3.Checked;
            //slave.DataStore.CoilDiscretes[4] = chkDO4.Checked;
        }
        int D1count = 0;
        float f = 0;
        /// <summary>
        /// d1 用以測試儲能系統可於1秒內反應頻率變化(符合A-1要求)，且輸出功率控制於相對應頻率之操作曲線範圍內(符合B-1~B-6要求)。
        /// 測試程序共計15項，每個測試時序均執行30秒，合計900秒。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tm_D1_Tick(object sender, EventArgs e)
        {
            //總共有30個階段 奇數的階段頻率等於60  偶數的階段頻率按照指定的輸出
            int Scale = 2;
            D1count++;
            ushort[] f_ushort = new ushort[2];
            if (D1count > 0 && D1count < 1 * Scale)
                f = (60.00F);
            if (D1count >= 1 * Scale && D1count < 2 * Scale) //31到60秒 
                f = (60.01F);
            if (D1count >= 2 * Scale && D1count < 3 * Scale)
                f = (60.00F);
            if (D1count >= 3 * Scale && D1count < 4 * Scale)
                f = (59.99F);
            if (D1count >= 4 * Scale && D1count < 5 * Scale)
                f = (60.00F);
            if (D1count >= 5 * Scale && D1count < 6 * Scale)
                f = (60.02F);
            if (D1count >= 6 * Scale && D1count < 7 * Scale)
                f = (60.00F);
            if (D1count >= 7 * Scale && D1count < 8 * Scale)
                f = (59.98F);
            if (D1count >= 8 * Scale && D1count < 9 * Scale)
                f = (60.00F);
            if (D1count >= 9 * Scale && D1count < 10 * Scale)
                f = (60.10F);
            if (D1count >= 10 * Scale && D1count < 11 * Scale)
                f = (60.00F);
            if (D1count >= 11 * Scale && D1count < 12 * Scale)
                f = (59.90F);
            if (D1count >= 12 * Scale && D1count < 13 * Scale)
                f = (60.00F);
            if (D1count >= 13 * Scale && D1count < 14 * Scale)
                f = (60.00F);
            if (D1count >= 14 * Scale && D1count < 15 * Scale)
                f = (60.00F);
            if (D1count >= 15 * Scale && D1count < 16 * Scale)
                f = (60.00F);
            if (D1count >= 16 * Scale && D1count < 17 * Scale)
                f = (60.00F);
            if (D1count >= 17 * Scale && D1count < 18 * Scale)
                f = (60.00F);
            if (D1count >= 18 * Scale && D1count < 19 * Scale)
                f = (60.00F);
            if (D1count >= 19 * Scale && D1count < 20 * Scale)
                f = (60.00F);
            if (D1count >= 20 * Scale && D1count < 21 * Scale)
                f = (60.00F);
            if (D1count >= 21 * Scale && D1count < 22 * Scale)
                f = (60.00F);
            if (D1count >= 22 * Scale && D1count < 23 * Scale)
                f = (60.00F);
            if (D1count >= 23 * Scale && D1count < 24 * Scale)
                f = (60.00F);
            if (D1count >= 24 * Scale && D1count < 25 * Scale)
                f = (60.00F);
            if (D1count >= 25 * Scale && D1count < 26 * Scale)
                f = (60.00F);
            if (D1count >= 26 * Scale && D1count < 27 * Scale)
                f = (60.00F);
            if (D1count >= 27 * Scale && D1count < 28 * Scale)
                f = (60.00F);
            if (D1count >= 28 * Scale && D1count < 29 * Scale)
                f = (60.00F);
            if (D1count >= 29 * Scale && D1count < 30 * Scale)
                f = (60.00F);
            if (D1count >= 30 * Scale)
            {
                tm_D1.Enabled = false;
                Debug.Print("tm_D1.Enabled = false;");
            }
            Debug.Print("f" + f.ToString());
            f_ushort = FloatToUshort(f);
            slave.DataStore.HoldingRegisters[f_addressH] = f_ushort[0];
            slave.DataStore.HoldingRegisters[f_addressH + 1] = f_ushort[1];

        }
        ushort[] FloatToUshort(float f)
        {
            //浮點數轉換成兩個ushort  
            // ushort 16位（2位元組）整數
            ushort[] answer = new ushort[2];
            var bytes = BitConverter.GetBytes(f);
            var i = BitConverter.ToInt32(bytes, 0);
            answer[0] = (ushort)(i >> 16);
            answer[1] = (ushort)i;
            return answer;
        }

        float FromHexString(string s)
        {
            var i = Convert.ToInt32(s, 16);
            var bytes = BitConverter.GetBytes(i);
            return BitConverter.ToSingle(bytes, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime nnn = DateTime.Now;
            string mou = nnn.ToString("MM");

            Debug.Print(mou);
            //var f = FromHexString(hexString);
            var fff = FloatToUshort(60.02F);

            Debug.Print(fff[0].ToString());
            Debug.Print(fff[1].ToString());
            Debug.Print(IEEE754_to_float(fff[0], fff[1]).ToString());
        }
        private double IEEE754_to_float(ushort num_high, ushort num_low)
        {
            //IEEE754 to floating point C#
            //兩個Modbus高低位置(a為高位元，b為低位元)，合併為一32位元正整數c，計算後取得電網頻率d
            double result = 0;
            ushort a; //17008
            ushort b; //8113
            uint c;   //1114644401
                      //double d; //60.030948638916
            a = num_high;
            b = num_low;
            c = Convert.ToUInt32(((uint)a * 65536) + b);
            result = BitConverter.ToSingle(BitConverter.GetBytes((int)c), 0);

            return Math.Round(result, 4);
        }
        private DateTime D1start_time;
        private void BT_D1_Click(object sender, EventArgs e)
        {
            D1count = 0;
            tm_D1.Enabled = true;
            D1start_time = DateTime.Now;
            //var filter2 = Builders<BsonDocument>.Filter.Eq("ID", Slave) & Builders<BsonDocument>.Filter.Eq("event", error_msg) & Builders<BsonDocument>.Filter.Eq("time", last_event);//////搜尋故障事件和發生時間以填入復歸時間
        }

        private void BT_stop_Click(object sender, EventArgs e)
        {
            tm_D1.Enabled = false;
            tm_D2.Enabled = false;
            tm_D3.Enabled = false;
            tm_D4.Enabled = false;
        }
        //用以測試儲能系統於頻率連續變化下，輸出/輸入功率控制於操作曲線範圍內(符合B-1~B-6要求)。
        //重點確認頻率為59.50Hz/60.50Hz時，電池輸出/輸入是否為100%；以及，59.75Hz/60.25Hz時，電池輸出/輸入是否為48%
        //頻率掃描測試之頻率時序圖，頻率由60.50Hz至59.50Hz，以及由59.50Hz至60.50Hz，各執行30秒。
        //各秒鐘之SBSPM應高於95%
        private DateTime D2start_time;
        private void BT_D2_Click(object sender, EventArgs e)
        {
            D2start_time = DateTime.Now;
            D2count = 0;
            tm_D2.Enabled = true;
        }
        double D2count = -1;
        private void tm_D2_Tick(object sender, EventArgs e)
        {
            //1到30秒 頻率60.5 等比例降到59.5  31秒到60秒 頻率59.5等比例升到60.5

            f = 60.50F;
            ushort[] f_ushort = new ushort[2];
            D2count++;
            if (D2count <= 30)
            {
                f = (float)(60.5F - (0.03333333 * D2count));
            }
            if (D2count > 30 && D2count <= 60)
            {
                f = (float)(59.5f + 0.03333333 * (D2count - 30));
            }
            if (D2count > 60)
            {
                tm_D2.Enabled = false;
                Debug.Print("tm_D2 結束 ;");
            }
            f_ushort = FloatToUshort(f);
            slave.DataStore.HoldingRegisters[f_addressH] = f_ushort[0];
            slave.DataStore.HoldingRegisters[f_addressH + 1] = f_ushort[1];
            Debug.Print("第" + D2count.ToString() + "秒 " + f.ToString() + "hz");
        }
        private DateTime D3start_time;
        private void BT_D3_Click(object sender, EventArgs e)
        {
            D3start_time = DateTime.Now;
            D3count = 0;
            tm_D3.Enabled = true;
        }
        private DateTime D4start_time;
        private void BT_D4_Click(object sender, EventArgs e)
        {
            D4start_time = DateTime.Now;
            D4count = 0;
            tm_D4.Enabled = true;
        }
        double D3count = 0;
        private void tm_D3_Tick(object sender, EventArgs e)
        {
            D3count++;
            f = 60.00F;
            ushort[] f_ushort = new ushort[2];

            if (D3count > 9 && D3count < 911)
            {
                f = 59.5F;
            }
            if (D3count > 911)
            {
                f = 60.00F;
                tm_D3.Enabled = false;
                Debug.Print("tm_D3 結束 ;");
            }
            f_ushort = FloatToUshort(f);
            slave.DataStore.HoldingRegisters[f_addressH] = f_ushort[0];
            slave.DataStore.HoldingRegisters[f_addressH + 1] = f_ushort[1];
            Debug.Print("第" + D3count.ToString() + "秒 " + f.ToString() + "hz");
        }
        double D4count = 0;
        //測試當儲能系統於系統電量(SOC)為75%狀況下，具備於頻率60.50Hz時，以100%功率輸入(充電)持續達5分鐘之能力(符合A-3)。
        //持續時間測試時，儲能系統之測試起始電量應SOC=75%。
        //持續時間測試之頻率時序圖，自頻率達60.50Hz起，持續300秒，各秒鐘之SBSPM均應為100%
        private void tm_D4_Tick(object sender, EventArgs e)
        {
            D4count++;
            f = 60.00F;
            ushort[] f_ushort = new ushort[2];
            if (D4count > 9 && D4count < 311)
            {
                f = 60.5F;
            }
            if (D4count > 311)
            {
                f = 60.00F;
                tm_D4.Enabled = false;
                Debug.Print("tm_D4 結束 ;");
            }


            f_ushort = FloatToUshort(f);
            slave.DataStore.HoldingRegisters[f_addressH] = f_ushort[0];
            slave.DataStore.HoldingRegisters[f_addressH + 1] = f_ushort[1];
            Debug.Print("第" + D4count.ToString() + "秒 " + f.ToString() + "hz");
        }

        private void LB_status_Click(object sender, EventArgs e)
        {

        }

        private void BT_D1_result_Click(object sender, EventArgs e)
        {
            int Year = D1start_time.Year;
            string Month = D1start_time.ToString("MM");
            string Day = D1start_time.ToString("dd");
            string Hour = D1start_time.ToString("hh");
            string Minute = D1start_time.ToString("mm");
            string Second = D1start_time.ToString("ss");
            string web_front = ConfigurationManager.AppSettings["web_address"];
            string web_address = web_front + Year + "-" + Month + "-" + Day + "-" + Hour + ":" + Minute + ":" + Second + "-910";

            Debug.Print(web_address);
            System.Diagnostics.Process.Start(web_address);
        }

        private void BT_D2_result_Click(object sender, EventArgs e)
        {
            int Year = D2start_time.Year;
            string Month = D2start_time.ToString("MM");
            string Day = D2start_time.ToString("dd");
            string Hour = D2start_time.ToString("hh");
            string Minute = D2start_time.ToString("mm");
            string Second = D2start_time.ToString("ss");
            string web_front = ConfigurationManager.AppSettings["web_address"];
            string web_address = web_front + Year + "-" + Month + "-" + Day + "-" + Hour + ":" + Minute + ":" + Second + "-70";

            Debug.Print(web_address);
            System.Diagnostics.Process.Start(web_address);
        }

        private void BT_D3_result_Click(object sender, EventArgs e)
        {
            int Year = D3start_time.Year;
            string Month = D3start_time.ToString("MM");
            string Day = D3start_time.ToString("dd");
            string Hour = D3start_time.ToString("hh");
            string Minute = D3start_time.ToString("mm");
            string Second = D3start_time.ToString("ss");
            string web_front = ConfigurationManager.AppSettings["web_address"];
            string web_address = web_front + Year + "-" + Month + "-" + Day + "-" + Hour + ":" + Minute + ":" + Second + "-910";

            Debug.Print(web_address);
            System.Diagnostics.Process.Start(web_address);
        }

        private void BT_D4_result_Click(object sender, EventArgs e)
        {
            int Year = D4start_time.Year;
            string Month = D4start_time.ToString("MM");
            string Day = D4start_time.ToString("dd");
            string Hour = D4start_time.ToString("hh");
            string Minute = D4start_time.ToString("mm");
            string Second = D4start_time.ToString("ss");
            string web_front = ConfigurationManager.AppSettings["web_address"];
            string web_address = web_front + Year + "-" + Month + "-" + Day + "-" + Hour + ":" + Minute + ":" + Second + "-310";

            Debug.Print(web_address);
            System.Diagnostics.Process.Start(web_address);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                slave.DataStore.HoldingRegisters[2001]++;

                slave.DataStore.HoldingRegisters[2003]++;
                slave.DataStore.HoldingRegisters[2004]++;

                if (slave.DataStore.HoldingRegisters[2005] == 0)
                {
                    slave.DataStore.HoldingRegisters[2005] = 1;
                }
                else if (slave.DataStore.HoldingRegisters[2005] == 1)
                {
                    slave.DataStore.HoldingRegisters[2005] = 0;
                }

                if (slave.DataStore.HoldingRegisters[2006] == 0)
                {
                    slave.DataStore.HoldingRegisters[2006] = 1;
                }
                else if (slave.DataStore.HoldingRegisters[2006] == 1)
                {
                    slave.DataStore.HoldingRegisters[2006] = 0;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine("" + ee.Message);
            }
        }
        void change(ref ushort value)
        {
            if (value == 0)
            {
                value = 1;
            }
            else if (value == 1)
            {
                value = 0;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void timerPD_Tick(object sender, EventArgs e)
        {

            for (int i = 1; i < 7; i++)
            {
                if (i == 1)
                {
                    Console.WriteLine("放電次數 : " + i.ToString() + "  " + (slave.DataStore.HoldingRegisters[2000 + i + 1] * 65535 + slave.DataStore.HoldingRegisters[2000 + i]));
                }
                if (i == 3)
                {
                    Console.WriteLine("放電平均值  : " + i.ToString() + "  " + slave.DataStore.HoldingRegisters[2000 + i]);
                }
                if (i == 4)
                {
                    Console.WriteLine("最大值 : " + i.ToString() + "  " + slave.DataStore.HoldingRegisters[2000 + i]);
                }
                if (i == 5)
                {
                    Console.WriteLine("警報 : " + i.ToString() + "  " + slave.DataStore.HoldingRegisters[2000 + i]);
                }

                if (i == 6)
                {
                    Console.WriteLine("通訊狀態  : " + i.ToString() + "  " + slave.DataStore.HoldingRegisters[2000 + i]);
                }
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            double value = 10000;
            ushort H_value = Convert.ToUInt16(value / 65535);
            ushort L_value = Convert.ToUInt16(value - H_value * 65535);

            slave.DataStore.HoldingRegisters[2001] = L_value;
            slave.DataStore.HoldingRegisters[2002] = H_value;
            //----------
            int CommFailureCount = 0;                 //記錄通訊故障的次數 ，  讀取完記得要把通訊故障計數器歸零 
            bool CommFailureReal = false;           //連續發生多次通訊故障的時候  就變TRUE
            bool CommFailureReal_updated = false;  //記錄是否有上傳通訊故障   有上傳通訊故障  就變TRUE
            DateTime CommFailureTime_happen;        //上傳通訊故障的時間 
            DateTime CommFailureTime_recover;       //上傳恢復通訊故障的時間

            try
            {
                //讀取資料 
                CommFailureCount = 0;
                if (CommFailureReal == true)
                {
                    CommFailureReal = false;
                    CommFailureTime_recover = DateTime.Now;
                }
            }
            catch (Exception except)
            {
                Console.WriteLine("設備名稱 時間 " + except.Message);
                CommFailureCount++;
                if (CommFailureCount > 5)
                {
                    CommFailureReal = true;
                    CommFailureTime_happen = DateTime.Now;
                }

            }
        }
        int CommFailureCount = 0;                 //記錄通訊故障的次數 ，  讀取完記得要把通訊故障計數器歸零 
        bool CommFailureReal = false;           //連續發生多次通訊故障的時候  就變TRUE
        bool CommFailureReal_updated = false;  //記錄是否有上傳通訊故障   有上傳通訊故障  就變TRUE
        DateTime CommFailureTime_happen;        //上傳通訊故障的時間 
        DateTime CommFailureTime_recover;       //上傳恢復通訊故障的時間

        //假如發生通訊故障就上傳設備ID通訊故障時間，假如恢復舊更新資料並且加入恢復時間
        void UpdateCommError()
        {
            if (CommFailureReal == true && CommFailureReal_updated == false)
            {//假如偵測到發生通訊故障 ，並且還沒上傳
                //上傳設備ID 通訊故障時間
                CommFailureReal_updated = true;
            }
            if (CommFailureReal == false && CommFailureReal_updated == true)
            {//假如通訊故障復原 
                //更新資料庫資料 增加通訊故障復原時間 
                CommFailureReal_updated = false;
            }
        }
    }
}