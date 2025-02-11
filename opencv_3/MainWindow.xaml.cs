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

namespace opencv_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string imgPath = "F:\\Lenna.png";
            Mat image = Cv2.ImRead(imgPath);
            // 1. 그레이스케일 변환 후 히스토그램 계산
            Mat gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
            Mat grayHist = new Mat();
            int[] histSize = { 256 }; // 히스토그램 크기 (256단계)
            Rangef[] ranges = { new Rangef(0, 256) }; // 픽셀 값 범위 (0~255)
            Cv2.CalcHist(new Mat[] { gray }, new int[] { 0 }, null, grayHist, 1, histSize, ranges);
            // 2. 히스토그램 시각화 (그레이스케일)
            Mat histImage = DrawHistogram(grayHist, new Scalar(255, 255, 255));
            // 3. 결과 이미지 및 히스토그램 출력
            Cv2.ImShow("Original Image", image);
            Cv2.ImShow("Grayscale Histogram", histImage);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
        static Mat DrawHistogram(Mat hist, Scalar color)
        {
            int histWidth = 512, histHeight = 400;
            Mat histImage = new Mat(histHeight, histWidth, MatType.CV_8UC3, Scalar.Black);
            // 히스토그램 정규화 (0~histHeight 범위)
            Cv2.Normalize(hist, hist, 0, histImage.Rows, NormTypes.MinMax);
            // 히스토그램 그리기
            int binWidth = histWidth / hist.Rows;
            for (int i = 1; i < hist.Rows; i++)
            {
                int x1 = (i - 1) * binWidth;
                int y1 = histHeight - (int)hist.At<float>(i - 1);
                int x2 = i * binWidth;
                int y2 = histHeight - (int)hist.At<float>(i);
                Cv2.Line(histImage, new OpenCvSharp.Point(x1, y1), new OpenCvSharp.Point(x2, y2), color, 2);
            }
            return histImage;
        }
    }
}