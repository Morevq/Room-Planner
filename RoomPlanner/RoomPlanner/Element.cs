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
    [Serializable]
    abstract public class Element // родительский класс для элементов интерьера
    {
        protected int width;
        protected int height;
        public RotateTransform rotate; // поле для хранения угла объекта
        public Shape shape; // поле для хранения ссылки на графический объект
        public bool isSelected = false;

        public virtual int Width
        {
            set
            {
                if (value < 1)
                {
                    MessageBox.Show("Введите корректное значение");
                    MainWindow.instance.ObjWidth.Text = Convert.ToString(width);
                }
                else width = value;
            }
            get
            {
                return width;
            }
        }
        public virtual int Height
        {
            set
            {
                if (value < 1)
                {
                    MessageBox.Show("Введите корректное значение");
                    MainWindow.instance.ObjHeight.Text = Convert.ToString(height);
                }
                else height = value;
            }
            get
            {
                return height;
            }
        }

        /// <summary>
        /// Изменение размеров объекта
        /// </summary>
        public abstract void Resize();

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
            rotate = new RotateTransform();
            element.RenderTransform = rotate;
            shape = element;

            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);

            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
            MainWindow.instance.WorkTable.Children.Add(element);
        }

        public override void Resize()
        {
            Room room = new Room(Width, Height);
            Canvas.SetLeft(room.shape, Canvas.GetLeft(shape));
            Canvas.SetTop(room.shape, Canvas.GetTop(shape));
            MainWindow.instance.WorkTable.Children.Remove(shape);
            MainWindow.instance.elements.Remove(this);
            MainWindow.instance.lockedElement = room;
            MainWindow.instance.lockedElement.isSelected = true;
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
            rotate = new RotateTransform();
            element.RenderTransform = rotate;
            shape = element;
            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            MainWindow.instance.WorkTable.Children.Add(element);
            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
        }

        public override void Resize()
        {
            Wardrobe wardrobe = new Wardrobe(Width, Height);
            Canvas.SetLeft(wardrobe.shape, Canvas.GetLeft(shape));
            Canvas.SetTop(wardrobe.shape, Canvas.GetTop(shape));
            MainWindow.instance.WorkTable.Children.Remove(shape);
            MainWindow.instance.elements.Remove(this);
            MainWindow.instance.lockedElement = wardrobe;
            MainWindow.instance.lockedElement.isSelected = true;
        }
    }

    [Serializable]
    public class Door : Element
    {
        public Door(int width = 75, int height = 75)
        {
            Width = width;
            Height = height;

            PathGeometry pathGeom = new PathGeometry();
            Path element = new Path();

            LineSegment vertLS = new LineSegment();
            PathFigure vertPF = new PathFigure();
            vertPF.StartPoint = new Point(Width, Height);
            vertLS.Point = new Point(0, Height);
            vertPF.Segments.Add(vertLS);
            pathGeom.Figures.Add(vertPF);

            LineSegment horLS = new LineSegment();
            PathFigure horPF = new PathFigure();
            horPF.StartPoint = new Point(Width, Height);
            horLS.Point = new Point(Width, 0);
            horPF.Segments.Add(horLS);
            pathGeom.Figures.Add(horPF);

            ArcSegment arc = new ArcSegment();
            PathFigure arcfrst = new PathFigure();
            arc.Point = new Point(0, Height);
            arcfrst.StartPoint = new Point(Width, 0);
            arc.Size = new Size(Width * 4 / 3, Height * 4 / 3);
            arcfrst.IsClosed = false;
            arcfrst.Segments.Add(arc);
            pathGeom.Figures.Add(arcfrst);

            element.Data = pathGeom;
            element.Stroke = Brushes.Gray;
            element.StrokeThickness = 10;
            MainWindow.instance.WorkTable.Children.Add(element);

            rotate = new RotateTransform();
            element.RenderTransform = rotate;

            shape = element;

            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);

            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
        }

        public override void Resize()
        {
            Door door = new Door(Width, Height);
            Canvas.SetLeft(door.shape, Canvas.GetLeft(shape));
            Canvas.SetTop(door.shape, Canvas.GetTop(shape));
            MainWindow.instance.WorkTable.Children.Remove(shape);
            MainWindow.instance.elements.Remove(this);
            MainWindow.instance.lockedElement = door;
            MainWindow.instance.lockedElement.isSelected = true;
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
    public class Casement : Element
    {
        public Casement(int width = 100, int height = 10)
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
            rotate = new RotateTransform();
            element.RenderTransform = rotate;

            shape = element;

            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);

            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
            MainWindow.instance.WorkTable.Children.Add(element);
        }

        public override void Resize()
        {
            Casement casement = new Casement(Width, Height);
            Canvas.SetLeft(casement.shape, Canvas.GetLeft(shape));
            Canvas.SetTop(casement.shape, Canvas.GetTop(shape));
            MainWindow.instance.WorkTable.Children.Remove(shape);
            MainWindow.instance.elements.Remove(this);
            MainWindow.instance.lockedElement = casement;
            MainWindow.instance.lockedElement.isSelected = true;
        }
    }

    [Serializable]
    public class Sofa : Element
    {
        public Sofa(int width = 80, int height = 160)
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

            RectangleGeometry rectGeometry0 = new RectangleGeometry();
            rectGeometry0.Rect = new Rect(Width * 0.25, Height *0.5, Width * 0.75, Height * 0.375);  //средняя нижняя подушка
            geometryGroup.Children.Add(rectGeometry0);

            RectangleGeometry rectGeometry1 = new RectangleGeometry();
            rectGeometry1.Rect = new Rect(Width * 0.25, Height * 0.125, Width * 0.75, Height * 0.375);  //средняя верхняя подушка
            geometryGroup.Children.Add(rectGeometry1);

            RectangleGeometry rectGeometry2 = new RectangleGeometry();
            rectGeometry2.Rect = new Rect(0, Height * 0.875, Width, Height *0.125);  //нижний подлокотник
            geometryGroup.Children.Add(rectGeometry2);

            RectangleGeometry rectGeometry3 = new RectangleGeometry();
            rectGeometry3.Rect = new Rect(0, 0, Width, Height * 0.125);  //верхний подлокотник
            geometryGroup.Children.Add(rectGeometry3);

            RectangleGeometry rectGeometry4 = new RectangleGeometry();
            rectGeometry4.Rect = new Rect(0, Height * 0.125, Width * 0.25, Height * 0.75);  //спинка
            geometryGroup.Children.Add(rectGeometry4);

            rotate = new RotateTransform();
            element.RenderTransform = rotate;

            element.Data = geometryGroup;
            shape = element;

            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            MainWindow.instance.WorkTable.Children.Add(element);

            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
        }

        public override void Resize()
        {
            Sofa sofa = new Sofa(Width, Height);
            Canvas.SetLeft(sofa.shape, Canvas.GetLeft(shape));
            Canvas.SetTop(sofa.shape, Canvas.GetTop(shape));
            MainWindow.instance.WorkTable.Children.Remove(shape);
            MainWindow.instance.elements.Remove(this);
            MainWindow.instance.lockedElement = sofa;
            MainWindow.instance.lockedElement.isSelected = true;
        }
    }

    [Serializable]
    public class Bath : Element
    {
        public Bath(int width = 50, int height = 100)
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

            RectangleGeometry rectGeometry0 = new RectangleGeometry();
            rectGeometry0.Rect = new Rect(0, 0, Width, Height);
            geometryGroup.Children.Add(rectGeometry0);

            RectangleGeometry rectGeometry1 = new RectangleGeometry();
            rectGeometry1.Rect = new Rect(Width * 0.1, Height * 0.05, Width * 0.8, Height * 0.9);
            geometryGroup.Children.Add(rectGeometry1);

            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = new Point(Width * 0.5, Height * 0.15);
            myEllipseGeometry.RadiusX = 3;
            myEllipseGeometry.RadiusY = 3;
            geometryGroup.Children.Add(myEllipseGeometry);

            rotate = new RotateTransform();
            element.RenderTransform = rotate;

            element.Data = geometryGroup;
            this.shape = element;

            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            MainWindow.instance.WorkTable.Children.Add(element);

            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
        }

        public override void Resize()
        {
            Bath bath = new Bath(Width, Height);
            Canvas.SetLeft(bath.shape, Canvas.GetLeft(shape));
            Canvas.SetTop(bath.shape, Canvas.GetTop(shape));
            MainWindow.instance.WorkTable.Children.Remove(shape);
            MainWindow.instance.elements.Remove(this);
            MainWindow.instance.lockedElement = bath;
            MainWindow.instance.lockedElement.isSelected = true;
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
            rotate = new RotateTransform();
            element.RenderTransform = rotate;

            shape = element;
            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            MainWindow.instance.WorkTable.Children.Add(element);
            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
        }

        public override void Resize()
        {
            Desk desk = new Desk(Width, Height);
            Canvas.SetLeft(desk.shape, Canvas.GetLeft(shape));
            Canvas.SetTop(desk.shape, Canvas.GetTop(shape));
            MainWindow.instance.WorkTable.Children.Remove(shape);
            MainWindow.instance.elements.Remove(this);
            MainWindow.instance.lockedElement = desk;
            MainWindow.instance.lockedElement.isSelected = true;
        }
    }

    [Serializable]
    public class Sink : Element
    {
        public Sink(int width = 40, int height = 50)
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

            RectangleGeometry rectGeometry0 = new RectangleGeometry();
            rectGeometry0.Rect = new Rect(0, 0, Width, Height);
            geometryGroup.Children.Add(rectGeometry0);

            RectangleGeometry rectGeometry1 = new RectangleGeometry();
            rectGeometry1.Rect = new Rect(Width * 0.125, Height * 0.1, Width * 0.75, Height * 0.8);
            geometryGroup.Children.Add(rectGeometry1);

            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = new Point(Width * 0.375, Height *0.5);
            myEllipseGeometry.RadiusX = 3;
            myEllipseGeometry.RadiusY = 3;
            geometryGroup.Children.Add(myEllipseGeometry);

            rotate = new RotateTransform();
            element.RenderTransform = rotate;

            element.Data = geometryGroup;
            this.shape = element;

            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            MainWindow.instance.WorkTable.Children.Add(element);

            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
        }

        public override void Resize()
        {
            Sink sink = new Sink(Width, Height);
            Canvas.SetLeft(sink.shape, Canvas.GetLeft(shape));
            Canvas.SetTop(sink.shape, Canvas.GetTop(shape));
            MainWindow.instance.WorkTable.Children.Remove(shape);
            MainWindow.instance.elements.Remove(this);
            MainWindow.instance.lockedElement = sink;
            MainWindow.instance.lockedElement.isSelected = true;
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
            rotate = new RotateTransform();
            element.RenderTransform = rotate;

            shape = element;
            Canvas.SetLeft(element, (MainWindow.instance.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (MainWindow.instance.WorkTable.ActualHeight - height) / 2);
            element.MouseLeftButtonDown += LeftMouseDown;
            element.MouseLeftButtonUp += LeftMouseUp;
            element.MouseRightButtonUp += RightMouseUp;
            MainWindow.instance.WorkTable.Children.Add(element);
        }

        public override void Resize()
        {
            Tv tv = new Tv(Width, Height);
            Canvas.SetLeft(tv.shape, Canvas.GetLeft(shape));
            Canvas.SetTop(tv.shape, Canvas.GetTop(shape));
            MainWindow.instance.WorkTable.Children.Remove(shape);
            MainWindow.instance.elements.Remove(this);
            MainWindow.instance.lockedElement = tv;
            MainWindow.instance.lockedElement.isSelected = true;
        }
    }
}