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
            FrameworkElement selectedElement = (FrameworkElement)sender;
            mainWindow.PropertyList.Visibility = Visibility.Visible;
            mainWindow.ObjHeight.Text = Convert.ToString(selectedElement.Height);
            mainWindow.ObjWidth.Text = Convert.ToString(selectedElement.Width);
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

            Point point = e.GetPosition(WorkTable);
            deltaX = point.X;
            deltaY = point.Y;
        }

        public void DeclineObject(object sender, MouseButtonEventArgs e)
        {
            currobj = null;
        }


        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(WorkTable);
            if (currobj != null)
            {
                Canvas.SetTop(currobj, point.Y - deltaY + ttop);
                Canvas.SetLeft(currobj, point.X - deltaX + lleft);

                Console.WriteLine("x = " + point.X.ToString() + "  + delX = " + (point.X + deltaX) + "  top=" + ttop.ToString() + "  left=" + lleft.ToString());
                //Console.WriteLine(currobj.Margin.Top.ToString() + " " + currobj.Margin.Left.ToString() + " " + point.X.ToString() + " " + point.Y.ToString() + " ");
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
            Canvas.SetLeft(element, (mainWindow.WorkTable.ActualWidth - width) / 2);
            Canvas.SetTop(element, (mainWindow.WorkTable.ActualHeight - height) / 2);
            element.AddHandler(Rectangle.MouseLeftButtonUpEvent, new MouseButtonEventHandler(ClickOnElement));
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
            element.AddHandler(Rectangle.MouseLeftButtonUpEvent, new MouseButtonEventHandler(ClickOnElement));
            mainWindow.WorkTable.Children.Add(element);
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
}