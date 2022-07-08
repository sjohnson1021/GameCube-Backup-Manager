using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCBM

{

    public partial class TransferProgress : UserControl
    {
        public TransferProgress()
        {
            InitializeComponent();
        }

        //        private void CopyTaskGeneric(FileInfo _source, FileInfo _destination, ProgressBar bar, Label progress, Label name)

        private FileInfo _source;
        private FileInfo _destination;

        public TransferProgress(Label title, Label percent, ProgressBar bar, FileInfo destination, FileInfo source)
        {
            Title = title;
            Percent = percent;
            Bar = bar;
            Destination = destination;
            Source = source;
        }

        public TransferProgress(FileInfo destination, FileInfo source)
        {
            Destination = destination;
            Source = source;
        }
        private void CopyTaskGeneric(TransferProgress pg)
        {

            // Disc 1
            if (pg.Destination.Exists)
            {
                pg.Destination.Delete();
            }
            //Create a task to run copy file
            Task.Run(() =>
            {
                    //pg.source.CopyTo(pg.destination, true);
                pg.Source.CopyTo(pg.Destination, x => pg.Bar.BeginInvoke(new Action(() =>
                {
                    pg.Bar.Visible = true;
                    pg.Percent.Visible = true;
                    pg.Title.Visible = true;
                    pg.Bar.Value = x;
                    pg.Title.Text = GCBM.Properties.Resources.CopyTask_String2 + pg.Source.Name;
                    pg.Percent.Text = x.ToString() + "%";
                })));
            }).GetAwaiter().OnCompleted(() => pg.Bar.BeginInvoke(new Action(() =>
            {
                pg.Bar.Value = 100;
                pg.Title.Text = GCBM.Properties.Resources.CopyTask_String4;
                pg.Percent.Text = GCBM.Properties.Resources.CopyTask_String5;
                pg.Bar.Visible = false;
                pg.Percent.Visible = false;
                pg.Title.Visible = false;
            })));
        }


        /// <summary>
        /// The Destination file to be copied to.
        /// </summary>
        public FileInfo Destination
        {
            get => _destination;
            set => _destination = value;
        }
        /// <summary>
        /// The Source file to be copied from.
        /// </summary>
        public FileInfo Source
        {
            get => _source;
            set => _source = value;
        }
        /// <summary>
        /// The progress bar to be used to display the progress of the copy.
        /// </summary>
        public ProgressBar Bar
        {
            get => pbBar;
            private set => pbBar = value;
        }
        /// <summary>
        /// The label to be used to display the progress of the copy.
        /// </summary>
        public Label Title
        {
            get => lblName;
            private set => lblName = value;
        }
        /// <summary>
        /// The label to be used to display the progress of the copy.
        /// </summary>
        public Label Percent
        {
            get => lblPercent;
            set => lblPercent = value;
        }
    }
}

