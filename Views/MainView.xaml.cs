using Nomina.Models;
using Nomina.ViewModels;
using System.Windows;

namespace Nomina.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private NominaContext _context;

        public MainView(NominaContext context)
        {
            InitializeComponent();
            _context = context;
            DataContext = new MainViewModel(_context);
        }
    }
}
