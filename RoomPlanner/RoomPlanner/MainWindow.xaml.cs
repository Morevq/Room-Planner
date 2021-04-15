﻿using System;
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
                    Rectangle room = new Rectangle()
                    {
                        Width = 100,
                        Height = 50,
                        Stroke = Brushes.Black,
                        Fill = Brushes.Black
                    };
                    Canvas.SetLeft(room, 800);
                    //Canvas.SetRight(room, 900);
                    Canvas.SetTop(room, 500);
                    WorkTable.Children.Add(room);
                    ttop = Canvas.GetTop(room);
                    lleft = Canvas.GetLeft(room);
                    room.MouseLeftButtonDown += ClickOnObject;
                    //room.MouseMove += MoveObject;
                    room.MouseLeftButtonUp += DeclineObject;
                    //room.AddHandler(Rectangle.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Rectangle_MouseLeftButtonUp));
                    break;
            }
        }
        private FrameworkElement currobj = null;
        public double deltaX, deltaY, lleft, ttop;
        /// <summary>
        /// Нажатие на объект
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClickOnObject(object sender, MouseButtonEventArgs e)
        {
            currobj = (FrameworkElement)sender;
            ttop = Canvas.GetTop(currobj);
            lleft = Canvas.GetLeft(currobj);

            //Vector offset = VisualTreeHelper.GetOffset(currobj);
            //lleft = offset.X;
            //ttop = offset.Y;

            //Point p = currobj.TranslatePoint(new Point(0, 0), currobj);
            //double currentLeft = p.X;
            //double currentTop = p.Y;

            Point point = e.GetPosition(WorkTable);
            deltaX = point.X;
            deltaY = point.Y;
        }

        public void DeclineObject(object sender, MouseButtonEventArgs e)
        {
            currobj = null;
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            //room.Height = Convert.ToDouble(ObjHeight.Text);
            //room.Width = Convert.ToDouble(ObjWidth.Text);
            ObjHeight.Background = Brushes.White;
            ObjWidth.Background = Brushes.White;
            //PropertyList.Visibility = Visibility.Hidden;
        }

        private void CreateBadButton_Click(object sender, RoutedEventArgs e)
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

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(WorkTable);
            if (currobj != null)
            {
                Canvas.SetTop(currobj, point.Y - deltaY + ttop);
                Canvas.SetLeft(currobj, point.X - deltaX + lleft);

                //= new Thickness(point.X, point.Y, 0, 0);
                Console.WriteLine("x = " + point.X.ToString() + "  + delX = " + (point.X + deltaX) + "  top=" + ttop.ToString() + "  left=" + lleft.ToString());
                //Console.WriteLine(currobj.Margin.Top.ToString() + " " + currobj.Margin.Left.ToString() + " " + point.X.ToString() + " " + point.Y.ToString() + " ");
            }
            //MessageBox.Show(currobj.Margin.Top);
            //Console.WriteLine(currobj.Margin.Top);
            //currobj.Margin.Left
            //Top
            //currobj.GetType().GetProperty("Location").SetValue(currobj, new Point(point.X, point.Y - 50));

        }
    }
}