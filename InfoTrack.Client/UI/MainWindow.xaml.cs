using InfoTrack.Client.ViewModels;
using System.Windows;

namespace InfoTrack.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ((MainWindowVM)this.DataContext).VMWindow = this;
        }
    }
}