using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenCvSharp;

namespace Opencv_1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //string imgPath = "F:\\cat.jpg";
            //Mat image = Cv2.ImRead(imgPath);
            // 새로운 이미지 생성
            Mat draw = new Mat(new OpenCvSharp.Size(640, 360), MatType.CV_8UC3, OpenCvSharp.Scalar.All(255));


            // 선그리기
            Cv2.Line(draw, new OpenCvSharp.Point(10, 10), new OpenCvSharp.Point(630, 10), Scalar.Red, 10, LineTypes.AntiAlias);
            Cv2.Line(draw, new OpenCvSharp.Point(10, 30), new OpenCvSharp.Point(630, 30), Scalar.Orange, 10, LineTypes.AntiAlias);

            // 원그리기
            Cv2.Circle(draw, new OpenCvSharp.Point(30, 70), 20, Scalar.Yellow, 10, LineTypes.AntiAlias);
            Cv2.Circle(draw, new OpenCvSharp.Point(90, 70), 25, Scalar.Green, -1, LineTypes.AntiAlias);

            // 사각형그리기
            Cv2.Rectangle(draw, new OpenCvSharp.Rect(130, 50, 40, 40), Scalar.Blue, 10, LineTypes.AntiAlias);
            Cv2.Rectangle(draw, new OpenCvSharp.Point(185, 45), new OpenCvSharp.Point(235, 95) ,Scalar.Navy, -1, LineTypes.AntiAlias);

            // 타원 그리그
            Cv2.Ellipse(draw, new RotatedRect(new Point2f(290, 70), new Size2f(75, 50), 30), Scalar.Purple, 10, LineTypes.AntiAlias);

            // 결과를 화면에 표시
            Cv2.ImShow("Drawn Shapes", draw);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

        }
    }
}
