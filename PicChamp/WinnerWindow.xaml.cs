using System;
using System.Collections.Generic;
using System.IO;
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

namespace PicChamp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WinnerWindow : Window
    {
        ImageSourceConverter converter;

        public WinnerWindow(string winnerPath, MainWindow mainWindow)
        {
            mainWindow.Close();

            converter = new ImageSourceConverter();
            InitializeComponent();

            this.WindowState = WindowState.Maximized;

            image_winner.Source = (ImageSource)converter.ConvertFromString(winnerPath);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
