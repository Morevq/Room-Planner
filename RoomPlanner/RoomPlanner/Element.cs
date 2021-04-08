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
    abstract public class Element // родительский класс для разработки элементов
    {                                   // элемент - объект на чертеже (прим.: комната, стул, стол)
        protected int width;
        protected int height;
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public bool isSelected = false;

        public MainWindow mainWindow;

        public void ClickOnElement(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement selectedElement = (FrameworkElement)sender;
            mainWindow.PropertyList.Visibility = Visibility.Visible;
            mainWindow.ObjHeight.Text = Convert.ToString(selectedElement.Height);
            mainWindow.ObjWidth.Text = Convert.ToString(selectedElement.Width);
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
            element.MouseLeftButtonUp += ClickOnElement;
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
}