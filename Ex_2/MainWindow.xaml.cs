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

namespace Ex_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 1.이미지 불러오기 (BGR 형식으로 불러옴)
            Mat original = Cv2.ImRead("F:\\pepe.jpg");

            // 2.색상 변환 이미지 초기화
            Mat gray = new Mat();
            Mat hsv = new Mat();
            Mat rgb = new Mat();
            Mat lab = new Mat();    
            Mat ycrcb = new Mat();

            // 3.이미지 변환 수행
            Cv2.CvtColor(original, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(original, hsv, ColorConversionCodes.BGR2HSV);
            Cv2.CvtColor(original, rgb, ColorConversionCodes.BGR2RGB);
            Cv2.CvtColor(original, lab, ColorConversionCodes.BGR2Lab);
            Cv2.CvtColor(original, ycrcb, ColorConversionCodes.BGR2YCrCb);

            // 4.변환된 이미지 표시
            Cv2.ImShow("Original", original);
            Cv2.ImShow("GrayScale", gray);
            Cv2.ImShow("HSV", hsv);
            Cv2.ImShow("RGB", rgb);
            Cv2.ImShow("LAB", lab);
            Cv2.ImShow("YCrCb", ycrcb);

            // 5.키 입력 대기 후 종료
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

        }
    }
}