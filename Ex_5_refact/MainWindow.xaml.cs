using System;
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

namespace Ex_5_refact
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private Mat original;
        private Mat gray;

        // 필터 민감도 변수
        private int sobelValue = 1;
        private int scharrScale = 1;
        private int laplacianValue = 1;
        private int cannyThreshold1 = 50;
        private int cannyThreshold2 = 150;

        public MainWindow()
        {
            InitializeComponent();

            // 이미지 로드 및 그레이스케일 변환
            original = Cv2.ImRead("F:\\pepe.jpg");
            if (original.Empty())
            {
                MessageBox.Show("이미지를 불러올 수 없습니다.");
                Application.Current.Shutdown();
            }
            gray = new Mat();
            Cv2.CvtColor(original, gray, ColorConversionCodes.BGR2GRAY);

            // UI 구성
            CreateUI();
        }

        private void CreateUI()
        {
            // 메인 그리드 설정
            Grid mainGrid = new Grid();
            this.Content = mainGrid;

            for (int i = 0; i < 3; i++)
            {
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            mainGrid.RowDefinitions.Add(new RowDefinition());
            mainGrid.RowDefinitions.Add(new RowDefinition());

            // 각 필터별 이미지와 입력 박스 추가
            AddFilterControl(mainGrid, "Original Image", 0, 0, null);
            AddFilterControl(mainGrid, "Sobel Filter", 0, 1, ApplySobel);
            AddFilterControl(mainGrid, "Scharr Filter", 0, 2, ApplyScharr);
            AddFilterControl(mainGrid, "Laplacian Filter", 1, 1, ApplyLaplacian);
            AddFilterControl(mainGrid, "Canny Edge", 1, 2, ApplyCanny);
        }

        private void AddFilterControl(Grid grid, string title, int row, int column, Action applyFilter)
        {
            StackPanel panel = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(10) };
            grid.Children.Add(panel);
            Grid.SetRow(panel, row);
            Grid.SetColumn(panel, column);

            // 필터 이름 표시
            TextBlock titleText = new TextBlock
            {
                Text = title,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            panel.Children.Add(titleText);

            // 이미지 표시
            Image imgControl = new Image { Width = 640, Height = 500, Margin = new Thickness(5) };
            panel.Children.Add(imgControl);

            // 텍스트 박스 추가 (원본 이미지는 제외)
            if (applyFilter != null)
            {
                StackPanel inputPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };
                TextBox inputBox = new TextBox { Width = 100, Margin = new Thickness(5) };
                Button applyButton = new Button { Content = "Apply", Margin = new Thickness(5) };

                applyButton.Click += (s, e) =>
                {
                    if (int.TryParse(inputBox.Text, out int value))
                    {
                        // 입력 값에 따라 필터 민감도 조절
                        if (title == "Sobel Filter") sobelValue = value;
                        else if (title == "Scharr Filter") scharrScale = value;
                        else if (title == "Laplacian Filter") laplacianValue = value;
                        else if (title == "Canny Edge") { cannyThreshold1 = value; cannyThreshold2 = value + 50; }

                        applyFilter();
                        imgControl.Source = ConvertMatToBitmapSource(GetFilteredMat(title));
                    }
                    else
                    {
                        MessageBox.Show("유효한 숫자를 입력하세요.");
                    }
                };

                inputPanel.Children.Add(inputBox);
                inputPanel.Children.Add(applyButton);
                panel.Children.Add(inputPanel);
            }
            else
            {
                imgControl.Source = ConvertMatToBitmapSource(original);
            }
        }

        // 필터 적용 메서드들
        private void ApplySobel()
        {
            Mat sobel = new Mat();
            Cv2.Sobel(gray, sobel, MatType.CV_8U, 1, 1, sobelValue * 2 + 1);
        }

        private void ApplyScharr()
        {
            Mat scharrX = new Mat();
            Mat scharrY = new Mat();
            Cv2.Scharr(gray, scharrX, MatType.CV_8U, 1, 0, scharrScale);
            Cv2.Scharr(gray, scharrY, MatType.CV_8U, 0, 1, scharrScale);
            Cv2.AddWeighted(scharrX, 0.5, scharrY, 0.5, 0, scharrX);
        }

        private void ApplyLaplacian()
        {
            Mat laplacian = new Mat();
            Cv2.Laplacian(gray, laplacian, MatType.CV_8U, laplacianValue * 2 + 1);
        }

        private void ApplyCanny()
        {
            Mat canny = new Mat();
            Cv2.Canny(gray, canny, cannyThreshold1, cannyThreshold2);
        }

        // 필터된 이미지 반환 메서드
        private Mat GetFilteredMat(string filterName)
        {
            Mat result = new Mat();
            if (filterName == "Sobel Filter") Cv2.Sobel(gray, result, MatType.CV_8U, 1, 1, sobelValue * 2 + 1);
            else if (filterName == "Scharr Filter")
            {
                Mat scharrX = new Mat();
                Mat scharrY = new Mat();
                Cv2.Scharr(gray, scharrX, MatType.CV_8U, 1, 0, scharrScale);
                Cv2.Scharr(gray, scharrY, MatType.CV_8U, 0, 1, scharrScale);
                Cv2.AddWeighted(scharrX, 0.5, scharrY, 0.5, 0, result);
            }
            else if (filterName == "Laplacian Filter") Cv2.Laplacian(gray, result, MatType.CV_8U, laplacianValue * 2 + 1);
            else if (filterName == "Canny Edge") Cv2.Canny(gray, result, cannyThreshold1, cannyThreshold2);
            return result;
        }

        // Mat -> BitmapSource 변환 (WPF에서 이미지 표시용)
        private BitmapSource ConvertMatToBitmapSource(Mat mat)
        {
            return OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(mat);
        }
    }
}