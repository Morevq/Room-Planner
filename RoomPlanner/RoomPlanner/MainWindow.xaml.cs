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

        public Element lockedElement; //текущий выбранный объект
        public double deltaX, deltaY, lleft, ttop; //поля для корректного перетаскивания объектов

        private void Button_Click(object sender, RoutedEventArgs e) //обработчик события нажатия на кнопки из списка объектов слева
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

                case "CreateСasementButton":
                    Сasement casement = new Сasement(this);
                    break;
                case "CreateSofaButton":
                    Sofa sofa = new Sofa(this);
                    break;
                case "CreateBathButton":
                    Bath bath = new Bath(this);
                    break;
                case "CreateDeskButton":
                    Desk desk = new Desk(this);
                    break;
                case "CreateSinkButton":
                    Sink sink = new Sink(this);
                    break;
                case "CreateTvButton":
                    Tv tv = new Tv(this);
                    break;
            }
        }

        public void Window_MouseMove(object sender, MouseEventArgs e) //реализия перетаскивания объектов
        {
            Point point = e.GetPosition(WorkTable);
            if (lockedElement != null)
            {
                Canvas.SetTop(lockedElement.shape, point.Y - deltaY + ttop);
                Canvas.SetLeft(lockedElement.shape, point.X - deltaX + lleft);

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