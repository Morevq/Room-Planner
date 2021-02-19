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

namespace RoomPlanner
{
    public partial class MainWindow : Window
    {
        Rectangle room;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            room = new Rectangle()
            {
                Width = 600,
                Height = 600,
                Stroke = Brushes.Black,
                StrokeThickness = 20
            };
            Canvas.SetLeft(room, (WorkTable.ActualWidth - 600) / 2);
            Canvas.SetTop(room, (WorkTable.ActualHeight - 600) / 2);
            room.AddHandler(Rectangle.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Rectangle_MouseLeftButtonUp));
            WorkTable.Children.Add(room);
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = (Rectangle)sender;
            PropertyList.Visibility = Visibility.Visible;
            ObjHeight.Text = Convert.ToString(rectangle.Height);
            ObjWidth.Text = Convert.ToString(rectangle.Width);
        }

        private void ObjHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Background = Convert.ToString(room.Height) != textBox.Text ?
                Brushes.LightGreen : Brushes.White;
        }

        private void ObjWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Background = Convert.ToString(room.Width) != textBox.Text ? 
                Brushes.LightGreen : Brushes.White;
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            room.Height = Convert.ToDouble(ObjHeight.Text);
            room.Width = Convert.ToDouble(ObjWidth.Text);
            ObjHeight.Background = Brushes.White;
            ObjWidth.Background = Brushes.White;
            PropertyList.Visibility = Visibility.Hidden;
        }
        private void CreateRoomButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            room = new Rectangle()
            {
                Width = 100,
                Height = 50,
                Stroke = Brushes.Black,
                Fill = Brushes.Black
            };
            Canvas.SetLeft(room, 800);
            Canvas.SetRight(room, 900);
            Canvas.SetTop(room, 500);
            WorkTable.Children.Add(room);
        }

        private void CreateRoomButton_Copy1_Click(object sender, RoutedEventArgs e)
        {
            PathGeometry pathGeom = new PathGeometry();
            Path p = new Path();

            LineSegment vertLS = new LineSegment();
            PathFigure vertPF = new PathFigure();
            vertPF.StartPoint = new Point(650, 600);
            vertLS.Point = new Point(600, 600);
            vertPF.Segments.Add(vertLS);
            pathGeom.Figures.Add(vertPF);

            LineSegment horLS = new LineSegment();
            PathFigure horPF = new PathFigure();
            horPF.StartPoint = new Point(650, 600);
            horLS.Point = new Point(650, 550);
            horPF.Segments.Add(horLS);
            pathGeom.Figures.Add(horPF);

            ArcSegment arc = new ArcSegment();
            PathFigure arcfrst = new PathFigure();
            //arcfrst.IsClosed = true;
            arc.Point = new Point(600, 600);
            arcfrst.StartPoint = new Point(650, 550);
            arc.Size = new Size(75, 75);
            //arcfrst.Size = new Size(250, 250);
            //arcfrst.IsClosed = false;
            arcfrst.Segments.Add(arc);
            pathGeom.Figures.Add(arcfrst);


            p.Data = pathGeom;
            p.Stroke = Brushes.Green;
            Canvas.SetLeft(p, 800);
            Canvas.SetRight(p, 900);
            Canvas.SetTop(p, 500);
            Grid.Children.Add(p);
        }
    }
}
