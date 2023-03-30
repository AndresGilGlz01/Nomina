using Nomina.Helpers;
using Nomina.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Nomina.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public static NominaContext Context { get; set; } = new();

    public IViewModel ViewModelActual
    {
        get => _viewmodelactual;
        set {
            _viewmodelactual = value;
            _viewmodelactual.Actualizar();
            Notificar();
        }
    }

    private readonly CategoriaViewModel _categoriaviewmodel = new();
    private readonly EmpleadoViewModel _empleadoviewmodel = new();
    private IViewModel _viewmodelactual;


    private void ActualizarEmpleados()
    {
        _empleadoviewmodel.Actualizar();
    }

    public ICommand NavegarEmpleadoCommand { get; set; }
    public ICommand NavegarCategoriaCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainViewModel()
    {
        NavegarEmpleadoCommand = new RelayCommand(NavegarEmpleado);
        NavegarCategoriaCommand = new RelayCommand(NavegarCategoria);
        ViewModelActual = _empleadoviewmodel;
        _categoriaviewmodel.ActualizarEmpleados = ActualizarEmpleados;
    }

    private void NavegarCategoria()
    {
        ViewModelActual = _categoriaviewmodel;
    }

    private void NavegarEmpleado()
    {
        ViewModelActual = _empleadoviewmodel;
    }

    void Notificar([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
