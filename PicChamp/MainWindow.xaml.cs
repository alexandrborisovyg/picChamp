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
    public partial class MainWindow : Window
    {
        Random rand;
        ImageSourceConverter converter;

        static string basePath = @"C:\";
        static List<string> currentRound;
        static List<string> nextRound;
        int randomImageIndex;

        public MainWindow()
        {
            // Initializing

            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                basePath = dialog.SelectedPath;
            }

            converter = new ImageSourceConverter();
            rand = new Random();
            currentRound = Directory.GetFiles(basePath).ToList();
            nextRound = new List<string>();

            InitializeComponent();

            Application.Current.MainWindow.WindowState = WindowState.Maximized;

            UpdateState();
        }

        private void image_1_competator_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChooseImage(sender, true);
        }

        private void image_2_competator_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChooseImage(sender, false);
        }

        private void UpdateState()
        {
            randomImageIndex = rand.Next(0, currentRound.Count);
            this.image_1_competator.Source = (ImageSource)converter.ConvertFromString(currentRound[randomImageIndex]);

            int randomImageIndex2 = -1;
            do
            {
                randomImageIndex2 = rand.Next(0, currentRound.Count);
            }
            while (randomImageIndex2 == randomImageIndex);
            this.image_2_competator.Source = (ImageSource)converter.ConvertFromString(currentRound[randomImageIndex2]);

            label_count_currenRound.Content = currentRound.Count;
            label_count_nextRound.Content = nextRound.Count;
        }

        private void ChooseImage(object sender, bool isLeftImage)
        {
            if (currentRound.Count > 1)
            {
                string imagePath = (sender as Image).Source.ToString().Remove(0, 8).Replace("/", @"\");
                nextRound.Add(imagePath);

                int removeImageIndex = currentRound.IndexOf(imagePath);
                currentRound.RemoveAt(removeImageIndex);

                string removeImagePath;
                if (isLeftImage)
                    removeImagePath = image_2_competator.Source.ToString().Remove(0, 8).Replace("/", @"\");
                else
                    removeImagePath = image_1_competator.Source.ToString().Remove(0, 8).Replace("/", @"\");

                removeImageIndex = currentRound.IndexOf(removeImagePath);
                currentRound.RemoveAt(removeImageIndex);
            }

            if (currentRound.Count <= 1)
            {
                if (currentRound.Count == 1)
                {
                    nextRound.Add(currentRound.FirstOrDefault());
                }

                currentRound = nextRound.OrderBy(x => Guid.NewGuid()).ToList();
                nextRound.Clear();

                if (currentRound.Count == 1)
                {
                    WinnerWindow newWindow = new WinnerWindow(currentRound.FirstOrDefault(), this);
                    newWindow.ShowDialog();
                }

                label_count_currenRound.Content = currentRound.Count;
                label_count_nextRound.Content = nextRound.Count;
                MessageBox.Show("Next round");
            }

            UpdateState();
        }
    }
}
