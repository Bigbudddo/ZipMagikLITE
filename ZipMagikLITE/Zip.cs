using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

using ZipMagikLITE.Views;

namespace ZipMagikLITE {

    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    public class Zip : SharpContextMenu {

        private ContextMenuStrip _menu = new ContextMenuStrip();
        private string[] _invalidExtensions = new string[] {
            "zip", "ZIP"
        };

        protected override bool CanShowMenu() {
            if (HasZipFile()) {
                return false;
            }
            else {
                this.UpdateMenu();
                return true;
            }
        }

        protected override ContextMenuStrip CreateMenu() {
            _menu.Items.Clear();

            var mainMenu = new ToolStripMenuItem {
                Text = "ZipMagik",
                Image = Properties.Resources.app_icon_small
            };

            var basicZip = new ToolStripMenuItem {
                Text = "Zip File(s)",
                Image = Properties.Resources.app_icon_small,
                ToolTipText = "Compress your files into a basic zip archive."
            };

            var advanZip = new ToolStripMenuItem {
                Text = "Zip File(s) w/Password",
                Image = Properties.Resources.app_icon_small,
                ToolTipText = "Compress and password protect your files."
            };

            var help = new ToolStripMenuItem {
                Text = "Help",
                Image = Properties.Resources.icon_question
            };

            var donate = new ToolStripMenuItem {
                Text = "Donate",
                Image = Properties.Resources.icon_paypal
            };

            basicZip.Click += (sender, args) => ZipFile();
            advanZip.Click += (sender, args) => ZipPasswordFile();
            help.Click += (sender, args) => OpenHelpPage();
            donate.Click += (sender, args) => OpenDonate();

            mainMenu.DropDownItems.Add(basicZip);
            mainMenu.DropDownItems.Add(advanZip);
            mainMenu.DropDownItems.Add(new ToolStripSeparator());
            mainMenu.DropDownItems.Add(help);
            mainMenu.DropDownItems.Add(donate);
            _menu.Items.Add(mainMenu);

            return _menu;
        }

        private bool HasZipFile() {
            foreach (var item in SelectedItemPaths) {
                string[] aItem = item.Split('.');
                string itemExt = aItem.Last();

                if (_invalidExtensions.Contains(itemExt)) {
                    return true;
                }
            }
            return false;
        }

        private string GetZipFileNamePath(string firstItem) {
            string[] aFilePath = firstItem.Split('.');
            string destination = string.Join(".", aFilePath.Take(aFilePath.Length - 1));
            destination += ".zip";
            return destination;
        }

        private void UpdateMenu() {
            _menu.Dispose();
            _menu = CreateMenu();
        }

        private void ZipFile() {
            ZipFile(false);
        }

        private void ZipPasswordFile() {
            ZipFile(true);
        }

        private void OpenHelpPage() {
            System.Diagnostics.Process.Start("https://github.com/Bigbudddo/ZipMagikLITE/wiki");
        }

        private void OpenDonate() {
            string url = string.Format(
                "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business={0}&lc={1}&item_name={2}&currency_code={3}&bn=PP%2dDonationsBF",
                "email@stuart-harrison.com", "UK", "ZipMagikLITE, Stuart Harrison", "GBP"
            );
            System.Diagnostics.Process.Start(url);
        }

        private void ZipFile(bool encrypt) {
            try {
                if (SelectedItemPaths.Count() == 0) {
                    return;
                }

                string destination = this.GetZipFileNamePath(SelectedItemPaths.First());
                var window = new Compressor(destination, encrypt, SelectedItemPaths.ToArray());
                window.Show();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "ZipMagikLITE: Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }

    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".zip")]
    public class UnZip : SharpContextMenu {

        private ContextMenuStrip _menu = new ContextMenuStrip();

        protected override bool CanShowMenu() {
            if (SelectedItemPaths.Count() == 1) {
                this.UpdateMenu();
                return true;
            }
            else {
                return false;
            }
        }

        protected override ContextMenuStrip CreateMenu() {
            var menu = new ContextMenuStrip();

            var unZipFileItem = new ToolStripMenuItem {
                Text = "Extract File(s)",
                Image = Properties.Resources.app_icon_small,
                ToolTipText = "Extract all the files within this archive"
            };
            unZipFileItem.Click += (sender, args) => UnZipFiles();
            menu.Items.Add(unZipFileItem);

            return menu;
        }

        private void UpdateMenu() {
            _menu.Dispose();
            _menu = CreateMenu();
        }

        private void UnZipFiles() {
            try {
                if (SelectedItemPaths.Count() == 0) {
                    return;
                }

                string[] aFilePath = SelectedItemPaths.First().Split('.');
                string destinationFolder = string.Join(".", aFilePath.Take(aFilePath.Length - 1));

                var window = new Compressor(destinationFolder, SelectedItemPaths.First());
                window.Show();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "ZipMagikLITE: Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
