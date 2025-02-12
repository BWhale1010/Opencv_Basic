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

namespace opencv_6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string fileName = "F:\\sample_video";
            VideoCapture capture = new VideoCapture(fileName + ".mp4");
            Mat frame = new Mat();

            double time_stamp = capture.Get(VideoCaptureProperties.PosMsec);
            double fram_now = capture.Get(VideoCaptureProperties.PosFrames);
            double frame_width = capture.Get(VideoCaptureProperties.FrameWidth);
            double frame_height = capture.Get(VideoCaptureProperties.FrameHeight);
            double frame_cout = capture.Get(VideoCaptureProperties.FrameCount);
            double fps = capture.Get(VideoCaptureProperties.Fps);

            textBox.Text = time_stamp + "\r\n";
            textBox.Text += fram_now + "\r\n";
            textBox.Text += frame_width + "\r\n";
            textBox.Text += frame_height + "\r\n";
            textBox.Text += frame_cout + "\r\n";
            textBox.Text += fps + "\r\n";

            while (true)
            {
                if (capture.PosFrames == capture.FrameCount)
                {
                    capture.Open(fileName + ".mp4");
                }

                capture.Read(frame);
                Cv2.ImShow("F:\\sample_video.mp4", frame);

                if (Cv2.WaitKey(33) == 'q') break;
            }

            capture.Release();
            Cv2.DestroyAllWindows();

        }
    }
}