using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using OposedPingService;

namespace OposedPingServiceManager
{
    public partial class SettingsUi : Form
    {
        public SettingsUi()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            lblSaved.Visible = false;

            txtRoomId.Text = Settings.Get("roomId", "0", null, true);
            txtUrlSchema.Text = Settings.Get("pingUrlSchema", "http", null, true);
            txtUrlHost.Text = Settings.Get("pingUrlHost", "local.host", null, true);
            txtUrlPort.Text = Settings.Get("pingUrlPort", "80", null, true);
            txtPingKey.Text = Settings.Get("pingKey", "", null, true);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            Settings.Set("roomId", txtRoomId.Text);
            Settings.Set("pingUrlSchema", txtUrlSchema.Text);
            Settings.Set("pingUrlHost", txtUrlHost.Text);
            Settings.Set("pingUrlPort", txtUrlPort.Text);
            Settings.Set("pingKey", txtPingKey.Text);

            string path = System.Environment.CurrentDirectory;
            Cmd($"SC CREATE \"OposedPing\" binpath=\"{path}\\OposedPingService.exe\" start=auto");

            lblSaved.Visible = true;
            
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
            });
            lblSaved.Visible = false;
            btnSave.Enabled = true;
        }

        private void btnShowLogs_Click(object sender, EventArgs e)
        {
            Cmd("eventvwr");
        }

        private void Cmd(string command)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }
    }
}
