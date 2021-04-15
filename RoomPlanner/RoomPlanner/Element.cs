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
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public bool isSelected = false;

        public MainWindow mainWindow;
        public FrameworkElement frameworkElement;

        public void ClickOnElement(object sender, MouseButtonEventArgs e)
        {
            mainWindow.lockedElement = this;
            mainWindow.lockedElement.isSelected = true;
            mainWindow.PropertyList.Visibility = Visibility.Visible;
            mainWindow.ObjHeight.Text = Convert.ToString(mainWindow.lockedElement.Height);
            mainWindow.ObjWidth.Text = Convert.ToString(mainWindow.lockedElement.Width);

            mainWindow.ttop = Canvas.GetTop(mainWindow.lockedElement.frameworkElement);
            mainWindow.lleft = Canvas.GetLeft(mainWindow.lockedElement.frameworkElement);

            Point point = e.GetPosition(mainWindow.WorkTable);
            mainWindow.deltaX = point.X;
            mainWindow.deltaY = point.Y;
        }

        public void DeclineElement(object sender, MouseButtonEventArgs e)
        {
            mainWindow.lockedElement = null;
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
            this.frameworkElement = element;
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
            this.frameworkElement = element;
            Canvas.SetLeft(element, (mainWindow.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (mainWindow.WorkTable.ActualHeight - height) / 2);
            mainWindow.WorkTable.Children.Add(element);
            element.MouseLeftButtonDown += ClickOnElement;
            element.MouseLeftButtonUp += DeclineElement;
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
            this.mainWindow = mainWindow;

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
            mainWindow.WorkTable.Children.Add(element);
            this.frameworkElement = element;

            Canvas.SetLeft(element, (mainWindow.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (mainWindow.WorkTable.ActualHeight - height) / 2);

            element.MouseLeftButtonDown += ClickOnElement;
            element.MouseLeftButtonUp += DeclineElement;
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
            this.mainWindow = mainWindow;

            Path element = new Path();
            element.Stroke = Brushes.Black;
            element.StrokeThickness = 1;
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, 204, 204, 255);
            element.Fill = mySolidColorBrush;

            RectangleGeometry myRectGeometry = new RectangleGeometry();
            myRectGeometry.Rect = new Rect(-50, -75, 100, 150);

            element.Data = myRectGeometry;
            //GeometryGroup myGeometryGroup = new GeometryGroup();
            //myGeometryGroup.Children.Add(myRectGeometry);
            //myPath.Data = myGeometryGroup;
            this.frameworkElement = element;

            Canvas.SetLeft(element, (mainWindow.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (mainWindow.WorkTable.ActualHeight - height) / 2);
            mainWindow.WorkTable.Children.Add(element);

            element.MouseLeftButtonDown += ClickOnElement;
            element.MouseLeftButtonUp += DeclineElement;
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