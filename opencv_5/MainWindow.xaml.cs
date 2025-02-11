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

namespace opencv_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Mat src = new Mat(new OpenCvSharp.Size(500, 500), MatType.CV_8UC3, new Scalar(255, 255, 255));

            Cv2.ImShow("white board", src);

            MouseCallback cvMouseCallback = new MouseCallback(MyMouseEvent);
            Cv2.SetMouseCallback("white board", cvMouseCallback, src.CvPtr);


            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }


        static void MyMouseEvent(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userdata)
        {
            if(flags == MouseEventFlags.LButton)
            {
                MessageBox.Show($"마우스 좌표 {x},{y}");
            }
        }
    }
}