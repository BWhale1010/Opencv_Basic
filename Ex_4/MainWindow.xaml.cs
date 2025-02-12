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
using Point = OpenCvSharp.Point;

namespace Ex_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private static OpenCvSharp.Point mousePosition = new Point(0, 0);  // 마우스 위치 저장
        private static bool isMousePressed = false;            // 마우스 클릭 여부 저장

        public MainWindow()
        {
            InitializeComponent();

            VideoCapture capture = new VideoCapture(0); // 기본 웹캠 (0번 장치)

            // 웹캠 열기 확인
            if (!capture.IsOpened())
            {
                MessageBox.Show("웹캠을 열 수 없습니다. 연결 상태를 확인하세요.");
                Application.Current.Shutdown();
                return;
            }

            // 해상도 설정
            capture.Set(VideoCaptureProperties.FrameWidth, 640);
            capture.Set(VideoCaptureProperties.FrameHeight, 480);

            Mat frame = new Mat();

            // 마우스 콜백 함수 등록
            Cv2.NamedWindow("Camera View");
            Cv2.SetMouseCallback("Camera View", OnMouseEvent);


            // 영상 스트리밍 루프
            while (true)
            {
                capture.Read(frame);

                if (!frame.Empty())
                {
                    // 마우스 클릭 상태일 때, 마우스 위치에 빨간 원 그리기
                    if (isMousePressed)
                    {
                        Cv2.Circle(frame, mousePosition, 10, new Scalar(0, 0, 255), -1); // 빨간색 원
                    }

                    Cv2.ImShow("Camera View", frame);
                }
                else
                {
                    MessageBox.Show("프레임을 읽을 수 없습니다.");
                    break;
                }

                // 'q' 키를 누르면 종료
                if (Cv2.WaitKey(33) == 'q')
                {
                    break;
                }
            }

            // 자원 해제
            capture.Release();
            Cv2.DestroyAllWindows();
        }

        private static void OnMouseEvent(MouseEventTypes eventType, int x, int y, MouseEventFlags flags, IntPtr userdata)
        {
            if(eventType == MouseEventTypes.LButtonDown)
            {
                isMousePressed = true;
                mousePosition = new Point(x, y); // 클릭한 위치 저장
            }else if (eventType == MouseEventTypes.LButtonUp)
            {
                isMousePressed = false; // 마우스 클릭 해제 시
            }else if(eventType == MouseEventTypes.MouseMove && isMousePressed)
            {
                mousePosition = new Point(x, y);
            }

        }
    }
}