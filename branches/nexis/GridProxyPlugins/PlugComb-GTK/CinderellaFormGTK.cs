
using System;
using Gtk; // Form stuff
using Gdk; // Image processing (GIMP toolkit)
using OpenMetaverse;
using OpenMetaverse.Imaging;
using System.IO;
using System.Drawing;

namespace PubComb
{
	public partial class CinderellaFormGTK : Gtk.Window
	{
		private CinderellaPlugin plug;
	    private byte[] UploadData = null;
	    private string FileName = String.Empty;
	
		public CinderellaFormGTK(CinderellaPlugin p) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.plug = p;
			this.Build();
		}
		
		#region Support functions
		private bool IsPowerOfTwo(uint n)
        {
            return (n & (n - 1)) == 0 && n != 0;
        }
		// TODO: Not used.
		/*
        private void LoadOther()
        {
            pbLoadStatus.Fraction = 0;
            cmdSave.Sensitive = false;
            UploadData = System.IO.File.ReadAllBytes(FileName);
            this.selImage.Pixbuf = Pixbuf.LoadFromResource("PubComb.partiny");
            //cmdSave.Sensitive = true;
            pbLoadStatus.Fraction = 1;
			cmdOpen.Sensitive = true;
           	//cmdSave.Sensitive = false;
        }
		*/
		
        private void LoadImage()
        {
            pbLoadStatus.Fraction = 0;
            cmdSave.Sensitive = false;
            if (FileName == null || FileName == "")
                return;

            string lowfilename = FileName.ToLower();
            System.Drawing.Bitmap bitmap = null;

            try
            {
                if (lowfilename.EndsWith(".jp2") || lowfilename.EndsWith(".j2c"))
                {
                    System.Drawing.Image image;
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
                        System.Drawing.Bitmap resized = new System.Drawing.Bitmap(newsx, newsy, bitmap.PixelFormat);
                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(resized);

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

                        System.Drawing.Bitmap resized = new System.Drawing.Bitmap(newwidth, newheight, bitmap.PixelFormat);
                        Graphics graphics = Graphics.FromImage(resized);

                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics.InterpolationMode =
                           System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(bitmap, 0, 0, newwidth, newheight);

                        bitmap.Dispose();
                        bitmap = resized;
                    }

                    Console.Write("Encoding image...");

                    UploadData = OpenJPEG.EncodeFromImage(bitmap,chkLossLess.Active);
                    cmdSave.Sensitive = true;
                    Console.Write("Finished encoding");
                }
            }
            catch (Exception ex)
            {
                UploadData = null;
                cmdSave.Sensitive = false;
                MessageDialog md = new MessageDialog (this, 
			                                      DialogFlags.DestroyWithParent,
			                                      MessageType.Error, 
			                                      ButtonsType.Close,
				                                  ex.Message);
				md.Run();
                return;
            }
            //int test = 16384;
            //Console.WriteLine(BitConverter.ToString(BitConverter.GetBytes(test),0) + " is hte test size");

            //lblSize.Text = Math.Round((double)UploadData.Length / 1024.0d, 2) + "KB";
            //this.1 = UploadData.Length;
            cmdSave.Sensitive = true;
            pbLoadStatus.Fraction = 1;
        
        }
        public void setProgress(double p)
        {
            pbLoadStatus.Fraction = p;
        }
        public void setProgressProps(int max)
        {
            //1 = max;
        }

#endregion

        protected virtual void OnCmdOpenActivated (object sender, System.EventArgs e)
        {
			Gtk.FileChooserDialog dialog = new FileChooserDialog("Choose the file to open",
		                            this,
		                            FileChooserAction.Open,
		                            "Cancel",ResponseType.Cancel,
		                            "Open",ResponseType.Accept);
			
			Gtk.FileFilter filter=new Gtk.FileFilter();
			filter.Name="Image Files";
			filter.AddPattern("*.jp2");
			filter.AddPattern("*.j2c");
			filter.AddPattern("*.jpg");
			filter.AddPattern("*.jpeg");
			filter.AddPattern("*.gif");
			filter.AddPattern("*.png");
			filter.AddPattern("*.bmp");
			filter.AddPattern("*.tga");
			filter.AddPattern("*.tif");
			filter.AddPattern("*.tiff");
			filter.AddPattern("*.ico");
			filter.AddPattern("*.wmf");
			filter.AddPattern("*.emf");
			dialog.AddFilter(filter); 
			
                                
            if (dialog.Run() == (int)ResponseType.Accept)
            {
                FileName = dialog.Filename;
				dialog.Destroy();
				cmdSave.Sensitive=false;
				if(File.Exists(FileName))
				{
					selImage.Pixbuf=new Pixbuf(FileName);
	                LoadImage();
	                cmdSave.Sensitive = true;
				} else {
					Gtk.MessageDialog md = new Gtk.MessageDialog(
					                                             this,
					                                             DialogFlags.DestroyWithParent,
					                                             MessageType.Error,
					                                             ButtonsType.Ok,
					                                             "Unable to find file {0}.",
					                                             FileName
					                                             );
					md.Run();
					FileName="";
				}
            }
        }

        protected virtual void OnCmdSaveActivated (object sender, System.EventArgs e)
        {
			if (UploadData != null)
            {
                //this.progressBar1.Fraction = 0;

                string name = System.IO.Path.GetFileNameWithoutExtension(FileName);
                this.plug.SaveToSLInventory(
				                            UploadData, 
				                            name, 
				                            (sbyte)0, //Type
				                            chkCopy.Active, 
				                            chkModify.Active, 
				                            chkTransfer.Active, 
				                            false, 	// Pay
				                            true, 	// Local
				                            false	// Temporary
				                            );

            }
        }	
	}
}
