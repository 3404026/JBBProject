
namespace JBBCounter
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cb_Drivers = new System.Windows.Forms.ComboBox();
            this.cb_ports = new System.Windows.Forms.ComboBox();
            this.b_open = new System.Windows.Forms.Button();
            this.b_close = new System.Windows.Forms.Button();
            this.b_Inventory = new System.Windows.Forms.Button();
            this.b_stopInventory = new System.Windows.Forms.Button();
            this.b_clear = new System.Windows.Forms.Button();
            this.nud_blockToRead = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_blockToRead = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_blockToWrite = new System.Windows.Forms.CheckBox();
            this.tb_blockToWrite = new System.Windows.Forms.TextBox();
            this.nud_blockToWrite = new System.Windows.Forms.NumericUpDown();
            this.nud_TagCount = new System.Windows.Forms.NumericUpDown();
            this.nud_TagLoseCount = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nud_blockToRead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_blockToWrite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TagCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TagLoseCount)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_Drivers
            // 
            this.cb_Drivers.FormattingEnabled = true;
            this.cb_Drivers.Location = new System.Drawing.Point(13, 12);
            this.cb_Drivers.Name = "cb_Drivers";
            this.cb_Drivers.Size = new System.Drawing.Size(121, 20);
            this.cb_Drivers.TabIndex = 0;
            // 
            // cb_ports
            // 
            this.cb_ports.FormattingEnabled = true;
            this.cb_ports.Location = new System.Drawing.Point(140, 12);
            this.cb_ports.Name = "cb_ports";
            this.cb_ports.Size = new System.Drawing.Size(121, 20);
            this.cb_ports.TabIndex = 1;
            // 
            // b_open
            // 
            this.b_open.Location = new System.Drawing.Point(267, 10);
            this.b_open.Name = "b_open";
            this.b_open.Size = new System.Drawing.Size(75, 23);
            this.b_open.TabIndex = 2;
            this.b_open.Text = "open";
            this.b_open.UseVisualStyleBackColor = true;
            this.b_open.Click += new System.EventHandler(this.b_open_Click);
            // 
            // b_close
            // 
            this.b_close.Enabled = false;
            this.b_close.Location = new System.Drawing.Point(348, 10);
            this.b_close.Name = "b_close";
            this.b_close.Size = new System.Drawing.Size(75, 23);
            this.b_close.TabIndex = 3;
            this.b_close.Text = "close";
            this.b_close.UseVisualStyleBackColor = true;
            this.b_close.Click += new System.EventHandler(this.b_close_Click);
            // 
            // b_Inventory
            // 
            this.b_Inventory.Enabled = false;
            this.b_Inventory.Location = new System.Drawing.Point(13, 48);
            this.b_Inventory.Name = "b_Inventory";
            this.b_Inventory.Size = new System.Drawing.Size(75, 23);
            this.b_Inventory.TabIndex = 4;
            this.b_Inventory.Text = "Inventory";
            this.b_Inventory.UseVisualStyleBackColor = true;
            this.b_Inventory.Click += new System.EventHandler(this.b_Inventory_Click);
            // 
            // b_stopInventory
            // 
            this.b_stopInventory.Enabled = false;
            this.b_stopInventory.Location = new System.Drawing.Point(94, 48);
            this.b_stopInventory.Name = "b_stopInventory";
            this.b_stopInventory.Size = new System.Drawing.Size(75, 23);
            this.b_stopInventory.TabIndex = 5;
            this.b_stopInventory.Text = "stop";
            this.b_stopInventory.UseVisualStyleBackColor = true;
            this.b_stopInventory.Click += new System.EventHandler(this.b_stopInventory_Click);
            // 
            // b_clear
            // 
            this.b_clear.Location = new System.Drawing.Point(175, 48);
            this.b_clear.Name = "b_clear";
            this.b_clear.Size = new System.Drawing.Size(75, 23);
            this.b_clear.TabIndex = 7;
            this.b_clear.Text = "clear";
            this.b_clear.UseVisualStyleBackColor = true;
            this.b_clear.Click += new System.EventHandler(this.b_clear_Click);
            // 
            // nud_blockToRead
            // 
            this.nud_blockToRead.Location = new System.Drawing.Point(397, 48);
            this.nud_blockToRead.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nud_blockToRead.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_blockToRead.Name = "nud_blockToRead";
            this.nud_blockToRead.Size = new System.Drawing.Size(42, 21);
            this.nud_blockToRead.TabIndex = 9;
            this.nud_blockToRead.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(323, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "blockToRead";
            // 
            // cb_blockToRead
            // 
            this.cb_blockToRead.AutoSize = true;
            this.cb_blockToRead.Location = new System.Drawing.Point(445, 52);
            this.cb_blockToRead.Name = "cb_blockToRead";
            this.cb_blockToRead.Size = new System.Drawing.Size(15, 14);
            this.cb_blockToRead.TabIndex = 11;
            this.cb_blockToRead.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "blockToWrite";
            // 
            // cb_blockToWrite
            // 
            this.cb_blockToWrite.AutoSize = true;
            this.cb_blockToWrite.Location = new System.Drawing.Point(758, 90);
            this.cb_blockToWrite.Name = "cb_blockToWrite";
            this.cb_blockToWrite.Size = new System.Drawing.Size(15, 14);
            this.cb_blockToWrite.TabIndex = 13;
            this.cb_blockToWrite.UseVisualStyleBackColor = true;
            // 
            // tb_blockToWrite
            // 
            this.tb_blockToWrite.Location = new System.Drawing.Point(140, 86);
            this.tb_blockToWrite.Name = "tb_blockToWrite";
            this.tb_blockToWrite.Size = new System.Drawing.Size(612, 21);
            this.tb_blockToWrite.TabIndex = 14;
            this.tb_blockToWrite.Text = "00000000";
            // 
            // nud_blockToWrite
            // 
            this.nud_blockToWrite.Location = new System.Drawing.Point(92, 86);
            this.nud_blockToWrite.Maximum = new decimal(new int[] {
            27,
            0,
            0,
            0});
            this.nud_blockToWrite.Name = "nud_blockToWrite";
            this.nud_blockToWrite.Size = new System.Drawing.Size(42, 21);
            this.nud_blockToWrite.TabIndex = 15;
            // 
            // nud_TagCount
            // 
            this.nud_TagCount.Location = new System.Drawing.Point(580, 48);
            this.nud_TagCount.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nud_TagCount.Name = "nud_TagCount";
            this.nud_TagCount.Size = new System.Drawing.Size(42, 21);
            this.nud_TagCount.TabIndex = 16;
            // 
            // nud_TagLoseCount
            // 
            this.nud_TagLoseCount.Location = new System.Drawing.Point(691, 48);
            this.nud_TagLoseCount.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nud_TagLoseCount.Name = "nud_TagLoseCount";
            this.nud_TagLoseCount.Size = new System.Drawing.Size(61, 21);
            this.nud_TagLoseCount.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(524, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "tagCount";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(629, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "lostCount";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tb_blockToWrite);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cb_Drivers);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cb_ports);
            this.panel1.Controls.Add(this.nud_TagLoseCount);
            this.panel1.Controls.Add(this.b_open);
            this.panel1.Controls.Add(this.nud_TagCount);
            this.panel1.Controls.Add(this.b_close);
            this.panel1.Controls.Add(this.nud_blockToWrite);
            this.panel1.Controls.Add(this.b_Inventory);
            this.panel1.Controls.Add(this.b_stopInventory);
            this.panel1.Controls.Add(this.cb_blockToWrite);
            this.panel1.Controls.Add(this.b_clear);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nud_blockToRead);
            this.panel1.Controls.Add(this.cb_blockToRead);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1135, 120);
            this.panel1.TabIndex = 20;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 626);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1135, 22);
            this.panel2.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 12);
            this.label5.TabIndex = 21;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(580, 171);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(344, 404);
            this.webBrowser1.TabIndex = 24;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 279);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(178, 341);
            this.textBox1.TabIndex = 25;
            this.textBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1135, 648);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JBBCounter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nud_blockToRead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_blockToWrite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TagCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TagLoseCount)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_Drivers;
        private System.Windows.Forms.ComboBox cb_ports;
        private System.Windows.Forms.Button b_open;
        private System.Windows.Forms.Button b_close;
        private System.Windows.Forms.Button b_Inventory;
        private System.Windows.Forms.Button b_stopInventory;
        private System.Windows.Forms.Button b_clear;
        private System.Windows.Forms.NumericUpDown nud_blockToRead;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_blockToRead;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cb_blockToWrite;
        private System.Windows.Forms.TextBox tb_blockToWrite;
        private System.Windows.Forms.NumericUpDown nud_blockToWrite;
        private System.Windows.Forms.NumericUpDown nud_TagCount;
        private System.Windows.Forms.NumericUpDown nud_TagLoseCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.RichTextBox textBox1;
    }
}

