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

        private void CreateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Rectangle room = new Rectangle()
            {
                Width = 900,
                Height = 610,
                Stroke = Brushes.Black,
                StrokeThickness = 10
            };
            Canvas.SetLeft(room, (WorkTable.ActualWidth - 900) / 2);
            Canvas.SetTop(room, (WorkTable.ActualHeight - 610) / 2);
            WorkTable.Children.Add(room);
        }
    }
}
