using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenCvSharp;


namespace Opencv_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string imgPath = "F:\\cat.jpg";
            Mat image = Cv2.ImRead(imgPath);

            Mat target0 = new Mat();
            Cv2.CvtColor(image, target0, ColorConversionCodes.BGRA2BGR);

            string text = "Hello, OpenCV!";
            OpenCvSharp.Point textPosition = new OpenCvSharp.Point(50, 100);
            Cv2.PutText(target0, text, textPosition, HersheyFonts.HersheyScriptSimplex, 3, new Scalar(0, 255, 0), 4, LineTypes.AntiAlias);

            imageBox.Source = OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(target0);


        }
    }
}