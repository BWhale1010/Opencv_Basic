using System;
using System.Windows;
using OpenCvSharp;

namespace opencv_4
{
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string imgPath = @"F:\Lenna.png";
            Mat image = Cv2.ImRead(imgPath);

            // 1. 원본 이미지를 그레이스케일로 변환
            Mat gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

            // 2. 히스토그램 평활화 (Equalization)
            Mat equalized = new Mat();
            Cv2.EqualizeHist(gray, equalized);

            // 3. 원본 그레이스케일 이미지의 히스토그램 계산
            Mat grayHist = new Mat();
            int[] histSize = { 256 };
            Rangef[] ranges = { new Rangef(0, 256) };
            Cv2.CalcHist(new Mat[] { gray }, new int[] { 0 }, null, grayHist, 1, histSize, ranges);

            // 4. Equalized 이미지의 히스토그램 계산
            Mat equalizedHist = new Mat();
            Cv2.CalcHist(new Mat[] { equalized }, new int[] { 0 }, null, equalizedHist, 1, histSize, ranges);

            // 5. 히스토그램 시각화
            Mat histImageGray = DrawHistogram(grayHist, new Scalar(255, 255, 255));
            Mat histImageEqualized = DrawHistogram(equalizedHist, new Scalar(255, 255, 255));

            // 6. 이미지 및 히스토그램 출력
            Cv2.ImShow("Original Grayscale Image", gray);
            Cv2.ImShow("Equalized Image", equalized);
            Cv2.ImShow("Original Grayscale Histogram", histImageGray);
            Cv2.ImShow("Equalized Histogram", histImageEqualized);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        static Mat DrawHistogram(Mat hist, Scalar color)
        {
            int histWidth = 512, histHeight = 400;
            Mat histImage = new Mat(histHeight, histWidth, MatType.CV_8UC3, Scalar.Black);

            // 히스토그램 정규화 (0 ~ histHeight 범위)
            Cv2.Normalize(hist, hist, 0, histImage.Rows, NormTypes.MinMax);

            int binWidth = histWidth / hist.Rows;

            // 히스토그램 그리기
            for (int i = 1; i < hist.Rows; i++)
            {
                int x1 = (i - 1) * binWidth;
                int y1 = histHeight - (int)Math.Round(hist.At<float>(i - 1));
                int x2 = i * binWidth;
                int y2 = histHeight - (int)Math.Round(hist.At<float>(i));

                Cv2.Line(histImage, new OpenCvSharp.Point(x1, y1), new OpenCvSharp.Point(x2, y2), color, 2);
            }

            return histImage;
        }
    }
}
