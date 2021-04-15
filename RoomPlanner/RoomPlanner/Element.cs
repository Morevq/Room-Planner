using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace RoomPlanner
{
    abstract public class Element
    {
        protected int width;
        protected int height;
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }

        public MainWindow mainWindow;

        public void ClickOnElement(object sender, MouseButtonEventArgs e)
        {
            mainWindow.selectedElement = (FrameworkElement)sender;
            mainWindow.PropertyList.Visibility = Visibility.Visible;
            mainWindow.ObjHeight.Text = Convert.ToString(mainWindow.selectedElement.Height);
            mainWindow.ObjWidth.Text = Convert.ToString(mainWindow.selectedElement.Width);

            mainWindow.ttop = Canvas.GetTop(mainWindow.selectedElement);
            mainWindow.lleft = Canvas.GetLeft(mainWindow.selectedElement);

            Point point = e.GetPosition(mainWindow.WorkTable);
            mainWindow.deltaX = point.X;
            mainWindow.deltaY = point.Y;
        }

        public void DeclineElement(object sender, MouseButtonEventArgs e)
        {
            mainWindow.selectedElement = null;
        }
    }

    public class Room : Element
    {
        public Room(MainWindow mainWindow, int width = 600, int height = 600)
        {
            Width = width;
            Height = height;
            this.mainWindow = mainWindow;
            Rectangle element = new Rectangle()
            {
                Width = Width,
                Height = Height,
                Stroke = Brushes.Black,
                StrokeThickness = 20
            };
            Canvas.SetLeft(element, (mainWindow.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (mainWindow.WorkTable.ActualHeight - height) / 2);
            element.MouseLeftButtonDown += ClickOnElement;
            element.MouseLeftButtonUp += DeclineElement;
            mainWindow.WorkTable.Children.Add(element);

        }

        public override int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public override int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
    }

    public class Wardrobe : Element
    {
        public Wardrobe(MainWindow mainWindow, int width = 100, int height = 50)
        {
            Width = width;
            Height = height;
            this.mainWindow = mainWindow;
            Rectangle element = new Rectangle()
            {
                Width = Width,
                Height = Height,
                Stroke = Brushes.Black,
                Fill = Brushes.Black
            };
            Canvas.SetLeft(element, (mainWindow.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (mainWindow.WorkTable.ActualHeight - height) / 2);
            mainWindow.WorkTable.Children.Add(element);
<<<<<<< Updated upstream
            element.MouseLeftButtonDown += ClickOnElement;
            element.MouseLeftButtonUp += DeclineElement;
=======

            element.MouseLeftButtonDown += ClickOnObject;
            element.MouseLeftButtonUp += DeclineObject;
        }

        public override int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public override int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
    }

    public class Door : Element
    {
        public Door(MainWindow mainWindow)
        {
            PathGeometry pathGeom = new PathGeometry();
            Path element = new Path();

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

            element.Data = pathGeom;
            element.Stroke = Brushes.Green;
            element.StrokeThickness = 10;
            Canvas.SetLeft(element, 50);
            Canvas.SetTop(element, 50);
            mainWindow.WorkTable.Children.Add(element);


            Canvas.SetLeft(element, (mainWindow.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (mainWindow.WorkTable.ActualHeight - height) / 2);

            element.MouseLeftButtonDown += ClickOnObject;
            element.MouseLeftButtonUp += DeclineObject;
        }

        public override int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;

            }
        }

        public override int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
    }

    public class Bed : Element
    {
        public Bed(MainWindow mainWindow)
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
            mainWindow.WorkTable.Children.Add(mainPanel);

            myPath.MouseLeftButtonDown += ClickOnObject;
            myPath.MouseLeftButtonUp += DeclineObject;
>>>>>>> Stashed changes
        }

        public override int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public override int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
    }
}