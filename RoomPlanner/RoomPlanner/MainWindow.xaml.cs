using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        public static MainWindow instance;
        public List<Element> elements;
        public string pathToFile;

        public MainWindow()
        {
            instance = this;
            elements = new List<Element>();
            InitializeComponent();
            MouseMove += Window_MouseMove;
        }

        public Element lockedElement; //текущий выбранный объект
        public double deltaX, deltaY, lleft, ttop; //поля для корректного перетаскивания объектов

        private void Button_Click(object sender, RoutedEventArgs e) //обработчик события нажатия на кнопки из списка объектов слева
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "CreateRoomButton":
                    Room room = new Room();
                    elements.Add(room);
                    break;
                case "CreateWardrobeButton":
                    Wardrobe wardrobe = new Wardrobe();
                    elements.Add(wardrobe);
                    break;
                case "CreateDoorButton":
                    Door door = new Door();
                    elements.Add(door);
                    break;
                case "CreateBedButton":
                    Bed bed = new Bed();
                    elements.Add(bed);
                    break;
                case "CreateСasementButton":
                    Сasement casement = new Сasement();
                    elements.Add(casement);
                    break;
                case "CreateSofaButton":
                    Sofa sofa = new Sofa();
                    elements.Add(sofa);
                    break;
                case "CreateBathButton":
                    Bath bath = new Bath();
                    elements.Add(bath);
                    break;
                case "CreateDeskButton":
                    Desk desk = new Desk();
                    elements.Add(desk);
                    break;
                case "CreateSinkButton":
                    Sink sink = new Sink();
                    elements.Add(sink);
                    break;
                case "CreateTvButton":
                    Tv tv = new Tv();
                    elements.Add(tv);
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
            //lockedElement.Width = Convert.ToInt32(ObjWidth.Text);
        }

        public void Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                pathToFile = saveFileDialog.FileName;
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(pathToFile, FileMode.Create, FileAccess.Write);
                binaryFormatter.Serialize(fileStream, elements);
                fileStream.Close();
            }
        }

        public void Load(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                pathToFile = openFileDialog.FileName;
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(pathToFile, FileMode.Open, FileAccess.Read);
                elements = (List<Element>)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
            }
        }

        public void New(object sender, RoutedEventArgs e)
        {
            elements.Clear();
        }
    }
}