using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using OpenMetaverse.Imaging;
using OpenMetaverse;
using OpenMetaverse.Packets;

namespace PubComb
{
    
    public partial class CinderForm1 : Form
    {
        private CinderellaPlugin gaylord;
        private byte[] UploadData = null;
        private string FileName = String.Empty;
        public CinderForm1(CinderellaPlugin free)
        {
            this.gaylord = free;
            InitializeComponent();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }
        private bool IsPowerOfTwo(uint n)
        {
            return (n & (n - 1)) == 0 && n != 0;
        }
        private void LoadOther()
        {
            progressBar1.Value = 0;
            button2free.Enabled = false;
            UploadData = System.IO.File.ReadAllBytes(FileName);
            Bitmap bitmap = null;
            this.pictureBox1.Image = bitmap;
            button2free.Enabled = true;
            progressBar1.Value = progressBar1.Maximum;
        }
        private void LoadImage()
        {
            progressBar1.Value = 0;
            button2free.Enabled = false;
            if (FileName == null || FileName == "")
                return;

            string lowfilename = FileName.ToLower();
            Bitmap bitmap = null;

            try
            {
                if (lowfilename.EndsWith(".jp2") || lowfilename.EndsWith(".j2c"))
                {
                    Image image;
                    ManagedImage managedImage;

                    // Upload JPEG2000 images untouched
                    UploadData = System.IO.File.ReadAllBytes(FileName);

                    OpenJPEG.DecodeToImage(UploadData, out managedImage, out image);
                    bitmap = (Bitmap)image;

                    Console.Write("Loaded raw JPEG2000 data " + FileName);
                }
                else
                {
                    if (lowfilename.EndsWith(".tga"))
                        bitmap = LoadTGAClass.LoadTGA(FileName);
                    else
                        bitmap = (Bitmap)System.Drawing.Image.FromFile(FileName);

                    Console.Write("Loaded image " + FileName);

                    int oldwidth = bitmap.Width;
                    int oldheight = bitmap.Height;

                    if (!IsPowerOfTwo((uint)oldwidth) || !IsPowerOfTwo((uint)oldheight))
                    {
                        //Console.Write("Image has irregular dimensions " + oldwidth + "x" + oldheight + ", resizing to 256x256");
                        //int biggestD = (int)Math.Max((decimal)oldwidth, (decimal)oldheight);
                        int newsx = 2;
                        while (newsx < oldwidth && newsx <= 1024)
                        {
                            newsx = newsx * 2;
                        }
                        int newsy = 2;
                        while (newsy < oldheight && newsy <= 1024)
                        {
                            newsy = newsy * 2;
                        }


                        Console.Write("Image has irregular dimensions " + oldwidth + "x" + oldheight + ", resizing to " + newsx + "x" + newsy);
                        Bitmap resized = new Bitmap(newsx, newsy, bitmap.PixelFormat);
                        Graphics graphics = Graphics.FromImage(resized);

                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics.InterpolationMode =
                           System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(bitmap, 0, 0, newsx, newsy);

                        bitmap.Dispose();
                        bitmap = resized;

                        oldwidth = newsx;
                        oldheight = newsy;
                    }

                    // Handle resizing to prevent excessively large images
                    if (oldwidth > 1024 || oldheight > 1024)
                    {
                        int newwidth = (oldwidth > 1024) ? 1024 : oldwidth;
                        int newheight = (oldheight > 1024) ? 1024 : oldheight;

                        Console.Write("Image has oversized dimensions " + oldwidth + "x" + oldheight + ", resizing to " +
                            newwidth + "x" + newheight);

                        Bitmap resized = new Bitmap(newwidth, newheight, bitmap.PixelFormat);
                        Graphics graphics = Graphics.FromImage(resized);

                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics.InterpolationMode =
                           System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(bitmap, 0, 0, newwidth, newheight);

                        bitmap.Dispose();
                        bitmap = resized;
                    }

                    Console.Write("Encoding image...");

                    UploadData = OpenJPEG.EncodeFromImage(bitmap,false);
                    button2free.Enabled = true;
                    Console.Write("Finished encoding");
                }
            }
            catch (Exception ex)
            {
                UploadData = null;
                button2free.Enabled = false;
                MessageBox.Show(ex.ToString(), "Gay Lord", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //int test = 16384;
            //Console.WriteLine(BitConverter.ToString(BitConverter.GetBytes(test),0) + " is hte test size");

            this.pictureBox1.Image = bitmap;
            //lblSize.Text = Math.Round((double)UploadData.Length / 1024.0d, 2) + "KB";
            //this.progressBar1.Maximum = UploadData.Length;
            button2free.Enabled = true;
            progressBar1.Value = progressBar1.Maximum;
        
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            buttonLoad.Enabled = true;
           button2free.Enabled = false;
           progressBar1.Maximum = 17;
        }
        public void setProgress(int p)
        {
            progressBar1.Value = p;
        }
        public void setProgressProps(int max)
        {
            progressBar1.Maximum = max;
        }
        private void buttonLoad_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Filter =
                "Image Files (*.jp2,*.j2c,*.jpg,*.jpeg,*.gif,*.png,*.bmp,*.tga,*.tif,*.tiff,*.ico,*.wmf,*.emf)|" +
                "*.jp2;*.j2c;*.jpg;*.jpeg;*.gif;*.png;*.bmp;*.tga;*.tif;*.tiff;*.ico;*.wmf;*.emf;";
                                
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileName = dialog.FileName;
                LoadImage();
                button2free.Enabled = true;
            }
            
        }


        private void button2free_Click(object sender, EventArgs e)
        {
            if (UploadData != null)
            {
                //this.progressBar1.Value = 0;

                string name = System.IO.Path.GetFileNameWithoutExtension(FileName);
                this.gaylord.SaveToSLInventory(UploadData, name, (sbyte)0, checkBox1copy.Checked, checkBox1mod.Checked, checkBox1trans.Checked, false, true, false);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "This Program was made by LordGregGreg under the help and guidance of \"Philip Linden\" (not his real name or sl name)\n" +
                "\n" +
                "This program allows you to upload textures for free to second life for testing purposes.\n" +
                "The uploads are NOT uploaded to the asset server, but rather to the sim's hard drive\n" +
                "These textures will only load on the sim you are on (unless they are already in your cache)\n" +
                "These uploads are free because they work the exact same way your client normally uploads backed textures\n" +
                "(Obviously you are not charged every time you enter a region and upload your baked skin texture)\n" +
                "\n" +
                "You are free to use and modify this code at your own risk, just please leave this message and acces to it intact :)"+
                "\nPlease see http://www.ispartaaa.com/super1337hax/pluginPack.html for more information", "Cinderella - About", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
