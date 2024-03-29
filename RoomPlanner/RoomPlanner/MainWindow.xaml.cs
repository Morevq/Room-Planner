﻿using Microsoft.Win32;
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
            instance = this; // поле для хранения ссылки на окно
            elements = new List<Element>(); // список всех элементов
            InitializeComponent();
        }

        public Element lockedElement; //текущий выбранный объект
        public double deltaX, deltaY, lleft, ttop; //поля для корректного перетаскивания объектов

        /// <summary>
        /// Нажатие на кнопки в столбце "ресурсы"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "ClearButton":
                    elements.Clear();
                    break;
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
                    Casement casement = new Casement();
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

        /// <summary>
        /// Перемещение мебели по холсту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_MouseMove(object sender, MouseEventArgs e)
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

        /// <summary>
        /// Нажатие на кнопку "готово"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = true;

            try { lockedElement.Width = Convert.ToInt32(ObjWidth.Text); }
            catch
            {
                MessageBox.Show("Введите корректное значение");
                ObjWidth.Text = Convert.ToString(lockedElement.Width);
                success = false;
            }

            try { lockedElement.Height = Convert.ToInt32(ObjHeight.Text); }
            catch
            {
                MessageBox.Show("Введите корректное значение");
                ObjHeight.Text = Convert.ToString(lockedElement.Height);
                success = false;
            }

            if (success == true) lockedElement.Resize();

            lockedElement.rotate.Angle = Convert.ToInt32(ObjAngle.Text);
            lockedElement.rotate.CenterX = lockedElement.Width / 2;
            lockedElement.rotate.CenterY = lockedElement.Height / 2;

        }

        /// <summary>
        /// Нажатие на клавишу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                lockedElement.isSelected = false;
                lockedElement = null;
                PropertyList.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Приближение / отдаление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                ScaleTransform scale = new ScaleTransform(1, 1);
                WorkTable.RenderTransform = scale;
            }
            else
            {
                ScaleTransform scale = new ScaleTransform(0.5, 0.5);
                WorkTable.RenderTransform = scale;
            }
        }

        /// <summary>
        /// Создание файла сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Загрузка файла сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Очистка холста от мебели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void New(object sender, RoutedEventArgs e)
        {
            elements.Clear();
        }
    }
}