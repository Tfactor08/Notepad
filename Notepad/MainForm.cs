using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Notepad
{
    public partial class MainForm : Form
    {
        string path = string.Empty;
        string fileName = "Нет файлов";

        public MainForm()
        {
            InitializeComponent();
            downloadUserSettings();

            var cm = new ContextMenu();
            cm.MenuItems.Add("Копировать", new EventHandler(buttonCopy_Click));
            cm.MenuItems.Add("Вырезать", new EventHandler(buttonCut_Click));
            cm.MenuItems.Add("Вставить", new EventHandler(buttonPaste_Click));

            textBox.ContextMenu = cm;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                textBox.LoadFile(fileName);
                
                this.fileName = fileName.Split('\\').Last();

                labelStatus.Text = this.fileName;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // File.WriteAllText(path = saveFileDialog.FileName, textBox.Text);
                textBox.SaveFile(saveFileDialog.FileName);
                saveUserSettings();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                File.WriteAllText(path, textBox.Text);
                saveUserSettings();
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
                saveUserSettings();
            }
        }

        private void exitPrompt()
        {
            DialogResult = MessageBox.Show("Сохранить текущий файл?",
                "Notepad",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                exitPrompt();

                if (DialogResult == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                    textBox.Text = string.Empty;
                    path = string.Empty;
                }
                else if (DialogResult == DialogResult.No)
                {
                    textBox.Text = string.Empty;
                    path = string.Empty;
                }
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
            => textBox.SelectAll();

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
            => textBox.Cut();

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
            => textBox.Copy();

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
            => textBox.Paste();

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
            => textBox.SelectedText = string.Empty;

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wordWrapToolStripMenuItem.Checked == true)
            {
                textBox.WordWrap = false;
                textBox.ScrollBars = RichTextBoxScrollBars.Both;
                wordWrapToolStripMenuItem.Checked = false;
            }
            else
            {
                textBox.WordWrap = true;
                textBox.ScrollBars = RichTextBoxScrollBars.Vertical;
                wordWrapToolStripMenuItem.Checked = true;
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                textBox.Font = new Font(fontDialog.Font, fontDialog.Font.Style);
                textBox.ForeColor = fontDialog.Color;
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Выполнил студент группы 2231117 Гарипов Булат");
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                exitPrompt();

                if (DialogResult == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
                else if (DialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        e.SuppressKeyPress = true;
                        textBox.SelectAll();
                        break;
                    case Keys.N:
                        e.SuppressKeyPress = true;
                        newToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.O:
                        e.SuppressKeyPress = true;
                        openToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.S:
                        e.SuppressKeyPress = true;
                        saveToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.P:
                        e.SuppressKeyPress = true;
                        printToolStripMenuItem_Click(sender, e);
                        break;
                }
            }
        }

        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.ForeColor = Color.White;
            textBox.BackColor = Color.Black;
            labelStatus.ForeColor = Color.White;
            this.BackColor = Color.Gray;
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.ForeColor = Color.Blue;
            textBox.BackColor = Color.Gray;
            labelStatus.ForeColor = Color.Black;
            this.BackColor = Color.White;
        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.ForeColor = Color.Black;
            textBox.BackColor = Color.White;
            labelStatus.ForeColor = Color.Black;
            this.BackColor = Color.White;
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void saveUserSettings()
        {
            var foreColor = textBox.ForeColor;
            var backColor = textBox.BackColor;

            Properties.Settings.Default.ForeColor = foreColor;
            Properties.Settings.Default.BackColor = backColor;

            Properties.Settings.Default.Save();
        }

        private void downloadUserSettings()
        {
            var foreColor = Properties.Settings.Default.ForeColor;
            var backColor = Properties.Settings.Default.BackColor;

            textBox.ForeColor = foreColor;
            textBox.BackColor = backColor;
        }

        private void buttonPaste_Click(object sender, EventArgs e)
        {
            textBox.Paste();
        }

        private void buttonCut_Click(object sender, EventArgs e)
        {
            textBox.Cut();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            textBox.Copy();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            int charCount = 0;
            foreach (var c in textBox.Text)
                if (c != ' ') charCount++;

            labelStatus.Text = $"{fileName} \t{charCount} симв.";

        }
    }
}
