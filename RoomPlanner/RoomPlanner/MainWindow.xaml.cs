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
        Rectangle room;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            room = new Rectangle()
            {
                Width = 600,
                Height = 600,
                Stroke = Brushes.Black,
                StrokeThickness = 20
            };
            Canvas.SetLeft(room, (WorkTable.ActualWidth - 600) / 2);
            Canvas.SetTop(room, (WorkTable.ActualHeight - 600) / 2);
            room.AddHandler(Rectangle.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Rectangle_MouseLeftButtonUp));
            WorkTable.Children.Add(room);
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = (Rectangle)sender;
            PropertyList.Visibility = Visibility.Visible;
            ObjHeight.Text = Convert.ToString(rectangle.Height);
            ObjWidth.Text = Convert.ToString(rectangle.Width);
        }

        private void ObjHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Background = Convert.ToString(room.Height) != textBox.Text ?
                Brushes.LightGreen : Brushes.White;
        }

        private void ObjWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Background = Convert.ToString(room.Width) != textBox.Text ? 
                Brushes.LightGreen : Brushes.White;
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            room.Height = Convert.ToDouble(ObjHeight.Text);
            room.Width = Convert.ToDouble(ObjWidth.Text);
            ObjHeight.Background = Brushes.White;
            ObjWidth.Background = Brushes.White;
            PropertyList.Visibility = Visibility.Hidden;
        }
    }
}
