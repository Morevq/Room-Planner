﻿using System;
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

    [Serializable]
    abstract public class Element
    {
        protected int width;
        protected int height;
        public RotateTransform rotate;
        public Shape shape;
        public bool isSelected = false;

        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        /// <summary>
        /// Нажатие ЛКМ по объекту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isSelected == true)
            {
                MainWindow.instance.MouseMove += MainWindow.instance.Window_MouseMove;

                MainWindow.instance.ttop = Canvas.GetTop(shape);
                MainWindow.instance.lleft = Canvas.GetLeft(shape);
                Point point = e.GetPosition(MainWindow.instance.WorkTable);
                MainWindow.instance.deltaX = point.X;
                MainWindow.instance.deltaY = point.Y;
            }
        }

        /// <summary>
        /// Отпускание ЛКМ на объекте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isSelected == false)
            {
                if (MainWindow.instance.lockedElement == null) MainWindow.instance.lockedElement = this;
                else
                {
                    MainWindow.instance.lockedElement.isSelected = false;
                    MainWindow.instance.lockedElement = this;
                }
                isSelected = true;
                MainWindow.instance.PropertyList.Visibility = Visibility.Visible;
                MainWindow.instance.ObjHeight.Text = Convert.ToString(MainWindow.instance.lockedElement.Height);
                MainWindow.instance.ObjWidth.Text = Convert.ToString(MainWindow.instance.lockedElement.Width);
                MainWindow.instance.ObjAngle.Text = Convert.ToString(MainWindow.instance.lockedElement.rotate.Angle);
            }
            else
            {
                MainWindow.instance.MouseMove -= MainWindow.instance.Window_MouseMove;
            }
        }

        /// <summary>
        /// Удаление объекта на ПКМ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RightMouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.instance.WorkTable.Children.Remove(shape);
            MainWindow.instance.elements.Remove(this);
            MainWindow.instance.lockedElement = null;
            MainWindow.instance.PropertyList.Visibility = Visibility.Hidden;
        }

        public virtual void Resize() { }
    }

    [Serializable]
    public class Room : Element
    {
        public Room(int width = 600, int height = 600)
        {
            Width = width;
            Height = height;

            Rectangle element = new Rectangle()
            {
                Width = Width,
                Height = Height,
                Stroke = Brushes.Black,
                StrokeThickness = 20
            };
            shape = element;
            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
            MainWindow.instance.WorkTable.Children.Add(element);
        }
    }

    [Serializable]
    public class Wardrobe : Element
    {
        public Wardrobe(int width = 100, int height = 50)
        {
            Width = width;
            Height = height;
            
            Rectangle element = new Rectangle()
            {
                Width = Width,
                Height = Height,
                Stroke = Brushes.Black,
                Fill = Brushes.Gray,
                StrokeThickness = 2
            };
            shape = element;
            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            MainWindow.instance.WorkTable.Children.Add(element);
            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
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

    [Serializable]
    public class Door : Element
    {
        public Door()
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
            element.Stroke = Brushes.Gray;
            element.StrokeThickness = 10;
            MainWindow.instance.WorkTable.Children.Add(element);
            this.shape = element;

            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);

            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
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

    [Serializable]
    public class Bed : Element
    {
        public Bed(int width = 100, int height = 150)
        {
            Width = width;
            Height = height;

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.FillRule = FillRule.Nonzero;

            Path element = new Path()
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Gray,
                StrokeThickness = 2
            };

            RectangleGeometry rectGeometry = new RectangleGeometry();
            rectGeometry.Rect = new Rect(0, 0, Width, Height);
            geometryGroup.Children.Add(rectGeometry);

            rectGeometry = new RectangleGeometry();
            rectGeometry.Rect = new Rect(0.06 * Width, 0.0333 * Height, 0.4 * Width, 0.2 * Height); //6 5 40 30
            geometryGroup.Children.Add(rectGeometry);

            rectGeometry = new RectangleGeometry();
            rectGeometry.Rect = new Rect(0.54 * Width, 0.0333 * Height, 0.4 * Width, 0.2 * Height); //54 5 40 30
            geometryGroup.Children.Add(rectGeometry);

            rotate = new RotateTransform();
            element.RenderTransform = rotate;

            element.Data = geometryGroup;
            shape = element;

            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - Width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - Height) / 2);
            MainWindow.instance.WorkTable.Children.Add(element);

            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
        }

        public override void Resize()
        {
            Bed bed = new Bed(Width, Height);
            Canvas.SetLeft(bed.shape, Canvas.GetLeft(shape));
            Canvas.SetTop(bed.shape, Canvas.GetTop(shape));
            MainWindow.instance.WorkTable.Children.Remove(shape);
            MainWindow.instance.elements.Remove(this);
            MainWindow.instance.lockedElement = bed;
            MainWindow.instance.lockedElement.isSelected = true;
        }
    }

    [Serializable]
    public class Сasement : Element
    {
        public Сasement(int width = 100, int height = 10)
        {
            Width = width;
            Height = height;
            

            Rectangle element = new Rectangle()
            {
                Width = Width,
                Height = Height,
                Fill = Brushes.Gray,
                Stroke = Brushes.White,
                StrokeThickness = 2
            };
            shape = element;
            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
            MainWindow.instance.WorkTable.Children.Add(element);
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

    [Serializable]
    public class Sofa : Element
    {
        public Sofa()
        {
            

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.FillRule = FillRule.Nonzero;
            Path element = new Path()
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Gray,
                StrokeThickness = 2
            };

            RectangleGeometry rectGeometry0 = new RectangleGeometry();
            rectGeometry0.Rect = new Rect(0, 0, 60, 60);
            geometryGroup.Children.Add(rectGeometry0);

            RectangleGeometry rectGeometry1 = new RectangleGeometry();
            rectGeometry1.Rect = new Rect(0, -60, 60, 60);
            geometryGroup.Children.Add(rectGeometry1);

            RectangleGeometry rectGeometry2 = new RectangleGeometry();
            rectGeometry2.Rect = new Rect(-20, 60, 80, 20);
            geometryGroup.Children.Add(rectGeometry2);

            RectangleGeometry rectGeometry3 = new RectangleGeometry();
            rectGeometry3.Rect = new Rect(-20, -80, 80, 20);
            geometryGroup.Children.Add(rectGeometry3);

            RectangleGeometry rectGeometry4 = new RectangleGeometry();
            rectGeometry4.Rect = new Rect(-20, -60, 20, 60);
            geometryGroup.Children.Add(rectGeometry4);

            RectangleGeometry rectGeometry5 = new RectangleGeometry();
            rectGeometry5.Rect = new Rect(-20, 0, 20, 60);
            geometryGroup.Children.Add(rectGeometry5);

            element.Data = geometryGroup;
            this.shape = element;

            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            MainWindow.instance.WorkTable.Children.Add(element);

            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
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

    [Serializable]
    public class Bath : Element
    {
        public Bath()
        {
            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.FillRule = FillRule.Nonzero;
            Path element = new Path()
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Gray,
                StrokeThickness = 2
            };

            RectangleGeometry rectGeometry0 = new RectangleGeometry();
            rectGeometry0.Rect = new Rect(-50, -75, 100, 50);
            geometryGroup.Children.Add(rectGeometry0);

            RectangleGeometry rectGeometry1 = new RectangleGeometry();
            rectGeometry1.Rect = new Rect(-45, -70, 90, 40);
            geometryGroup.Children.Add(rectGeometry1);

            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = new Point(30, -50);
            myEllipseGeometry.RadiusX = 3;
            myEllipseGeometry.RadiusY = 3;
            geometryGroup.Children.Add(myEllipseGeometry);

            element.Data = geometryGroup;
            this.shape = element;

            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            MainWindow.instance.WorkTable.Children.Add(element);

            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
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

    [Serializable]
    public class Desk : Element
    {
        public Desk(int width = 100, int height = 50)
        {
            Width = width;
            Height = height;
            
            Rectangle element = new Rectangle()
            {
                Width = Width,
                Height = Height,
                Stroke = Brushes.Black,
                Fill = Brushes.Gray,
                StrokeThickness = 2
            };
            shape = element;
            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            MainWindow.instance.WorkTable.Children.Add(element);
            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
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

    [Serializable]
    public class Sink : Element
    {
        public Sink()
        {
            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.FillRule = FillRule.Nonzero;
            Path element = new Path()
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Gray,
                StrokeThickness = 2
            };

            RectangleGeometry rectGeometry0 = new RectangleGeometry();
            rectGeometry0.Rect = new Rect(4, -63, 50, 40);
            geometryGroup.Children.Add(rectGeometry0);

            RectangleGeometry rectGeometry1 = new RectangleGeometry();
            rectGeometry1.Rect = new Rect(9, -58, 40, 30);
            geometryGroup.Children.Add(rectGeometry1);

            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = new Point(30, -50);
            myEllipseGeometry.RadiusX = 3;
            myEllipseGeometry.RadiusY = 3;
            geometryGroup.Children.Add(myEllipseGeometry);

            element.Data = geometryGroup;
            this.shape = element;

            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            MainWindow.instance.WorkTable.Children.Add(element);

            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
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

    [Serializable]
    public class Tv : Element
    {
        public Tv(int width = 100, int height = 10)
        {
            Width = width;
            Height = height;
            

            Rectangle element = new Rectangle()
            {
                Width = Width,
                Height = Height,
                Fill = Brushes.Gray,
                Stroke = Brushes.White,
                StrokeThickness = 2
            };
            shape = element;
            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
            MainWindow.instance.WorkTable.Children.Add(element);
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