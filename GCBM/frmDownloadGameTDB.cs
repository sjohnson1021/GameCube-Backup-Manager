﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.InteropServices;
using System.Threading;

namespace GCBM
{
    public partial class frmDownloadGameTDB : Form
    {
        private static string GET_CURRENT_PATH   = Directory.GetCurrentDirectory();
        private static string DOWNLOAD_FILE      = GCBM.Properties.Resources.DownloadingWiiTDB_String1;
        private static string EXTRACT_FILE       = GCBM.Properties.Resources.DownloadingWiiTDB_String2;
        private static string PROCESS_COMPLETED  = GCBM.Properties.Resources.DownloadingWiiTDB_String3;
        private const string WIITDB_FILE         = "wiitdb.xml";
        private const string WIITDB_ZIP_FILE     = "wiitdb.zip";
        public int RETURN_CONFIRM { get; set; }

        public frmDownloadGameTDB()
        {
            InitializeComponent();

            btnCancelWork.Enabled = false;
            GameTDB();
        }

        private void GameTDB()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri("https://www.gametdb.com/wiitdb.zip"), WIITDB_ZIP_FILE);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lblDownload.Font = new Font(lblDownload.Font, FontStyle.Bold);
            lblDownload.Text = DOWNLOAD_FILE + e.ProgressPercentage + "%";
            pbGameTDB.Value = e.ProgressPercentage;
        }

        private async void Completed(object sender, AsyncCompletedEventArgs e)
        {
            lblExtracting.Font = new Font(lblExtracting.Font, FontStyle.Bold);
            lblExtracting.Text = EXTRACT_FILE; 
            
            try
            {
                ZipFile.ExtractToDirectory(GET_CURRENT_PATH + @"\" + WIITDB_ZIP_FILE, GET_CURRENT_PATH);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                File.Delete(GET_CURRENT_PATH + @"\" + WIITDB_ZIP_FILE);

                await ProcessTaskDelay();
                FileInfo fileinfo = new FileInfo(GET_CURRENT_PATH + @"\" + WIITDB_FILE);
                if (fileinfo.Length >= 31035000) //31035596
                {
                    lblConverting.Font = new Font(lblConverting.Font, FontStyle.Bold);
                    lblConverting.Text = PROCESS_COMPLETED;
                    btnCancelWork.Enabled = true;
                }
            }
        }

        async Task ProcessTaskDelay()
        {
            await Task.Delay(5000);
        }

        private void btnCancelWork_Click(object sender, EventArgs e)
        {
            //SaveConfigFile();

            //MessageBox.Show("Deseja mesmo sair?", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.RETURN_CONFIRM = 1;
            this.Close();
            this.Dispose();
        }

        const int MF_BYPOSITION = 0x400;

        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);

        private void frmDownloadGameTDB_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int MenuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, MenuItemCount - 1, MF_BYPOSITION);
        }
    }
}
