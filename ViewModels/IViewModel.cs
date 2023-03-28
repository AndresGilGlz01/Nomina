using Nomina.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Nomina.ViewModels;
public interface IViewModel : INotifyPropertyChanged
{
    Operacion Operacion { get; set; }
    string Errores { get; set; }
    string Error { get; set; }

    ICommand VerRegistrarCommand { get; set; }
    ICommand VerModificarCommand { get; set; }
    ICommand VerEliminarCommand { get; set; }
    ICommand RegistrarCommand { get; set; }
    ICommand EliminarCommand { get; set; }
    ICommand RegresarCommand { get; set; }
    ICommand FiltrarCommand { get; set; }

    event PropertyChangedEventHandler? PropertyChanged;

    void Actualizar();
    void Notificar([CallerMemberName] string? propertyName = null);
}
