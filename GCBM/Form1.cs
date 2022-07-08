using GCBM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCBM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region PROPERTIES
        /// <summary>
        /// Statics
        /// </summary>
        private static string GET_CURRENT_PATH = Directory.GetCurrentDirectory();
        private static string GAMES_DIR = @"games";
        private static string TEMP_DIR = @"\temp";
        private static string COVERS_DIR = @"\covers\cache";
        private static string MEDIA_DIR = @"\media\covers";
        private static string CULTURE_CURRENT = "pt-BR";
        private static string PROG_UPDATE = "08/05/2022";
        private static string FAT32 = "FAT32";
        private static string NTFS = "NTFS";
        private static string EXFAT_FAT64 = "EXFAT";
        private static string RES_PATH;
        private static string IMAGE_PATH;
        private static string LINK_DOMAIN;
        private static string FLUSH_SD;
        private static string SCRUB_ALIGN;
        private static char REGION = 'n';
        private const string INI_FILE = "config.ini";
        private const string WIITDB_FILE = "wiitdb.xml";
        private const string WIITDB_DOWNLOAD_SITE = "https://www.gametdb.com/";
        private const string en_US = "en-US";
        private bool ERROR = false;
        private bool SPLASH_SCREEN_DONE = false;
        private bool ROOT_OPENED = true;
        private bool FILENAME_SORT = true;
        private bool RETRIEVE_FILES_INFO = true;
        private readonly Assembly assembly = Assembly.GetExecutingAssembly();
        private readonly IniFile CONFIG_INI_FILE = new IniFile(INI_FILE);
        private readonly CultureInfo MY_CULTURE = new CultureInfo(CULTURE_CURRENT, false);
        private readonly ProcessStartInfo START_INFO = new ProcessStartInfo();
        private readonly WebClient NET_CLIENT = new WebClient();
        private HttpWebResponse NET_RESPONSE = null;
        private frmSplashScreen SPLASH_SCREEN;

        private bool WORKING;
        private string dgvGameListPath;
        private string dgvGameListDiscPath;
        private int intQueueLength;
        private int intQueuePos;
        private List<string> lstInstallQueue = new List<string>();
        private List<Game> sourceGames = new List<Game>();
        private DataGridView dgvSelected = new DataGridView();

        [DllImport("kernel32.dll")]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
        #endregion

        // REWRITE FUNCTION - Reload DataGridView List

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

            if (CONFIG_INI_FILE.IniReadBool("SEVERAL", "RecursiveMode") == true)
            {
                isRecursive = true;
            }
            else
            {
                isRecursive = false;
            }

            var optionSearch = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                try
                {
                    filesFound.AddRange(Directory.GetFiles(rootFolder, string.Format("*.{0}", filter), optionSearch).AsParallel());
                }
                catch (Exception ex)
                {

                }
            }
            return filesFound.ToArray();
        }
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

            allDrives.Where(d => d.IsReady).AsParallel().ForAll(r => box.Items.Add(r.Name));
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

        #region Build Game list as List<Game>
        /// <summary>
        /// Build a List<Game> with file and game info for easier access programmatically.
        /// </summary>
        private List<Game> GameList(string path)
        {
            string[] filters = { "ISO", "GCM" };
            List<Game> list = new List<Game>();
            string[] files = GetFilesFolder(path, filters, false);
            foreach (var file in files)
            {
                FileInfo _file = new FileInfo(file);
                //Title - ID - Region - Path - Extension - Size
                DirectoryOpenGameList(_file.FullName);
                Game game = new Game(tbIDName.Text, tbIDGame.Text, tbIDRegion.Text, loadPath, _file.Extension, (int)_file.Length);
                //game.Title = _oldNameInternal;
                //game.Region = _IDRegionCode;
                //game.Path = _file.FullName;
                //game.Extension = _file.Extension;
                //game.Size = (int)_file.Length;
                list.Add(game);
            }
            return list;
        }
        #endregion

        private void DirectoryOpenGameList(string path)
        {
            //OpenFileDialog ofd;

            //if (path.Length == 0)
            //{
            //    ofd = new OpenFileDialog() { Filter = "GameCube ISO (*.iso)|*.iso|GameCube Image File (*.gcm)|*.gcm|All files (*.*)|*.*", Title = "Open image" };
            //    if (imgPath != "")
            //        ofd.FileName = imgPath;
            //    if (ofd.ShowDialog() == DialogResult.OK)
            //    {
            //        imgPath = ofd.FileName;
            //        path = imgPath;
            //    }
            //}

            if (path.Length == 0)
                return;

            IMAGE_PATH = path;

            if (CheckImage())
            {
                if (ReadImageTOC())
                {
                    if (CONFIG_INI_FILE.IniReadBool("TITLES", "GameXmlName") == true)
                    {
                        if (File.Exists(WIITDB_FILE))
                        {
                            XElement root = XElement.Load(WIITDB_FILE);
                            IEnumerable<XElement> tests = from el in root.Elements("game") where (string)el.Element("id") == tbIDGame.Text select el;
                            foreach (XElement el in tests)
                            {
                                tbIDName.Text = (string)el.Element("locale").Element("title");
                            }
                        }
                        else
                        {
                            CheckWiiTdbXml();
                        }
                    }
                    //miImageOpen.ToolTipText = imgPath;
                    ROOT_OPENED = false;
                }
            }
        }

        #region Check Image
        /// <summary>
        /// Checks if it is a valid Nintendo GameCube file.
        /// </summary>
        /// <returns></returns>
        private bool CheckImage()
        {
            sio.FileStream fs = null;
            sio.BinaryReader br = null;

            try
            {
                fs = new sio.FileStream(IMAGE_PATH, sio.FileMode.Open, sio.FileAccess.Read, sio.FileShare.Read);
                br = new sio.BinaryReader(fs, ste.Default);

                fs.Position = 0x1c;
                if (br.ReadInt32() != 0x3d9f33c2)
                {
                    tbLog.AppendText("[" + DateString() + "]" + GCBM.Properties.Resources.NotNintendoGameCubeFile);
                    GlobalNotifications(GCBM.Properties.Resources.NotNintendoGameCubeFile + Environment.NewLine +
                        GCBM.Properties.Resources.RecommendedDeleteOrMoveFile, ToolTipIcon.Info);

                    pbGameBox.LoadAsync(GET_CURRENT_PATH + MEDIA_DIR + @"\disc.png");
                    pbGameCover3D.LoadAsync(GET_CURRENT_PATH + MEDIA_DIR + @"\3d.png");

                    ERROR = true;
                    // INVALID FILE!!!
                }
                else
                {
                    ERROR = false;
                    // VALID FILE!!!
                }
            }
            catch (Exception ex)
            {
                GlobalNotifications(ex.Message, ToolTipIcon.Error);
                ERROR = true;
            }
            finally
            {
                if (br != null) br.Close();
                if (fs != null) fs.Close();
            }

            return !ERROR;
        }
        #endregion

        #region Load Cover
        /// <summary>
        /// Loads the respective Disk and 2D image files into the loaded ISO/GCM file.
        /// </summary>
        //private void LoadCover()
        private void LoadCover(string _idGame)
        {
            try
            {
                switch (_IDRegionCode)
                {
                    case "e": // AMERICA - USA
                        LINK_DOMAIN = "US";
                        break;
                    case "p": // EUROPE - ALL
                    case "r": // EUROPE - RUSSIA                   
                        LINK_DOMAIN = "EN";
                        break;
                    case "j": // ASIA - JAPAN
                    case "t": // ASIA - TAIWAN
                    case "k": // ASIA - KOREA
                        LINK_DOMAIN = "JA";
                        break;
                    case "d": // EUROPE - GERMANY
                        LINK_DOMAIN = "DE";
                        break;
                    case "s": // EUROPE - SPAIN
                        LINK_DOMAIN = "ES";
                        break;
                    case "i": // EUROPE - ITALY
                        LINK_DOMAIN = "IT";
                        break;
                    case "u": // AUSTRALIA
                        LINK_DOMAIN = "AU";
                        break;
                    case "y": // EUROPE - Netherlands ???
                        LINK_DOMAIN = "NL";
                        break;
                    case "f": // EUROPE - FRANCE
                        LINK_DOMAIN = "FR";
                        break;
                    default:
                        LINK_DOMAIN = "US";
                        //GlobalNotifications(GCBM.Properties.Resources.UnknownRegion, ToolTipIcon.Info);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (File.Exists(GET_CURRENT_PATH + COVERS_DIR + @"\" + LINK_DOMAIN + @"\disc\" + _idGame + ".png"))
            {
                pbGameBox.LoadAsync(GET_CURRENT_PATH + COVERS_DIR + @"\" + LINK_DOMAIN + @"\disc\" + _idGame + ".png");
            }
            else
            {
                pbGameBox.LoadAsync(GET_CURRENT_PATH + MEDIA_DIR + @"\disc.png");
            }

            if (File.Exists(GET_CURRENT_PATH + COVERS_DIR + @"\" + LINK_DOMAIN + @"\3d\" + _idGame + ".png"))
            {
                pbGameCover3D.LoadAsync(GET_CURRENT_PATH + COVERS_DIR + @"\" + LINK_DOMAIN + @"\3d\" + _idGame + ".png");
            }
            else
            {
                pbGameCover3D.LoadAsync(GET_CURRENT_PATH + MEDIA_DIR + @"\3d.png");
            }
        }
        #endregion


        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.SelectedPath;

            }
        }
    }
}
