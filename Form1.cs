using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PS3Lib;
using System.IO;

namespace StayboogyUP
{
    public partial class Form1 : Form
    {
        public static CCAPI ps3 = new CCAPI();

        public Form1()
        {
            InitializeComponent();
            ConnectAttach.ForeColor = Color.DeepPink;
            Disconnect.Text = "Not Connected";
            CustomCode.Enabled = false;
            stayboogyLink.ForeColor = Color.Aqua;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("stayboogyUP PS3 MW3 Solo Survival RTM Tool \n\n1) Make sure your PS3 is powered on. \n\n2) Make sure your PS3 is connected to the same network as your computer. \n\n3) Make sure MW3 is already running.");
            Disconnect.Enabled = false;
            stayboogyUP.Enabled = false;
            GodMode.Enabled = false;
            SoftReboot.Enabled = false;
            textBox1.Text = "offset: 0x10101010 = 10101010";
            textBox2.Text = "bytes: 0x00 0x00 = 0000";
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            Manly.Enabled = false;
        }

        private void ConnectAttach_Click(object sender, EventArgs e)
        {
            {
                if (ps3.ConnectTarget())
                {
                    ps3.AttachProcess();
                    MessageBox.Show("Connected!");
                    ConnectAttach.Text = "Connected";
                    ConnectAttach.Enabled = false;
                    Disconnect.Text = "Disconnect";
                    Disconnect.ForeColor = Color.DeepPink;
                    Disconnect.Enabled = true;
                    stayboogyUP.ForeColor = Color.Blue;
                    stayboogyUP.Enabled = true;
                    GodMode.ForeColor = Color.Blue;
                    GodMode.Enabled = true;
                    SoftReboot.ForeColor = Color.DeepPink;
                    SoftReboot.Enabled = true;
                    CustomCode.Enabled = true;
                    CustomCode.ForeColor = Color.DeepPink;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Couldn't connect to PS3");
                    ConnectAttach.ForeColor = Color.DeepPink;
                    ConnectAttach.Text = "Try Again!";
                    Disconnect.Text = "Not Connected";
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                }
            }
        }

        private void stayboogyUP_Click(object sender, EventArgs e)
        {
            // UAV always on
            uint UAV = 0x18DB880;
            byte[] UAVON = { 0x10 };
            ps3.SetMemory(UAV, UAVON);
            uint UAV2 = 0x5F067;
            byte[] UAV2ON = { 0x02 };
            ps3.SetMemory(UAV2, UAV2ON);

            // Have all Perks
            uint Perks = 0x01227788;
            byte[] PerksON = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            ps3.SetMemory(Perks, PerksON);

            // Max Ammo for everything
            uint[] Ammo = { 0x01227631, 0x01227649, 0x012276cd, 0x012276d9, 0x012276b5, 0x012276c1, 0x012276d9, 0x012276e5, 0x012326f9, 0x01232711, 0x01232771, 0x01232795, 0x0123277d, 0x01232789, 0x012327a1, 0x012327ad };
            for (int i = 0; i < Ammo.Length; i++)
            {
                ps3.SetMemory(Ammo[i], new byte[] { 0x00, 0x90 });
            }
            // Notification Toast
            ps3.Notify(CCAPI.NotifyIcon.INFO, "Perked Up! Loaded Up!");
        }

        private void GodMode_Click(object sender, EventArgs e)
        {
            // God Mode
            string godly = "3831";
            ps3.SetMemory(0x012272ea, godly);

            // Notification Toast
            ps3.Notify(CCAPI.NotifyIcon.INFO, "God Mode!");
            GodMode.Enabled = false;
            Manly.Enabled = true;
            Manly.ForeColor = Color.Blue;
        }

        private void Manly_Click(object sender, EventArgs e)
        {
            // God Mode
            string manly = "3830";
            ps3.SetMemory(0x012272ea, manly);

            // Notification Toast
            ps3.Notify(CCAPI.NotifyIcon.INFO, "Just a Man!");
            GodMode.Enabled = true;
            Manly.Enabled = false;
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {
            // Notification Toast
            ps3.Notify(CCAPI.NotifyIcon.INFO, "RTM Disconnected");
            ps3.DisconnectTarget();
            ConnectAttach.Enabled = true;
            ConnectAttach.ForeColor = Color.DeepPink;
            ConnectAttach.Text = "Re-Connect";
            Disconnect.ForeColor = Color.DeepPink;
            Disconnect.Text = "Disconnected";
            Disconnect.Enabled = false;
            stayboogyUP.Enabled = false;
            GodMode.Enabled = false;
            SoftReboot.Enabled = false;
            CustomCode.Enabled = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox1.Enabled = false;
            textBox2.Enabled = false;
        }

        private void SoftReboot_Click(object sender, EventArgs e)
        {
            ps3.ShutDown(CCAPI.RebootFlags.SoftReboot);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //this string is converted to an offset
            textBox1.Enabled = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //this string is converted to a BigEndian byte array
            textBox2.Enabled = true;
        }

        private void CustomCode_Click(object sender, EventArgs e)
        {
            // custom offset address and bytes input
            uint CustomOffset = Convert.ToUInt32(textBox1.Text, 16);
            string Data = textBox2.Text;
            byte[] CustomCode = System.Text.Encoding.BigEndianUnicode.GetBytes(Data);
            ps3.SetMemory(CustomOffset, CustomCode);

            // Notification Toast
            ps3.Notify(CCAPI.NotifyIcon.INFO, "Custom Memory Set");

            //uint CustomOffset = Convert.ToUInt32(textBox1.Text, 16);
            //uint CustomOffset = uint.Parse(textBox1.Text, System.Globalization.NumberStyles.HexNumber);
            //byte[] CustomCode = Encoding.ASCII.GetBytes(textBox2.Text);
        }

        private void stayboogyLink_Click(object sender, EventArgs e)
        {
            var developer = "https://github.com/stayboogy";
            var devProps = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = developer
            };
            System.Diagnostics.Process.Start(devProps);
        }
    }
}
