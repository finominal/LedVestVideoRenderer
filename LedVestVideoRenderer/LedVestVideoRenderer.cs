﻿using System;
using System.Windows.Forms;
using LedVestVideoRenderer.Domain;

namespace LedVestVideoRenderer
{
    public partial class LedVestVideoRenderer : Form
    {
        public LedVestVideoRenderer()
        {
            InitializeComponent();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            var fileName = GetFileName("AVI (*.avi)|*.avi|MPEG (*.mpeg)|*.mpeg");
            if (fileName != null) {
                txtAviFileName.Text = fileName;
            }
        }

        private String GetFileName(String filter)
        {

            var dlg = new OpenFileDialog {Filter = filter, RestoreDirectory = true};
            if (txtAviFileName.Text.Length > 0) 
            {
                dlg.InitialDirectory = GetCurrentFilePath();
            }

            return dlg.ShowDialog(this) == DialogResult.OK ? dlg.FileName : null;
        }

        private String MakeFileName(String filter)
        {
            var dlg = new SaveFileDialog {Filter = filter, RestoreDirectory = true};

            if (txtAviFileName.Text.Length > 0)
            {
                dlg.InitialDirectory = GetCurrentFilePath();
            }

            return dlg.ShowDialog(this) == DialogResult.OK ? dlg.FileName : null;
        }

        private String GetCurrentFilePath()
        {
            return txtAviFileName.Text.Substring(0, txtAviFileName.Text.LastIndexOf("\\") + 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var fileName = MakeFileName("LedFile (*.led)|*.led");
            if (fileName != null)
            {
                textSaveAs.Text = fileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(txtAviFileName.Text.Length == 0)
            {
                MessageBox.Show("Please choose a movie file to render.");
                return;
            }

            if (textSaveAs.Text.Length == 0)
            {
                MessageBox.Show("Please choose a filename to save the data.");
                return;
            }

            /*
            if (!Validation.IsValidBrightness(textMaxBrightness.Text))
            {
                MessageBox.Show("Please choose a value between 0 and 255.");
                return;
            }
             */ 
            try
            {
                var render = new RenderManager();

                render.RenderVideoToFile(
                                        txtAviFileName.Text, 
                                        textSaveAs.Text, 
                                        int.Parse(textMaxBrightness.Text),
                                        chbxSmoothen.Checked, 
                                        chbxDuplicate.Checked,
                                        chbxTwoFrames.Checked
                                        );

                MessageBox.Show("Render Complete!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("unable to process the file \"{0}\",  The following error occured: {1}", txtAviFileName.Text, ex.Message));
            }
        }
    }
}
