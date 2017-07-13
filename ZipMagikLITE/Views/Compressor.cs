using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Ionic.Zip;

using ZipMagikLITE.Models;

namespace ZipMagikLITE.Views {

    public enum CompressorType {
        Archive, Extract
    }

    public partial class Compressor : Form {

        private CompressorType _type;
        private bool? _encrypt;
        private string _destinationFolder;
        private object _file;

        public Compressor(string destinationFolder, bool encrypt, params string[] files) {
            InitializeComponent();
            // This is for compressing/zipping
            this.Text = "ZipMagikLITE: Compressing Files...";
            this._encrypt = encrypt;
            this._type = CompressorType.Archive;
            this._file = files;
            this._destinationFolder = destinationFolder;
        }

        public Compressor(string destinationFolder, string zipFileLocationPath) {
            InitializeComponent();
            // This is for extracting
            this.Text = "ZipMagikLITE: Extracting Files...";
            this._encrypt = null;
            this._type = CompressorType.Extract;
            this._file = zipFileLocationPath;
            this._destinationFolder = destinationFolder;
        }

        private void Compressor_Shown(object sender, EventArgs e) {
            switch (_type) {
                case CompressorType.Archive:
                    Archive(sender, e);
                    break;
                case CompressorType.Extract:
                    Extract(sender, e);
                    break;
            }
        }

        private void Archive(object sender, EventArgs e) {
            // Create zipMagik!!
            var magicZip = new ZipMagik();
            magicZip.SaveProgress += Zip_SaveProgress;
            magicZip.ZipError += Zip_Error;
            // Begin the process?
            try {
                string password = null;
                if (_encrypt == true) {
                    // Set a password before we do anything!
                    var window = new Password(((string[])_file).First(), false);
                    DialogResult result = window.ShowDialog();

                    if (result == DialogResult.OK) {
                        password = window.Value;   
                    }
                    else if (result == DialogResult.Cancel) {
                        this.Close();
                    }
                }

                magicZip.ZipFiles(_destinationFolder, password, (string[])_file);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "ZipMagikLITE: Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
        }

        private void Extract(object sender, EventArgs e) {
            // Create zipMagik!!
            var magicZip = new ZipMagik();
            magicZip.ExtractProgress += Zip_ExtractProgress;
            magicZip.ZipError += Zip_Error;
            // Begin the process?
            try {
                // Check for a password?
                string password = null;
                if (magicZip.IsArchiveEncrypted((string)_file)) {
                    var window = new Password((string)_file, false);
                    DialogResult result = window.ShowDialog();

                    if (result == DialogResult.OK) {
                        password = window.Value;
                    }
                    else {
                        this.Close();
                    }
                }

                magicZip.UnZipFiles((string)_file, _destinationFolder, password);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "ZipMagikLITE: Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
        }

        private void Zip_ExtractProgress(object sender, ExtractProgressEventArgs e) {
            if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry) {
                lblMessage.Text = string.Format("Extracting: {0}", e.CurrentEntry.FileName);
            }
            if (e.EventType == ZipProgressEventType.Extracting_AfterExtractEntry && e.TotalBytesToTransfer > 0) {
                int progress = Convert.ToInt32(100 * e.BytesTransferred / e.TotalBytesToTransfer);

                lblProgress.Text = string.Format("{0}%", progress);
                pgb.Value = progress;
            }
            if (e.EventType == ZipProgressEventType.Extracting_AfterExtractAll) {
                this.Close();
            }
        }

        private void Zip_SaveProgress(object sender, SaveProgressEventArgs e) {
            if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry) {
                lblMessage.Text = string.Format("Archiving: {0}", e.CurrentEntry.FileName);
            }
            if (e.EventType == ZipProgressEventType.Saving_AfterWriteEntry) {
                int progress = e.EntriesSaved * 100 / e.EntriesTotal;

                lblProgress.Text = string.Format("{0}%", progress);
                pgb.Value = progress;
            }
            if (e.EventType == ZipProgressEventType.Saving_Completed) {
                this.Close();
            }
        }

        private void Zip_Error(object sender, ZipErrorEventArgs e) {
            MessageBox.Show(e.Exception.Message, "ZipMagikLITE: Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.Close();
        }
    }
}
