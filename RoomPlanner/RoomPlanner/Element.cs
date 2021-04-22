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
using System.Windows.Data;

namespace RoomPlanner
{
    abstract public class Element
    {
        protected int width;
        protected int height;
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        protected bool isSelected = false;
        public virtual bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                Brush brush = value ? Brushes.DarkBlue : Brushes.Black;
                shape.Stroke = brush;
                isSelected = value;
            }
        }
        public bool isDragged = false;

        public MainWindow mainWindow;
        public Shape shape;

        public void ClickOnElement(object sender, MouseButtonEventArgs e)
        {
            if (IsSelected)
            {
                if (mainWindow.lockedElement != null) mainWindow.lockedElement.IsSelected = false;
                mainWindow.lockedElement = this;
                mainWindow.MouseMove += mainWindow.Window_MouseMove;

                mainWindow.ttop = Canvas.GetTop(mainWindow.lockedElement.shape);
                mainWindow.lleft = Canvas.GetLeft(mainWindow.lockedElement.shape);

                Point point = e.GetPosition(mainWindow.WorkTable);
                isDragged = true;
                mainWindow.deltaX = point.X;
                mainWindow.deltaY = point.Y;
            }
            else
            {
                if (mainWindow.lockedElement != null) mainWindow.lockedElement.IsSelected = false;
                mainWindow.lockedElement = this;
                mainWindow.lockedElement.IsSelected = true;
                mainWindow.PropertyList.Visibility = Visibility.Visible;
                mainWindow.ObjHeight.Text = Convert.ToString(mainWindow.lockedElement.Height);
                mainWindow.ObjWidth.Text = Convert.ToString(mainWindow.lockedElement.Width);
            }
        }

        public void DeclineElement(object sender, MouseButtonEventArgs e)
        {
            if (mainWindow.lockedElement.isDragged)
            {
                mainWindow.lockedElement = null;
                isDragged = false;
            }
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
            shape = element;
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
                Fill = Brushes.Gray,
                StrokeThickness = 2
            };
            shape = element;
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
            this.shape = element;

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

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.FillRule = FillRule.Nonzero;
            Path element = new Path()
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Gray,
                StrokeThickness = 2
            };

            RectangleGeometry rectGeometry0 = new RectangleGeometry();
            rectGeometry0.Rect = new Rect(-50, -75, 100, 150);
            geometryGroup.Children.Add(rectGeometry0);

            RectangleGeometry rectGeometry1 = new RectangleGeometry();
            rectGeometry1.Rect = new Rect(-44, -70, 40, 30);
            geometryGroup.Children.Add(rectGeometry1);

            RectangleGeometry rectGeometry2 = new RectangleGeometry();
            rectGeometry2.Rect = new Rect(4, -70, 40, 30);
            geometryGroup.Children.Add(rectGeometry2);

            element.Data = geometryGroup;
            this.shape = element;

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