using System.Diagnostics;
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

namespace Ex_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Mat original = Cv2.ImRead("F:\\pepe.jpg");

            // 윈도우 창 생성 및 트랙바  추가
            Cv2.NamedWindow("Original Image");
            Cv2.NamedWindow("Sobel Filter");
            Cv2.NamedWindow("sharr Filter");
            Cv2.NamedWindow("Laplacian Filter");
            Cv2.NamedWindow("Canny Edge");

            // 트랙바 초기값 설정
            int sobelValue = 1;
            int sharrScale = 1;
            int laplacianValue = 1;
            int cannythreshold1 = 50;
            int cannythreshold2 = 150;

            // 트랙바 생성(민감도 조절)
            Cv2.CreateTrackbar("Sobel", "Sobel Filter", ref sobelValue, 5);
            Cv2.CreateTrackbar("sharr", "sharr Filter", ref sharrScale, 5);
            Cv2.CreateTrackbar("Laplacian", "Laplacian Filter", ref laplacianValue, 5);
            Cv2.CreateTrackbar("Canny Threshold1", "Canny Edge", ref cannythreshold1, 255);
            Cv2.CreateTrackbar("Canny Threshold2", "Canny Edge", ref cannythreshold2, 255);

            Mat gray = new Mat();
            Cv2.CvtColor(original, gray, ColorConversionCodes.BGR2GRAY); // 필터를 위해 그레이스케일 변환

            while (true)
            {
                // 필터 적용

                // 소벨 필터 적용
                Mat sobel = new Mat();
                Cv2.Sobel(gray, sobel, MatType.CV_8U, 1, 1, sobelValue * 2 + 1); 

                // shcarr 필터 적용
                Mat scharrX = new Mat();
                Mat scharrY = new Mat();
                Cv2.Scharr(gray, scharrX, MatType.CV_8U, 1, 0, sharrScale); // X 방향 미분
                Cv2.Scharr(gray, scharrY, MatType.CV_8U, 0, 1, sharrScale); // Y 방향 미분

                // Scharr X, Y 결과 결합
                Mat scharr = new Mat();
                Cv2.AddWeighted(scharrX, 0.5, scharrY, 0.5, 0, scharr);

                // 라플라시안 필터 적용
                Mat laplacian = new Mat();
                Cv2.Laplacian(gray, laplacian, MatType.CV_8U, laplacianValue * 2 + 1);

                // 캐니 엣지 검출
                Mat canny = new Mat();
                Cv2.Canny(gray, canny, cannythreshold1, cannythreshold2);

                // 결과 이미지 표시
                Cv2.ImShow("Original Image", original);
                Cv2.ImShow("Sobel Derivate", sobel);
                Cv2.ImShow("Scharr Filter", scharr);
                Cv2.ImShow("Laplacian", laplacian);
                Cv2.ImShow("Canny Edge", canny);

                if (Cv2.WaitKey(30) == 'q')
                {
                    break;
                }
            }

            Cv2.DestroyAllWindows();
        }
    }
}