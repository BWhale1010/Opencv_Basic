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

namespace opencv_10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string imgPath = "F:\\dog.jpg";
            Mat src = Cv2.ImRead(imgPath);
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY); // 그레이스케일 변환
            Mat imageCanny = new Mat(), imageThresh = new Mat(), imageAdaptive = new Mat();
            // :앞쪽_화살표: Canny 엣지 검출 적용
            Cv2.Canny(gray, imageCanny, 100, 200);
            // :앞쪽_화살표: 기본 Threshold 적용
            Cv2.Threshold(gray, imageThresh, 127, 255, ThresholdTypes.Binary);
            // :앞쪽_화살표: Adaptive Threshold 적용
            Cv2.AdaptiveThreshold(gray, imageAdaptive, 255,
                        AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 11, 2);
            // :앞쪽_화살표: Contour 검출
            Mat cannyContours = DrawContoursOnImage(src.Clone(), imageCanny, "Canny");
            Mat threshContours = DrawContoursOnImage(src.Clone(), imageThresh, "Threshold");
            Mat adaptiveContours = DrawContoursOnImage(src.Clone(), imageAdaptive, "Adaptive Threshold");
            // :앞쪽_화살표: 5. 결과 출력
            Cv2.ImShow("Original Image", src);
            Cv2.ImShow("Canny Edge Detection", cannyContours);
            Cv2.ImShow("Binary Threshold", threshContours);
            Cv2.ImShow("Adaptive Threshold", adaptiveContours);
            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }
        static Mat DrawContoursOnImage(Mat image, Mat binary, string title)
        {
            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchy;
            // 컨투어 찾기
            Cv2.FindContours(binary, out contours, out hierarchy,
                        RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            // 컨투어 그리기 (초록색)
            Cv2.DrawContours(image, contours, -1, new Scalar(0, 255, 0), 2);
            // 컨투어의 좌표값을 이용하여 가장 왼쪽, 오른쪽, 위쪽, 아래쪽 점 찾기
            foreach (var contour in contours)
            {
                OpenCvSharp.Point extLeft = contour[0], extRight = contour[0],
                                extTop = contour[0], extBottom = contour[0];
                foreach (var pt in contour)
                {
                    if (pt.X < extLeft.X) extLeft = pt;
                    if (pt.X > extRight.X) extRight = pt;
                    if (pt.Y < extTop.Y) extTop = pt;
                    if (pt.Y > extBottom.Y) extBottom = pt;
                }
                // 빨간색 (왼쪽), 초록색 (오른쪽), 파란색 (위쪽), 노란색 (아래쪽) 점 표시
                Cv2.Circle(image, extLeft, 8, new Scalar(0, 0, 255), -1);
                Cv2.Circle(image, extRight, 8, new Scalar(0, 255, 0), -1);
                Cv2.Circle(image, extTop, 8, new Scalar(255, 0, 0), -1);
                Cv2.Circle(image, extBottom, 8, new Scalar(255, 255, 0), -1);
            }
            return image;
        
    }
    }
}