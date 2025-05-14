using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Resources.Extensions;
using System.Windows;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.QrCode;

namespace WpfQrCodeDecoder
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*",
                Title = "Select QR Code Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Display the image
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(openFileDialog.FileName);
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    imgQRCode.Source = bitmapImage;

                    // Decode the QR code
                    string decodedText = DecodeQRCode(openFileDialog.FileName);

                    if (string.IsNullOrEmpty(decodedText))
                    {
                        txtResult.Text = "No QR code found or could not be decoded.";
                    }
                    else
                    {
                        txtResult.Text = decodedText;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string DecodeQRCode(string imagePath)
        {
            // Load the image
            using (Bitmap bitmap = new Bitmap(imagePath))
            {
                // Create a barcode reader instance
                var reader = new ZXing.Windows.Compatibility.BarcodeReader();
                var result = reader.Decode(bitmap);

                // Return the decoded text (or null if not found)
                return result?.Text ?? string.Empty;
            }
        }
    }
}