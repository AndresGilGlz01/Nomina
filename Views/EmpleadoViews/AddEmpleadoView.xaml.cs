using Google.Protobuf.Collections;
using Nomina.ViewModels;
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

namespace Nomina.Views.EmpleadoViews
{
    /// <summary>
    /// Interaction logic for AddEmpleadoView.xaml
    /// </summary>
    public partial class AddEmpleadoView : UserControl
    {
        public AddEmpleadoView()
        {
            InitializeComponent();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if ((this.DataContext as EmpleadoViewModel)?.Operacion != Operacion.Update)
            {
                ((ComboBox)sender).SelectedIndex = 0;
            }
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            var x = (this.DataContext as EmpleadoViewModel);

            if (x.Operacion == Operacion.Create)
            {
                (sender as Button).Command = x.RegistrarCommand;
            }
            else if (x.Operacion == Operacion.Update)
            {
                (sender as Button).Command = x.ModificarCommand;
            }
        }
    }
}
