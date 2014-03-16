namespace KeyBV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.B_KS = new System.Windows.Forms.Button();
            this.B_V2 = new System.Windows.Forms.Button();
            this.B_V1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.B_Break = new System.Windows.Forms.Button();
            this.TB_V2 = new System.Windows.Forms.TextBox();
            this.TB_V1 = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.TB_Path = new System.Windows.Forms.TextBox();
            this.B_ChangePath = new System.Windows.Forms.Button();
            this.B_Dump = new System.Windows.Forms.Button();
            this.CHK_PK6 = new System.Windows.Forms.CheckBox();
            this.CB_Mode = new System.Windows.Forms.ComboBox();
            this.B_Key = new System.Windows.Forms.Button();
            this.B_Video = new System.Windows.Forms.Button();
            this.TB_Key = new System.Windows.Forms.TextBox();
            this.TB_Video = new System.Windows.Forms.TextBox();
            this.RTB = new System.Windows.Forms.RichTextBox();
            this.CB_TeamSelect = new System.Windows.Forms.ComboBox();
            this.CB_TeamSelect2 = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(7, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(270, 172);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.CB_TeamSelect);
            this.tabPage2.Controls.Add(this.B_KS);
            this.tabPage2.Controls.Add(this.B_V2);
            this.tabPage2.Controls.Add(this.B_V1);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.B_Break);
            this.tabPage2.Controls.Add(this.TB_V2);
            this.tabPage2.Controls.Add(this.TB_V1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(262, 146);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Video Breaker";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // B_KS
            // 
            this.B_KS.Enabled = false;
            this.B_KS.Location = new System.Drawing.Point(202, 115);
            this.B_KS.Name = "B_KS";
            this.B_KS.Size = new System.Drawing.Size(53, 23);
            this.B_KS.TabIndex = 14;
            this.B_KS.Text = "KS";
            this.B_KS.UseVisualStyleBackColor = true;
            this.B_KS.Click += new System.EventHandler(this.dumpkey);
            // 
            // B_V2
            // 
            this.B_V2.Location = new System.Drawing.Point(6, 63);
            this.B_V2.Name = "B_V2";
            this.B_V2.Size = new System.Drawing.Size(58, 23);
            this.B_V2.TabIndex = 13;
            this.B_V2.Text = "Video 2";
            this.B_V2.UseVisualStyleBackColor = true;
            this.B_V2.Click += new System.EventHandler(this.openfile);
            // 
            // B_V1
            // 
            this.B_V1.Location = new System.Drawing.Point(6, 17);
            this.B_V1.Name = "B_V1";
            this.B_V1.Size = new System.Drawing.Size(56, 23);
            this.B_V1.TabIndex = 12;
            this.B_V1.Text = "Video 1";
            this.B_V1.UseVisualStyleBackColor = true;
            this.B_V1.Click += new System.EventHandler(this.openfile);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(183, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "2 Pokemon - 2nd Mon = from Video 1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(93, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Only 1 Pokemon - Singles";
            // 
            // B_Break
            // 
            this.B_Break.Enabled = false;
            this.B_Break.Location = new System.Drawing.Point(121, 115);
            this.B_Break.Name = "B_Break";
            this.B_Break.Size = new System.Drawing.Size(75, 23);
            this.B_Break.TabIndex = 8;
            this.B_Break.Text = "Break";
            this.B_Break.UseVisualStyleBackColor = true;
            this.B_Break.Click += new System.EventHandler(this.dobreak);
            // 
            // TB_V2
            // 
            this.TB_V2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_V2.Location = new System.Drawing.Point(70, 65);
            this.TB_V2.Name = "TB_V2";
            this.TB_V2.ReadOnly = true;
            this.TB_V2.Size = new System.Drawing.Size(185, 20);
            this.TB_V2.TabIndex = 5;
            // 
            // TB_V1
            // 
            this.TB_V1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_V1.Location = new System.Drawing.Point(68, 18);
            this.TB_V1.Name = "TB_V1";
            this.TB_V1.ReadOnly = true;
            this.TB_V1.Size = new System.Drawing.Size(187, 20);
            this.TB_V1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.CB_TeamSelect2);
            this.tabPage1.Controls.Add(this.TB_Path);
            this.tabPage1.Controls.Add(this.B_ChangePath);
            this.tabPage1.Controls.Add(this.B_Dump);
            this.tabPage1.Controls.Add(this.CHK_PK6);
            this.tabPage1.Controls.Add(this.CB_Mode);
            this.tabPage1.Controls.Add(this.B_Key);
            this.tabPage1.Controls.Add(this.B_Video);
            this.tabPage1.Controls.Add(this.TB_Key);
            this.tabPage1.Controls.Add(this.TB_Video);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(262, 146);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Video Ripper";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TB_Path
            // 
            this.TB_Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Path.Location = new System.Drawing.Point(70, 91);
            this.TB_Path.Name = "TB_Path";
            this.TB_Path.ReadOnly = true;
            this.TB_Path.Size = new System.Drawing.Size(187, 20);
            this.TB_Path.TabIndex = 22;
            this.TB_Path.Visible = false;
            // 
            // B_ChangePath
            // 
            this.B_ChangePath.Location = new System.Drawing.Point(6, 90);
            this.B_ChangePath.Name = "B_ChangePath";
            this.B_ChangePath.Size = new System.Drawing.Size(56, 23);
            this.B_ChangePath.TabIndex = 21;
            this.B_ChangePath.Text = "Path";
            this.B_ChangePath.UseVisualStyleBackColor = true;
            this.B_ChangePath.Visible = false;
            this.B_ChangePath.Click += new System.EventHandler(this.changepath);
            // 
            // B_Dump
            // 
            this.B_Dump.Location = new System.Drawing.Point(211, 117);
            this.B_Dump.Name = "B_Dump";
            this.B_Dump.Size = new System.Drawing.Size(46, 23);
            this.B_Dump.TabIndex = 20;
            this.B_Dump.Text = "Dump";
            this.B_Dump.UseVisualStyleBackColor = true;
            this.B_Dump.Click += new System.EventHandler(this.dodump);
            // 
            // CHK_PK6
            // 
            this.CHK_PK6.AutoSize = true;
            this.CHK_PK6.Checked = true;
            this.CHK_PK6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_PK6.Location = new System.Drawing.Point(72, 121);
            this.CHK_PK6.Name = "CHK_PK6";
            this.CHK_PK6.Size = new System.Drawing.Size(46, 17);
            this.CHK_PK6.TabIndex = 19;
            this.CHK_PK6.Text = "PK6";
            this.CHK_PK6.UseVisualStyleBackColor = true;
            this.CHK_PK6.Visible = false;
            // 
            // CB_Mode
            // 
            this.CB_Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Mode.FormattingEnabled = true;
            this.CB_Mode.Items.AddRange(new object[] {
            "Default",
            "Reddit",
            "TSV",
            ".csv",
            "Files"});
            this.CB_Mode.Location = new System.Drawing.Point(120, 118);
            this.CB_Mode.Name = "CB_Mode";
            this.CB_Mode.Size = new System.Drawing.Size(85, 21);
            this.CB_Mode.TabIndex = 18;
            this.CB_Mode.SelectedIndexChanged += new System.EventHandler(this.changemode);
            // 
            // B_Key
            // 
            this.B_Key.Location = new System.Drawing.Point(6, 42);
            this.B_Key.Name = "B_Key";
            this.B_Key.Size = new System.Drawing.Size(56, 23);
            this.B_Key.TabIndex = 17;
            this.B_Key.Text = "Key";
            this.B_Key.UseVisualStyleBackColor = true;
            this.B_Key.Click += new System.EventHandler(this.openfile);
            // 
            // B_Video
            // 
            this.B_Video.Location = new System.Drawing.Point(6, 17);
            this.B_Video.Name = "B_Video";
            this.B_Video.Size = new System.Drawing.Size(56, 23);
            this.B_Video.TabIndex = 16;
            this.B_Video.Text = "Video";
            this.B_Video.UseVisualStyleBackColor = true;
            this.B_Video.Click += new System.EventHandler(this.openfile);
            // 
            // TB_Key
            // 
            this.TB_Key.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Key.Location = new System.Drawing.Point(68, 44);
            this.TB_Key.Name = "TB_Key";
            this.TB_Key.ReadOnly = true;
            this.TB_Key.Size = new System.Drawing.Size(187, 20);
            this.TB_Key.TabIndex = 15;
            // 
            // TB_Video
            // 
            this.TB_Video.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Video.Location = new System.Drawing.Point(68, 18);
            this.TB_Video.Name = "TB_Video";
            this.TB_Video.ReadOnly = true;
            this.TB_Video.Size = new System.Drawing.Size(187, 20);
            this.TB_Video.TabIndex = 14;
            // 
            // RTB
            // 
            this.RTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RTB.Location = new System.Drawing.Point(7, 190);
            this.RTB.Name = "RTB";
            this.RTB.ReadOnly = true;
            this.RTB.Size = new System.Drawing.Size(270, 167);
            this.RTB.TabIndex = 5;
            this.RTB.Text = "";
            this.RTB.WordWrap = false;
            // 
            // CB_TeamSelect
            // 
            this.CB_TeamSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TeamSelect.FormattingEnabled = true;
            this.CB_TeamSelect.Items.AddRange(new object[] {
            "My Team",
            "Opponent\'s Team"});
            this.CB_TeamSelect.Location = new System.Drawing.Point(8, 116);
            this.CB_TeamSelect.Name = "CB_TeamSelect";
            this.CB_TeamSelect.Size = new System.Drawing.Size(107, 21);
            this.CB_TeamSelect.TabIndex = 15;
            this.CB_TeamSelect.SelectedIndexChanged += new System.EventHandler(this.changeteam);
            // 
            // CB_TeamSelect2
            // 
            this.CB_TeamSelect2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TeamSelect2.FormattingEnabled = true;
            this.CB_TeamSelect2.Items.AddRange(new object[] {
            "My Team",
            "Opponent\'s Team"});
            this.CB_TeamSelect2.Location = new System.Drawing.Point(149, 67);
            this.CB_TeamSelect2.Name = "CB_TeamSelect2";
            this.CB_TeamSelect2.Size = new System.Drawing.Size(107, 21);
            this.CB_TeamSelect2.TabIndex = 23;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 362);
            this.Controls.Add(this.RTB);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 400);
            this.Name = "Form1";
            this.Text = "KeyBV";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button B_KS;
        private System.Windows.Forms.Button B_V2;
        private System.Windows.Forms.Button B_V1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button B_Break;
        private System.Windows.Forms.TextBox TB_V2;
        private System.Windows.Forms.TextBox TB_V1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button B_Dump;
        private System.Windows.Forms.CheckBox CHK_PK6;
        private System.Windows.Forms.ComboBox CB_Mode;
        private System.Windows.Forms.Button B_Key;
        private System.Windows.Forms.Button B_Video;
        private System.Windows.Forms.TextBox TB_Key;
        private System.Windows.Forms.TextBox TB_Video;
        private System.Windows.Forms.RichTextBox RTB;
        private System.Windows.Forms.TextBox TB_Path;
        private System.Windows.Forms.Button B_ChangePath;
        private System.Windows.Forms.ComboBox CB_TeamSelect;
        private System.Windows.Forms.ComboBox CB_TeamSelect2;
    }
}

