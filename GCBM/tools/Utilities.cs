using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCBM;

public class Utilities
{
    #region Reload DataGridView List
    /// <summary>
    /// Reloads the contents of the DataGridView Games List.
    /// </summary>
    private void ReloadDataGridViewGameList(DataGridView dgv)
    {
        dgvSelected = dgv;
        if (dgv.RowCount != 0)
        {

            try
            {
                if (dgv == dgvGameList)
                {
                    DirectoryOpenGameList(dgv.CurrentRow.Cells[6].Value.ToString());
                }
                else
                {
                    DirectoryOpenDiscList(dgv.CurrentRow.Cells[6].Value.ToString());
                }
                if (ERROR == false)
                {
                    LoadCover(dgv.CurrentRow.Cells[3].Value.ToString());
                }
                // pictureBox GameID
                if (pbWebGameID.Enabled == false)
                {
                    pbWebGameID.Enabled = true;
                    pbWebGameID.Image = Resources.globe_earth_color_64;
                }

            }
            catch (Exception ex)
            {
                tbLog.AppendText("[" + DateString() + "]" + GCBM.Properties.Resources.Info + ex.Message);
                GlobalNotifications(ex.Message, ToolTipIcon.Error);
            }
        }
    }
    #endregion

    #region Display Files Selected
    /// <summary>
    /// Display Files Selected
    /// --sjohnson1021-bookmark
    /// </summary>
    /// <param name="sourceFolder"></param>
    /// <param name="dgv"></param>
    private void DisplayFilesSelected(string sourceFolder, DataGridView dgv)
    {
        var filters = new String[] { "iso", "gcm" };
        bool isRecursive;

        if (dgv == dgvGameList)
        {
            if (dgv.RowCount == 0)
            {
                btnGameInstallExactCopy.Enabled = true;
                btnGameInstallScrub.Enabled = true;
                tsmiReloadGameList.Enabled = true;
                tsmiGameListSelectAll.Enabled = true;
                tsmiGameListSelectNone.Enabled = true;
                tsmiGameListDeleteAllFiles.Enabled = true;
                tsmiGameListDeleteSelectedFile.Enabled = true;
                //tsmiGameListAllHashSHA1.Enabled = true;
                tsmiGameListHashSHA1.Enabled = true;
                tsmiDownloadCoversSelectedGame.Enabled = true;
                tsmiSyncDownloadDiscOnly3DCovers.Enabled = true;
                tsmiGameInfo.Enabled = true;
                tsmiTransferDeviceCovers.Enabled = true;
            }
        }

        if (dgv == dgvGameListDisc)
        {
            isRecursive = true;
        }
        else
        {
            isRecursive = false;
        }

        string[] files = GetFilesFolder(sourceFolder, filters, isRecursive);

        if (dgv == dgvGameListDisc)
        {
            //tsmiReloadGameListDisc.Enabled = true;
            try
            {
                if (dgv.RowCount == 0)
                {
                    tsmiReloadGameListDisc.Enabled = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        // Groups files in the folder by extension.
        var filesGroupedByExtension = files.Select(
            arq => Path.GetExtension(arq).TrimStart('.').ToLower(MY_CULTURE)).GroupBy(x => x, (ext, extCnt) =>
            new { _fileExtension = ext, Counter = extCnt.Count() });

        // Scroll through the result and display the values.
        foreach (var _files in filesGroupedByExtension)
        {
            tbLog.AppendText("[" + DateString() + "]" + GCBM.Properties.Resources.DisplayFilesSelected_Found_String1 + _files.Counter +
                GCBM.Properties.Resources.DisplayFilesSelected_Found_String2
                + _files._fileExtension.ToUpper(MY_CULTURE) + Environment.NewLine);

            GlobalNotifications(GCBM.Properties.Resources.DisplayFilesSelected_Found_String3 + _files.Counter +
                GCBM.Properties.Resources.DisplayFilesSelected_Found_String2 + _files._fileExtension.ToUpper(MY_CULTURE), ToolTipIcon.Info);

        }
        //txtLog.AppendText(Environment.NewLine + Environment.NewLine);

        // Creates a DataTable with file data.
        //DataTable _table = new DataTable();

        FileInfo _file = null;
        dgv.Rows.Clear();
        //dgvGameList.DataSource = GameDataTable();


        foreach (Game game in GameList(sourceFolder))
        {
            _file = new FileInfo(game.Path);


            string _getSize = DisplayFormatFileSize(_file.Length, CONFIG_INI_FILE.IniReadInt("GENERAL", "FileSize"));


            //5° coluna

            dgv.Rows.Add(false,
                            game.Title,
                            game.Region,
                            game.ID,
                            game.Extension.Substring(1, 3).Trim().ToUpper(MY_CULTURE),
                            _getSize,
                            game.Path);
        }


        //for (int i = 0; i < files.Length; i++)
        //{
        //    //FileInfo _file = new FileInfo(files[i]);
        //    _file = new FileInfo(files[i]);
        //    string _getSize = DisplayFormatFileSize(_file.Length, CONFIG_INI_FILE.IniReadInt("GENERAL", "FileSize"));
        //    //string _getSize = BytesToGB(_file.Length);                
        //    // 4° coluna
        //    _table.Rows.Add(_file.Name, _file.Extension.Substring(1, 3).Trim().ToUpper(MY_CULTURE), _getSize, _file.FullName);
        //    //_table.Rows.Add(_file.Name, _file.Extension.Substring(1, 3).Trim().ToUpper(myCulture), _getSize);
        //}

        //if(dgvGameList.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect){
        //    MessageBox.Show("O modo de seleção é RowHeaderSelect");
        //}
        //else
        //{
        //    MessageBox.Show("O modo de seleção NÃO é RowHeaderSelect");
        //}


        if (dgv == dgvGameList)
        {
            ReloadDataGridViewGameList(dgvGameList);
        }
        else if (dgv == dgvGameListDisc)
        {
            ReloadDataGridViewGameList(dgvGameListDisc);
        }
    }
    #endregion

    #region Display Files Selected
    /// <summary>
    /// Display Files Selected
    /// --sjohnson1021-bookmark
    /// </summary>
    /// <param name="sourceFolder"></param>
    /// <param name="dgv"></param>
    private void DisplayFilesSelected(string sourceFolder, DataGridView dgv, bool isRecursive)
    {
        var filters = new String[] { "iso", "gcm" };
        string[] files = GetFilesFolder(sourceFolder, filters, isRecursive);
        // Groups files in the folder by extension.
        var filesGroupedByExtension = files.Select(
            arq => Path.GetExtension(arq).TrimStart('.').ToLower(MY_CULTURE)).GroupBy(x => x, (ext, extCnt) =>
            new { _fileExtension = ext, Counter = extCnt.Count() });

        FileInfo _file = null;
        foreach (Game game in GameList(sourceFolder))
        {
            _file = new FileInfo(game.Path);


            string _getSize = DisplayFormatFileSize(_file.Length, CONFIG_INI_FILE.IniReadInt("GENERAL", "FileSize"));


            //5° coluna

            dgv.Rows.Add(false,
                            game.Title,
                            game.Region,
                            game.ID,
                            game.Extension.Substring(1, 3).Trim().ToUpper(MY_CULTURE),
                            _getSize,
                            game.Path);
        }
        ReloadDataGridViewGameList(dgv);
    }
    #endregion

    #region Get Files Folder
    /// <summary>
    /// Get files from folder origem.
    /// </summary>
    /// <param name="rootFolder"></param>
    /// <param name="filters"></param>
    /// <param name="isRecursive"></param>
    /// <returns></returns>
    private string[] GetFilesFolder(string rootFolder, string[] filters, bool isRecursive)
    {
        List<string> filesFound = new List<string>();
        // Sets options for displaying root folder images.

        var optionSearch = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        foreach (var filter in filters)
        {
            try
            {
                filesFound.AddRange(Directory.GetFiles(rootFolder, string.Format("*.{0}", filter), optionSearch));
            }
            catch (Exception ex)
            {

            }
        }
        return filesFound.ToArray();
    }

    /// <summary>
    ///  Get files from folder destin.
    /// </summary>
    /// <param name="rootFolder"></param>
    /// <param name="filters"></param>
    /// <param name="isRecursive"></param>
    /// <returns></returns>
    //private string[] GetFilesFolderDisc(string rootFolder, string[] filters, bool isRecursive)
    //{
    //    List<string> filesFound = new List<string>();
    //    // Sets options for displaying root folder images.
    //    //isRecursive = false;

    //    var optionSearch = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
    //    foreach (var filter in filters)
    //    {
    //        filesFound.AddRange(Directory.GetFiles(rootFolder, string.Format("*.{0}", filter), optionSearch));
    //    }
    //    return filesFound.ToArray();
    //}
    #endregion

    #region Get All Drives
    /// <summary>
    /// Gets a list of all connected drives, and lists them in a ComboBox.
    /// </summary>
    /// <param name="box">ComboBox to display drives in.</param>
    private void GetAllDrives(ComboBox box)
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        box.Items.Clear();
        box.Items.Add(GCBM.Properties.Resources.GetAllDrives_Inactive);
        box.SelectedIndex = 0;

        foreach (DriveInfo d in allDrives)
        {
            if (d.IsReady == true)
            {
                box.Items.Add(d.Name);
            }
        }
    }
    #endregion

    #region Display Format File Size
    /// <summary>
    /// Adjusts the file size display format.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public static string DisplayFormatFileSize(long i, int k)
    {
        // Obtém o valor absoluto
        long i_absolute = (i < 0 ? -i : i);
        string suffix;
        double reading;

        if (k == 0)
        {
            if (i_absolute >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                reading = (i >> 50);
            }
            else if (i_absolute >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                reading = (i >> 40);
            }
            else if (i_absolute >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                reading = (i >> 30);
            }
            else if (i_absolute >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                reading = (i >> 20);
            }
            else if (i_absolute >= 0x100000) // Megabyte
            {
                suffix = "MB";
                reading = (i >> 10);
            }
            else if (i_absolute >= 0x400) // Kilobyte
            {
                suffix = "KB";
                reading = i;
            }
            else
            {
                return i.ToString("0 bytes"); // Byte
            }
        }
        else if (k == 1) // Kilobyte
        {
            suffix = "KB";
            reading = i;
        }
        else if (k == 2) // Megabyte
        {
            suffix = "MB";
            reading = (i >> 10);
        }
        else if (k == 3) // Gigabyte
        {
            suffix = "GB";
            reading = (i >> 20);
        }
        else if (k == 4) // Terabyte
        {
            suffix = "TB";
            reading = (i >> 30);
        }
        else
        {
            return i.ToString("0 bytes"); // Byte
        }
        // Divide by 1024 to get the fractional value.
        reading = (reading / 1024);
        // Returns the suffix formatted number.
        return reading.ToString("0.## ") + suffix;
    }
    #endregion

    #region Display Format File Size (Basic Version - Automatic)
    /// <summary>
    /// Display Format File Size (Basic Version - Automatic)
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string BytesToGB(long bytes)
    {
        string result;
        double _bytes = bytes;
        string[] array_fs = new string[5] { "B", "KB", "MB", "GB", "TB" };
        int num2_fs = 0;

        while (_bytes >= 1024.0 && num2_fs < array_fs.Length - 1)
        {
            num2_fs++;
            _bytes /= 1024.0;
        }
        result = $"{_bytes:0.##} {array_fs[num2_fs]}";

        return result;
    }
    #endregion

}
