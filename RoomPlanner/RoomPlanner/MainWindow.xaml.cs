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
            switch(button.Name)
            {
                case "CreateRoomButton":
                    Room room1 = new Room(this);
                    break;
                case "CreateCupboardButton":
                    //Rectangle room = new Rectangle()
                    //{
                    //    Width = 100,
                    //    Height = 50,
                    //    Stroke = Brushes.Black,
                    //    Fill = Brushes.Black
                    //};
                    //Canvas.SetLeft(room, 800);
                    //Canvas.SetRight(room, 900);
                    //Canvas.SetTop(room, 500);
                    //WorkTable.Children.Add(room);
                    //room.AddHandler(Rectangle.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Rectangle_MouseLeftButtonUp));
                    break;
            }
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            //room.Height = Convert.ToDouble(ObjHeight.Text);
            //room.Width = Convert.ToDouble(ObjWidth.Text);
            //ObjHeight.Background = Brushes.White;
            //ObjWidth.Background = Brushes.White;
            //PropertyList.Visibility = Visibility.Hidden;
        }

        private void CreateDoorButton_Click(object sender, RoutedEventArgs e)
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
            p.Width = 100;
            p.Stroke = Brushes.Green;
            Canvas.SetLeft(p, 800);
            Canvas.SetRight(p, 900);
            Canvas.SetTop(p, 500);
            Grid.Children.Add(p);
        }
    }
}
