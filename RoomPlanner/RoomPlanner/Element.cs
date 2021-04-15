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