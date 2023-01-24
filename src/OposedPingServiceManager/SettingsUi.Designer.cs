namespace OposedPingServiceManager
{
    partial class SettingsUi
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsUi));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtPingKey = new System.Windows.Forms.TextBox();
            this.txtRoomId = new System.Windows.Forms.TextBox();
            this.txtUrlHost = new System.Windows.Forms.TextBox();
            this.txtUrlPort = new System.Windows.Forms.TextBox();
            this.txtUrlSchema = new System.Windows.Forms.ComboBox();
            this.btnShowLogs = new System.Windows.Forms.Button();
            this.lblSaved = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(265, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This tool helps to set up and configure the ping service";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "PingService Manager";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Room";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Id:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Authentication";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "PingKey";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 190);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Schema:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 168);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Oposed";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 214);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Host:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 239);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Port:";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(303, 282);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtPingKey
            // 
            this.txtPingKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPingKey.Location = new System.Drawing.Point(74, 82);
            this.txtPingKey.Name = "txtPingKey";
            this.txtPingKey.Size = new System.Drawing.Size(304, 20);
            this.txtPingKey.TabIndex = 11;
            // 
            // txtRoomId
            // 
            this.txtRoomId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRoomId.Location = new System.Drawing.Point(74, 133);
            this.txtRoomId.Name = "txtRoomId";
            this.txtRoomId.Size = new System.Drawing.Size(304, 20);
            this.txtRoomId.TabIndex = 12;
            // 
            // txtUrlHost
            // 
            this.txtUrlHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrlHost.Location = new System.Drawing.Point(74, 211);
            this.txtUrlHost.Name = "txtUrlHost";
            this.txtUrlHost.Size = new System.Drawing.Size(304, 20);
            this.txtUrlHost.TabIndex = 14;
            // 
            // txtUrlPort
            // 
            this.txtUrlPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrlPort.Location = new System.Drawing.Point(74, 236);
            this.txtUrlPort.Name = "txtUrlPort";
            this.txtUrlPort.Size = new System.Drawing.Size(304, 20);
            this.txtUrlPort.TabIndex = 15;
            // 
            // txtUrlSchema
            // 
            this.txtUrlSchema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrlSchema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtUrlSchema.FormattingEnabled = true;
            this.txtUrlSchema.Items.AddRange(new object[] {
            "http",
            "https"});
            this.txtUrlSchema.Location = new System.Drawing.Point(74, 187);
            this.txtUrlSchema.Name = "txtUrlSchema";
            this.txtUrlSchema.Size = new System.Drawing.Size(304, 21);
            this.txtUrlSchema.TabIndex = 16;
            // 
            // btnShowLogs
            // 
            this.btnShowLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowLogs.Location = new System.Drawing.Point(222, 282);
            this.btnShowLogs.Name = "btnShowLogs";
            this.btnShowLogs.Size = new System.Drawing.Size(75, 23);
            this.btnShowLogs.TabIndex = 17;
            this.btnShowLogs.Text = "Show logs";
            this.btnShowLogs.UseVisualStyleBackColor = true;
            this.btnShowLogs.Click += new System.EventHandler(this.btnShowLogs_Click);
            // 
            // lblSaved
            // 
            this.lblSaved.AutoSize = true;
            this.lblSaved.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaved.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblSaved.Location = new System.Drawing.Point(15, 287);
            this.lblSaved.Name = "lblSaved";
            this.lblSaved.Size = new System.Drawing.Size(62, 13);
            this.lblSaved.TabIndex = 18;
            this.lblSaved.Text = "Saving ...";
            // 
            // SettingsUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 317);
            this.Controls.Add(this.lblSaved);
            this.Controls.Add(this.btnShowLogs);
            this.Controls.Add(this.txtUrlSchema);
            this.Controls.Add(this.txtUrlPort);
            this.Controls.Add(this.txtUrlHost);
            this.Controls.Add(this.txtRoomId);
            this.Controls.Add(this.txtPingKey);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsUi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Oposed PingService Manager";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtPingKey;
        private System.Windows.Forms.TextBox txtRoomId;
        private System.Windows.Forms.TextBox txtUrlHost;
        private System.Windows.Forms.TextBox txtUrlPort;
        private System.Windows.Forms.ComboBox txtUrlSchema;
        private System.Windows.Forms.Button btnShowLogs;
        private System.Windows.Forms.Label lblSaved;
    }
}

