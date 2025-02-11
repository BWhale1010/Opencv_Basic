using System;
using System.Windows;
using OpenCvSharp;

namespace Ex_1
{
    public partial class MainWindow : System.Windows.Window
    {
        private static bool isDrawing = false;
        private static Mat canvas;

        public MainWindow()
        {
            InitializeComponent();

            // 흰색 배경 보드 생성
            canvas = new Mat(new OpenCvSharp.Size(500, 500), MatType.CV_8UC3, new Scalar(255, 255, 255));

            Cv2.ImShow("white board", canvas);

            // 마우스 콜백 설정
            Cv2.SetMouseCallback("white board", MyMouseEvent);

            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }

        // 마우스 이벤트 핸들러
        private static void MyMouseEvent(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userdata)
        {
            OpenCvSharp.Point lastPoint;

            if (@event == MouseEventTypes.LButtonDown)
            {
                // 마우스 왼쪽 버튼을 누르면 드로잉 시작
                isDrawing = true;
                lastPoint = new OpenCvSharp.Point(x, y);
                DrawCircle(x, y);  // 클릭한 지점에도 원을 찍음
            }
            else if (@event == MouseEventTypes.MouseMove && isDrawing)
            {
                // 마우스를 움직이는 동안 원을 그리면서 선처럼 보이게 함
                DrawCircle(x, y);
                lastPoint = new OpenCvSharp.Point(x, y);
            }
            else if (@event == MouseEventTypes.LButtonUp)
            {
                // 마우스 왼쪽 버튼을 놓으면 드로잉 종료
                isDrawing = false;
            }
        }

        // 원(점)을 그리는 함수
        private static void DrawCircle(int x, int y)
        {
            int brushSize = 3;  // 원의 크기 (브러시 크기)
            Scalar brushColor = Scalar.Black;  // 원의 색 (검정색)

            // 원 그리기
            Cv2.Circle(canvas, new OpenCvSharp.Point(x, y), brushSize, brushColor, -1);  // -1은 채워진 원을 의미

            // 화면에 업데이트
            Cv2.ImShow("white board", canvas);
        }
    }
}
