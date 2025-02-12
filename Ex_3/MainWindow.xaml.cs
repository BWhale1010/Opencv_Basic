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

namespace Ex_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Mat = Matrix = 2차원 행렬
            Mat src = Cv2.ImRead("F:\\pepe.jpg"); // 원본 이미지
            Mat gray = new Mat(); // 흑백 이미지
            Mat hist = new Mat(); // 히스토그램   

            // 가로 256, 세로 이미지 크기 만큼 1로만 가득 차 있는 매트릭스 생성
            Mat result = Mat.Ones(new OpenCvSharp.Size(256, src.Height), MatType.CV_8UC1); // CV_8UC1 : 8bit 1channel 이미지
            Mat dst = new Mat();

            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY); // 원본 이미지 흑백으로 변환

            // 히스토그램 값 생성
            // 원본 매트릭스, 채널 수, 이미지 마스크, 출력 매트릭스, 차원 수, 히스토그램 크기(가로), 각 차원에 대한 색상 값 범위
            Cv2.CalcHist(new Mat[] { gray }, new int[] { 0 }, null, hist, 1, new int[] { 256 }, new Rangef[] { new Rangef(0, 256) });

            // 값의 범위를 정규화 (최소값 60~ 최대값 210 이었다면, 최소값 0 ~ 최대값 355로 변환)
            // 원본 매트릭스, 출력 매트릭스, 최소값(MinMax일 경우), 최대값(MinMax일 경우), 변환 방식
            Cv2.Normalize(hist, hist, 0, 2555, NormTypes.MinMax);

            // 히스토그램 겂을 갖고 값의 크기만큼 선의 길이를 정하고 선을 그림
            for (int i = 0; i < hist.Rows; i++)
            {
                Cv2.Line(result,
                    new OpenCvSharp.Point(i, src.Height),
                    new OpenCvSharp.Point(i, src.Height - (int)hist.Get<float>(i)),
                    Scalar.White);

            }

            // 가로로 매트릭스 두개를 붙이기 (높이가 같아야만 가능)
            Cv2.HConcat(new Mat[] { gray, result }, dst);
            Cv2.ImShow("dst", dst);

            // 사용자 입력이 들어오면 창을 모두 닫기
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        
        }
    }
}