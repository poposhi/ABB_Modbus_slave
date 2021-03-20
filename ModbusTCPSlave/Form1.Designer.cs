using System;
using System.Configuration; //app.config

namespace ModbusTCPSlave
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tm_D1 = new System.Windows.Forms.Timer(this.components);
            this.tm_D2 = new System.Windows.Forms.Timer(this.components);
            this.tm_D3 = new System.Windows.Forms.Timer(this.components);
            this.tm_D4 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.LB_status = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lv = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.timerPD = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btStop
            // 
            this.btStop.Location = new System.Drawing.Point(445, 19);
            this.btStop.Margin = new System.Windows.Forms.Padding(4);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(131, 35);
            this.btStop.TabIndex = 59;
            this.btStop.Text = "停止";
            this.btStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(303, 19);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(131, 35);
            this.btnStart.TabIndex = 58;
            this.btnStart.Text = "啟動Modbus通訊";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 24F);
            this.label5.Location = new System.Drawing.Point(11, 9);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(284, 56);
            this.label5.TabIndex = 60;
            this.label5.Text = "PD監控";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tm_D1
            // 
            this.tm_D1.Tick += new System.EventHandler(this.tm_D1_Tick);
            // 
            // tm_D2
            // 
            this.tm_D2.Tick += new System.EventHandler(this.tm_D2_Tick);
            // 
            // tm_D3
            // 
            this.tm_D3.Tick += new System.EventHandler(this.tm_D3_Tick);
            // 
            // tm_D4
            // 
            this.tm_D4.Tick += new System.EventHandler(this.tm_D4_Tick);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("新細明體", 14F);
            this.label1.Location = new System.Drawing.Point(183, 86);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 32);
            this.label1.TabIndex = 68;
            this.label1.Text = "狀態:";
            // 
            // LB_status
            // 
            this.LB_status.Font = new System.Drawing.Font("新細明體", 14F);
            this.LB_status.Location = new System.Drawing.Point(266, 86);
            this.LB_status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LB_status.Name = "LB_status";
            this.LB_status.Size = new System.Drawing.Size(418, 32);
            this.LB_status.TabIndex = 69;
            this.LB_status.Text = "停止";
            this.LB_status.Click += new System.EventHandler(this.LB_status_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("新細明體", 14F);
            this.label2.Location = new System.Drawing.Point(183, 331);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(358, 31);
            this.label2.TabIndex = 74;
            this.label2.Text = "Bug info";
            // 
            // lv
            // 
            this.lv.HideSelection = false;
            this.lv.Location = new System.Drawing.Point(3, 378);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(292, 134);
            this.lv.TabIndex = 75;
            this.lv.UseCompatibleStateImageBehavior = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(37, 168);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 35);
            this.button1.TabIndex = 76;
            this.button1.Text = "數值變動測試 ";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("新細明體", 14F);
            this.label3.Location = new System.Drawing.Point(192, 129);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(418, 32);
            this.label3.TabIndex = 77;
            this.label3.Text = "停止";
            // 
            // timerPD
            // 
            this.timerPD.Interval = 1000;
            this.timerPD.Tick += new System.EventHandler(this.timerPD_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(37, 233);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(131, 35);
            this.button2.TabIndex = 78;
            this.button2.Text = "十萬";
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 524);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lv);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LB_status);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label5);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "AFC模擬電表";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer tm_D1;
        private System.Windows.Forms.Timer tm_D2;
        private System.Windows.Forms.Timer tm_D3;
        private System.Windows.Forms.Timer tm_D4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LB_status;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timerPD;
        private System.Windows.Forms.Button button2;
    }
}

