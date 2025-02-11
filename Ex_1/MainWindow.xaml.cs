using System;
using System.Windows;
using OpenCvSharp;

namespace Ex_1
{
    public partial class MainWindow : System.Windows.Window
    {
        private static bool isDrawing = false;
        private static Mat image;

        public MainWindow()
        {
            InitializeComponent();

            string imgPath = @"F:\\pepe.jpg";
            image = Cv2.ImRead(imgPath);

            Cv2.Resize(image, image, new OpenCvSharp.Size(500, 500), interpolation: InterpolationFlags.Linear); // 이미지 크기 조정

            Cv2.ImShow("board", image);

            // 마우스 콜백 설정
            Cv2.SetMouseCallback("board", MyMouseEvent);

            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }

        // 마우스 이벤트 핸들러
        private static void MyMouseEvent(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userdata)
        {
            OpenCvSharp.Point lastPoint;

            //  마우스 왼쪽 버튼 클릭
            if (@event == MouseEventTypes.LButtonDown)
            {
                isDrawing = true;
                lastPoint = new OpenCvSharp.Point(x, y);
                DrawCircle(x, y);  
            }
            //  마우스 왼쪽 버튼 클릭 중 드래그
            else if (@event == MouseEventTypes.MouseMove && isDrawing)
            {
                DrawCircle(x, y);
                lastPoint = new OpenCvSharp.Point(x, y);
            }
            //  마우스 왼쪽 버튼 업
            else if (@event == MouseEventTypes.LButtonUp)
            {
                isDrawing = false;
            }
        }

        // 원(점)을 그리는 함수
        private static void DrawCircle(int x, int y)
        {
            int brushSize = 3;
            Scalar brushColor = Scalar.Black;

            // 원 그리기
            Cv2.Circle(image, new OpenCvSharp.Point(x, y), brushSize, brushColor, -1);

            // 화면에 업데이트
            Cv2.ImShow("board", image);
        }
    }
}
