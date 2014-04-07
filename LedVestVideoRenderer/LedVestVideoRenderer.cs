using System;
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

        private void button1_Click(object sender, EventArgs e)
        {
            var fileName = GetFileName("AVI (*.avi)|*.avi|MPEG (*.mpeg)|*.mpeg");
            if (fileName != null) {
                txtAviFileName.Text = fileName;
                //ShowFrame();
            }
        }

        private String GetFileName(String filter)
        {
            
            var dlg = new OpenFileDialog();
            dlg.Filter = filter;
            dlg.RestoreDirectory = true;
            if (txtAviFileName.Text.Length > 0) 
            {
                dlg.InitialDirectory = GetCurrentFilePath();
            }
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                return dlg.FileName;
            }
            else
            {
                return null;
            }
        }

        private String MakeFileName(String filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = filter;
            dlg.RestoreDirectory = true;
            if (txtAviFileName.Text.Length > 0)
            {
                dlg.InitialDirectory = GetCurrentFilePath();
            }
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                return dlg.FileName;
            }
            else
            {
                return null;
            }
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
                MessageBox.Show("Please choose a filename to save the data.");
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

                MessageBox.Show("Render Completed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("unable to open the file \"{0}\", DirectShow reported the following error: {1}", txtAviFileName.Text, ex.Message));
            }
        }




    }
}
