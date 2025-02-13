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

namespace opencv_9
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
            Mat image = Cv2.ImRead(imgPath);
            // :앞쪽_화살표: 그레이스케일 변환 (CV_8UC1 이미지만 허용)
            Mat gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
            // :앞쪽_화살표: 이미지 이진화 (Thresholding)
            Mat binary = new Mat();
            Cv2.Threshold(gray, binary, 100, 255, ThresholdTypes.Binary);
            // :앞쪽_화살표: 윤곽선 검출 (CV_8UC1 이미지 사용)
            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binary, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            // :앞쪽_화살표: 윤곽선 그리기 (컬러 이미지 위에 그리기)
            Mat contourImage = image.Clone();
            Cv2.DrawContours(contourImage, contours, -1, new Scalar(0, 0, 255), 2);
            // :앞쪽_화살표: 결과 출력
            Cv2.ImShow("Original Image", image);
            Cv2.ImShow("Binary Image", binary);
            Cv2.ImShow("Contour Detection", contourImage);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}