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
                using (var document = PdfDocument.Load(pdfPath))
                {
                    var page = document.Render(0, 300, 300, true); // Render the first page at 300 DPI
                    pdfPictureBox.Image = page;
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
                    PdfDocument document = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Modify);
                    PdfPage page = document.Pages[0];
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    // Save the signature image to a temporary file and then load it back as XImage
                    string tempPath = Path.GetTempFileName();
                    signaturePictureBox.Image.Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
                    XImage xImage = XImage.FromFile(tempPath);

                    // Specify the position where the signature should be placed
                    int xPosition = 100; // Example x position
                    int yPosition = 100; // Example y position
                    gfx.DrawImage(xImage, xPosition, yPosition, xImage.PixelWidth, xImage.PixelHeight);

                    document.Save(savePath);
                    MessageBox.Show("PDF saved successfully.");

                    // Clean up temporary file
                    File.Delete(tempPath);
                }
            }
        }
    }
}