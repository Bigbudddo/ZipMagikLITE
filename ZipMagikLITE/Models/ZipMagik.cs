using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ionic.Zip;
using Ionic.Zlib;

namespace ZipMagikLITE.Models {

    public sealed class ZipMagik {

        private CompressionLevel _compression;
        private ExtractExistingFileAction _existingFileAction;

        public CompressionLevel Compression {
            get {
                return _compression;
            }
            set {
                _compression = value;
            }
        }

        public ExtractExistingFileAction ExistingFileAction {
            get {
                return _existingFileAction;
            }
            set {
                _existingFileAction = value;
            }
        }

        public event EventHandler<AddProgressEventArgs> AddProgress;
        public event EventHandler<ExtractProgressEventArgs> ExtractProgress;
        public event EventHandler<ReadProgressEventArgs> ReadProgress;
        public event EventHandler<SaveProgressEventArgs> SaveProgress;
        public event EventHandler<ZipErrorEventArgs> ZipError;


        public ZipMagik() {
            Compression = CompressionLevel.BestCompression;
            ExistingFileAction = ExtractExistingFileAction.OverwriteSilently;
        }

        public ZipMagik(CompressionLevel compression) {
            Compression = compression;
            ExistingFileAction = ExtractExistingFileAction.OverwriteSilently;
        }

        public ZipMagik(CompressionLevel compression, ExtractExistingFileAction fileAction) {
            Compression = compression;
            ExistingFileAction = fileAction;
        }


        public bool IsArchiveEncrypted(string targetZip) {
            using (ZipFile zip = ZipFile.Read(targetZip)) {
                zip.CompressionLevel = Compression;
                ZipEntry firstEntry = zip.First();

                return firstEntry.UsesEncryption;
            }
        }


        public bool ZipFiles(string destinationZipPath, params string[] files) {
            return ZipFiles(destinationZipPath, null, files);
        }

        public bool ZipFiles(string destinationZipPath, string zipPassword, params string[] files) {
            using (ZipFile zip = new ZipFile()) {
                zip.CompressionLevel = Compression;

                if (SaveProgress != null) {
                    zip.SaveProgress += this.SaveProgress;
                }
                if (ZipError != null) {
                    zip.ZipError += this.ZipError;
                }

                if (!String.IsNullOrWhiteSpace(zipPassword)) {
                    zip.Password = zipPassword;
                }

                foreach (var file in files) {
                    if (IsDirectory(file)) {
                        zip.AddDirectory(file, GetDirectoryFileName(file));
                    }
                    else {
                        zip.AddFile(file, "");
                    }
                }

                zip.Save(destinationZipPath);
                return true;
            }
        }


        public bool UnZipFiles(string targetZip, string destinationFolder) {
            return UnZipFiles(targetZip, destinationFolder, null);
        }

        public bool UnZipFiles(string targetZip, string destinationFolder, string zipPassword) {
            using (ZipFile zip = ZipFile.Read(targetZip)) {
                zip.CompressionLevel = Compression;

                if (ExtractProgress != null) {
                    zip.ExtractProgress += this.ExtractProgress;
                }

                if (ZipError != null) {
                    zip.ZipError += this.ZipError;
                }

                if (!String.IsNullOrWhiteSpace(zipPassword)) {
                    zip.Password = zipPassword;
                }

                zip.ExtractAll(destinationFolder, ExistingFileAction);
                return true;
            }
        }


        private bool IsDirectory(string path) {
            FileAttributes fAtt = File.GetAttributes(path);
            return (fAtt.HasFlag(FileAttributes.Directory));
        }

        private string GetDirectoryFileName(string path) {
            string[] aPath = path.Split('\\');
            return aPath.Last();
        }
    }
}
