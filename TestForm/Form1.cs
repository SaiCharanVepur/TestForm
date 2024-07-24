using PdfiumViewer;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using wgssSTU;
using TabletLib;
using GraphicsLib; 
using System.Collections.Generic;
using DemoButtons;



enum PenDataOptionMode
{
    PenDataOptionMode_None,
    PenDataOptionMode_TimeCount,
    PenDataOptionMode_SequenceNumber,
    PenDataOptionMode_TimeCountSequence
};




namespace TestForm
{
    public partial class Form1 : Form
    {
        private string pdfPath;
        private wgssSTU.Tablet m_tablet;
        private wgssSTU.ICapability m_capability;
        private wgssSTU.IInformation m_information;
        private wgssSTU.IProtocolHelper m_protocolHelper;
        private Bitmap m_bitmap;


        public Form1()
        {
            InitializeComponent();
            m_protocolHelper = new wgssSTU.ProtocolHelper();
        }

        private void btnLoadPDF_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pdfPath = openFileDialog.FileName;

                // Render PDF to Bitmap using PdfiumViewer
                using (var pdfDocument = PdfiumViewer.PdfDocument.Load(pdfPath))
                {
                    var pageSize = pdfDocument.PageSizes[0];
                    m_bitmap = new Bitmap((int)pageSize.Width, (int)pageSize.Height);

                    using (var bitmapGraphics = Graphics.FromImage(m_bitmap))
                    {
                        bitmapGraphics.Clear(Color.White);
                        using (var pageBitmap = pdfDocument.Render(0, (int)pageSize.Width, (int)pageSize.Height, true))
                        {
                            bitmapGraphics.DrawImage(pageBitmap, 0, 0, pageBitmap.Width, pageBitmap.Height);
                        }
                    }

                    pdfPictureBox.Image = m_bitmap;
                }
            }

        }


        private void btnCaptureSignature_Click(object sender, EventArgs e)
        {
            int penDataType;
            List<IPenDataTimeCountSequence> penTimeData = null;
            List<IPenData> penData = null;

            UsbDevices usbDevices = new UsbDevices();
            if (usbDevices.Count != 0)
            {
                try
                {
                    IUsbDevice usbDevice = usbDevices[0]; // select a device

                    SignatureForm demo = new SignatureForm(usbDevice, false); // Assuming no encryption for simplicity
                    demo.ShowDialog();
                    penDataType = demo.penDataType;

                    if (penDataType == (int)PenDataOptionMode.PenDataOptionMode_TimeCountSequence)
                        penTimeData = demo.getPenTimeData();
                    else
                        penData = demo.getPenData();

                    if (penData != null || penTimeData != null)
                    {
                        m_capability = demo.getCapability();
                        m_information = demo.getInformation();

                        if (penData != null)
                        {
                            DisplaySignature(penData);
                        }
                        else
                        {
                            DisplaySignature(penTimeData);
                        }
                    }
                    demo.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No STU devices attached");
            }
        }

        // Method to display the signature using your graphics library
        private void DisplaySignature(List<IPenData> penData)
        {
            m_bitmap = new Bitmap(m_capability.screenWidth, m_capability.screenHeight);
            using (Graphics gfx = Graphics.FromImage(m_bitmap))
            {
                gfx.Clear(Color.White);
                foreach (var data in penData)
                {
                    if (data.sw > 0)
                    {
                        Point pt = new Point(data.x, data.y);
                        gfx.DrawLine(Pens.Black, pt, pt); // Use appropriate logic to draw lines
                    }
                }
            }
            signaturePictureBox.Image = m_bitmap;
        }

        // Overloaded method for IPenDataTimeCountSequence
        private void DisplaySignature(List<IPenDataTimeCountSequence> penTimeData)
        {
            m_bitmap = new Bitmap(m_capability.screenWidth, m_capability.screenHeight);
            using (Graphics gfx = Graphics.FromImage(m_bitmap))
            {
                gfx.Clear(Color.White);
                foreach (var data in penTimeData)
                {
                    if (data.sw > 0)
                    {
                        Point pt = new Point(data.x, data.y);
                        gfx.DrawLine(Pens.Black, pt, pt); // Use appropriate logic to draw lines
                    }
                }
            }
            signaturePictureBox.Image = m_bitmap;
        }


        private void btnSavePDF_Click(object sender, EventArgs e)
        {
            if (signaturePictureBox.Image != null && pdfPictureBox.Image != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog.FileName;

                    // Save the signature image to a temporary file and then load it back as an image
                    string tempPath = Path.GetTempFileName();
                    signaturePictureBox.Image.Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);

                    // Use PdfiumViewer to load and modify the PDF
                    using (var pdfDocument = PdfiumViewer.PdfDocument.Load(pdfPath))
                    {
                        var pageSize = pdfDocument.PageSizes[0];
                        using (var bitmap = new Bitmap((int)pageSize.Width, (int)pageSize.Height))
                        {
                            using (var graphics = Graphics.FromImage(bitmap))
                            {
                                graphics.Clear(Color.White);
                                using (var pageBitmap = pdfDocument.Render(0, (int)pageSize.Width, (int)pageSize.Height, true))
                                {
                                    graphics.DrawImage(pageBitmap, 0, 0, pageBitmap.Width, pageBitmap.Height);

                                    // Draw the signature on the rendered PDF page
                                    using (var signature = new Bitmap(tempPath))
                                    {
                                        graphics.DrawImage(signature, new System.Drawing.Rectangle(100, 100, signature.Width, signature.Height));
                                    }
                                }
                            }

                            // Save the modified page as a new PDF
                            using (var stream = new FileStream(savePath, FileMode.Create))
                            {
                                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                    }
                    MessageBox.Show("PDF saved successfully.");

                    // Clean up temporary file
                    File.Delete(tempPath);
                }

            }
        }
    }
}