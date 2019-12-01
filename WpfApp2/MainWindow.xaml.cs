using System.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var mLock = new deadlock.DeadLock(this);
            mLock.demonstrateDeadlock(false);

        }
    }
}
