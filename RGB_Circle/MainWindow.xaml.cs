using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace RGB_Circle
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public Point MousePosition { get; private set; } = new Point();
		public double WindowHeight { get; set; }
		public double WindowWidth { get; set; }

		public MainWindow()
		{
			InitializeComponent();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			MousePosition = Mouse.GetPosition(Application.Current.MainWindow);
			OnPropertyChanged("MousePosition");
			double xMapped = mapf(MousePosition.X, 0, WindowWidth, -1, 1);
			double yMapped = mapf(MousePosition.Y, 0, WindowHeight, -1, 1);
			double angle = FindAngle(xMapped, yMapped);
			ConvertToRGB(angle);
			labelCoordinates.Content = $"X: {xMapped}; Y: {yMapped}; Angle: {angle}";
		}

		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}

		double mapf(double val, double in_min, double in_max, double out_min, double out_max)
		{
			return (val - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			WindowHeight = this.ActualHeight;
			WindowWidth = this.ActualWidth;
		}

		double FindAngle(double coordinateX, double coordinateY)
		{
			double hypotenuse = Math.Sqrt(Math.Pow(coordinateX, 2) + Math.Pow(coordinateY, 2));
			double sinA = coordinateY / hypotenuse;
			double cosA = coordinateX / hypotenuse;
			double asinA = Math.Asin(sinA);
			double acosA = Math.Acos(cosA);
			double angle = 0;
			if (asinA >= 0 && acosA > 0)
			{
				angle = Math.PI + acosA; 
				//Console.WriteLine("Asin and acos positive");
			}
			else
			{
				angle = Math.PI - acosA;
				//Console.WriteLine("Asin negative acos positive");
			}
			return angle;
		}

		void ConvertToRGB(double angle)
		{
			int redValue = 0;
			if (Math.Sin(angle) <= -0.5)
			{
				redValue = 0;
			}
			else
			{
				redValue = (int)mapf(Math.Sin(angle), -0.5, 1, 0, 255);
			}
			int greenValue = 0;
			if (angle <= Math.PI / 2 || angle >= (11 * Math.PI) / 6)
			{
				greenValue = 0;
			}
			else
			{
				greenValue = (int)mapf(Math.Sin(angle - (2 * Math.PI) / 3), -0.5, 1, 0, 255);
			}
			int blueValue = 0;
			if (angle >= Math.PI / 2 && angle <= (7 * Math.PI) / 6)
			{
				blueValue = 0;
				//Console.WriteLine($"angle = {angle}, PI / 2 = {Math.PI / 2}, 7PI / 6 = {(7 * Math.PI) / 6} ");
				//Console.WriteLine("Value between PI/2 and 7PI/6");
			}
			else
			{
				blueValue = (int)mapf(Math.Sin(angle - (4 * Math.PI) / 3), -0.5, 1, 0, 255);
			}
			Console.WriteLine($"Red: {redValue} Green: {greenValue} Blue: {blueValue}");
			RGBColor(redValue, greenValue, blueValue);
		}

		private void RGBColor(int redValue, int greenValue, int blueValue)
		{
			this.Background = new SolidColorBrush(Color.FromRgb((byte)redValue, (byte)greenValue, (byte)blueValue));
		}
	}
}
