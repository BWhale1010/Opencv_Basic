using System;
using System.Windows;
using OpenCvSharp;

namespace opencv_7
{
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            VideoCapture capture = new VideoCapture(0);  // 기본 웹캠 (0번 장치)

            // 웹캠 열기 확인
            if (!capture.IsOpened())
            {
                MessageBox.Show("웹캠을 열 수 없습니다. 장치 연결 상태를 확인하세요.");
                Application.Current.Shutdown();
                return;
            }

            // 해상도 설정
            capture.Set(VideoCaptureProperties.FrameWidth, 640);
            capture.Set(VideoCaptureProperties.FrameHeight, 480);

            Mat frame = new Mat();

            // 영상 스트리밍 루프
            while (true)
            {
                capture.Read(frame);

                if (!frame.Empty())
                {
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
    }
}
