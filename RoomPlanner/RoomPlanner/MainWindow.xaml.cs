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
        public MainWindow()
        {
            InitializeComponent();
        }

        public Element lockedElement;
        public double deltaX, deltaY, lleft, ttop;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "CreateRoomButton":
                    Room room = new Room(this);
                    break;
                case "CreateWardrobeButton":
                    Wardrobe wardrobe = new Wardrobe(this);
                    break;
                case "CreateDoorButton":
                    Door door = new Door(this);
                    break;
                case "CreateBedButton":
                    Bed bed = new Bed(this);
                    break;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(WorkTable);
            if (lockedElement != null)
            {
                Canvas.SetTop(lockedElement.frameworkElement, point.Y - deltaY + ttop);
                Canvas.SetLeft(lockedElement.frameworkElement, point.X - deltaX + lleft);

                Console.WriteLine("x = " + point.X.ToString() + "  + delX = " + (point.X + deltaX) +
                    "  top=" + ttop.ToString() + "  left=" + lleft.ToString());
            }
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            lockedElement.Width = Convert.ToInt32(ObjWidth.Text);
        }
    }
}