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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NA_laba7_MNK
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnMNK_Click(object sender, RoutedEventArgs e)
        {
            Line l = new Line();
            l.X1 = 0;l.Y1 = 295;
            l.X2 = 600;l.Y2 = 295;
            l.Fill = Brushes.Black;
            l.StrokeThickness = 5;
            cnv.Children.Add(l);

            Line l1 = new Line();
            l1.X1 = 0; l1.Y1 = 0;
            l1.X2 = 0; l1.Y2 = 295;
            l1.Fill = Brushes.Black;
            l1.StrokeThickness = 5;
            cnv.Children.Add(l1);
            List<KeyValuePair<double, double>> xy = new List<KeyValuePair<double, double>>()
            {
                new KeyValuePair<double, double>(0.5,5.77),
                new KeyValuePair<double, double>(1,0.07),
                new KeyValuePair<double, double>(11.5,6.95),
                new KeyValuePair<double, double>(2,12.05),
                new KeyValuePair<double, double>(2.5,18.97),
                new KeyValuePair<double, double>(3,25.67),
                new KeyValuePair<double, double>(3.5,31.57),
                new KeyValuePair<double, double>(4,38.44),
                new KeyValuePair<double, double>(4.5,46.2),
                new KeyValuePair<double, double>(5,51.33),
                new KeyValuePair<double, double>(5.5,58.83)
            };
            List<KeyValuePair<double, double>> xyn = new List<KeyValuePair<double, double>>()
{
new KeyValuePair<double, double>(1.1,0.3),
new KeyValuePair<double, double>(1.7,0.6),
new KeyValuePair<double, double>(2.4,1.1),
new KeyValuePair<double, double>(3.0,1.7),
new KeyValuePair<double, double>(3.7,2.3),
new KeyValuePair<double, double>(4.5,3.0),
new KeyValuePair<double, double>(5.1,3.8),
new KeyValuePair<double, double>(5.8,4.6)
};
           

            double[,] delS = new double[2,3];
            
                double sum_x = 0;
                double sum_x2 = 0;
                double sum_xy = 0;
                double sum_y = 0;

            double m, c;
            double sum_lnx = 0;
            double sum_lnxy = 0;
            double sum_lny = 0;
            double sum_lnx2 = 0;
            for (int i = 0; i < xy.Count; i++)
            {
                KeyValuePair<double, double> x = xy[i];
                sum_x += x.Key;
                sum_x2 += x.Key * x.Key;
                sum_xy += x.Key * x.Value;
                sum_y += x.Value;

                sum_lnx += Math.Log(x.Key);
                sum_lnxy += Math.Log(x.Key) * Math.Log(x.Value);
                sum_lny += Math.Log(x.Value);
                sum_lnx2 += Math.Log(x.Key)* Math.Log(x.Key);
            }
            delS[0, 0] = sum_x2;
            delS[0, 1] = sum_x;
            delS[0, 2] = sum_xy;
            delS[1, 0] = sum_x;
            delS[1, 1] = xy.Count;
            delS[1, 2] = sum_y;
            double[] X = obr(gauss(delS, 2), 2);
            tbRes.Text = string.Format("y = {0:F4}x + {1:F4}", X[0], X[1]);
           
            m = (xy.Count * sum_lnxy - sum_lnx * sum_lny ) / 
                (xy.Count * sum_lnx2 - sum_lnx * sum_lnx);
            c = Math.Pow(Math.E, (sum_lny - m * sum_lnx) / xy.Count);
            tbRes.Text += string.Format("\ny = {0:F4}x^{1:F4}", c, m);

            List<KeyValuePair<double, double>> lin = new List<KeyValuePair<double, double>>();
            List<KeyValuePair<double, double>> polin = new List<KeyValuePair<double, double>>();
            for (int i = 0; i < xy.Count; i++)
            {
                KeyValuePair<double, double> x = xy[i];
                lin.Add(new KeyValuePair<double, double>(x.Key, func1(x.Key, X[0], X[1])));
                polin.Add(new KeyValuePair<double, double>(x.Key, func2(x.Key, c, m)));
            }
            Gr(xy, Brushes.Red);
            Gr(lin, Brushes.Black);
            Gr(polin, Brushes.Green);
        }
        public double func1(double x, double k, double b)
        {
            return x * k - b;
        }
        public double func2(double x, double c, double m)
        {
            return c * Math.Pow(x, m);
        }
        private void Gr(List<KeyValuePair<double, double>> xy, Brush br)
        {
            int k = 5;
            for (int i = 0; i < xy.Count; i++)
            {
                Ellipse l = new Ellipse();
                l.Width = l.Height = 3;
                l.Fill = br;
                Canvas.SetTop(l, 300 - xy[i].Key * k);
                Canvas.SetLeft(l, xy[i].Value * k+200);
                cnv.Children.Add(l);
            }

        }
        public static double[,] gauss(double[,] A, int n)
        {
            for (int i = 0; i < n; i++)
            {
                double buf = A[i, i];
                for (int j = i; j < n + 1; j++) A[i, j] /= buf;

                for (int k = i + 1; k < n; k++)
                {
                    buf = A[k, i];
                    for (int j = i; j < n + 1; j++)
                        A[k, j] = buf * A[i, j] - A[k, j];
                }
            }
            return A;
        }
        public static double[] obr(double[,] A, int n)
        {
            double[] x = new double[n + 1];
            for (int i = n - 1; i >= 0; i--)
            {
                double buf = 0;
                for (int k = i; k < n; k++)
                {
                    buf += A[i, k] * x[k];
                }
                x[i] = A[i, n] - buf;
            }
            return x;
        }
    }
}
