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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "CreateRoomButton":
                    Room room1 = new Room(this);
                    break;
                case "CreateCupboardButton":
                    Wardrobe wardrobe = new Wardrobe(this);
                    break;
            }
        }


        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CreateBedButton_Click(object sender, RoutedEventArgs e)
        {
            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, 204, 204, 255);
            myPath.Fill = mySolidColorBrush;

            RectangleGeometry myRectGeometry = new RectangleGeometry();
            myRectGeometry.Rect = new Rect(30, 55, 100, 150);

            GeometryGroup myGeometryGroup = new GeometryGroup();
            myGeometryGroup.Children.Add(myRectGeometry);
            myPath.Data = myGeometryGroup;

            StackPanel mainPanel = new StackPanel();
            mainPanel.Children.Add(myPath);
            //Canvas.SetLeft(myPath, 100);
            //Canvas.SetTop(myPath, 100);
            //ttop = Canvas.GetTop(mainPanel);
            //lleft = Canvas.GetLeft(mainPanel);
            //Canvas.SetLeft(mainPanel, 150);
            //Canvas.SetTop(mainPanel, 150);
            WorkTable.Children.Add(mainPanel);


            myPath.MouseLeftButtonDown += ClickOnObject;
            myPath.MouseLeftButtonUp += DeclineObject;
        }

        private void CreateDoorButton_Click(object sender, RoutedEventArgs e)
        {
            PathGeometry pathGeom = new PathGeometry();
            Path p = new Path();

            LineSegment vertLS = new LineSegment();
            PathFigure vertPF = new PathFigure();
            vertPF.StartPoint = new Point(50, 0);
            vertLS.Point = new Point(0, 0);
            vertPF.Segments.Add(vertLS);
            pathGeom.Figures.Add(vertPF);

            LineSegment horLS = new LineSegment();
            PathFigure horPF = new PathFigure();
            horPF.StartPoint = new Point(50, 0);
            horLS.Point = new Point(50, -50);
            horPF.Segments.Add(horLS);
            pathGeom.Figures.Add(horPF);

            ArcSegment arc = new ArcSegment();
            PathFigure arcfrst = new PathFigure();
            arc.Point = new Point(0, 0);
            arcfrst.StartPoint = new Point(50, -50);
            arc.Size = new Size(75, 75);
            arcfrst.IsClosed = false;
            arcfrst.Segments.Add(arc);
            pathGeom.Figures.Add(arcfrst);

            p.Data = pathGeom;
            p.Stroke = Brushes.Green;
            p.StrokeThickness = 10;
            Canvas.SetLeft(p, 50);
            Canvas.SetTop(p, 50);
            WorkTable.Children.Add(p);
            p.MouseLeftButtonDown += ClickOnObject;
            p.MouseLeftButtonUp += DeclineObject;
        }

    }
}