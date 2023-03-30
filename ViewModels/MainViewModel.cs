using Nomina.Helpers;
using Nomina.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Nomina.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public static NominaContext Context { get; set; } = new();
    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand NavegarEmpleadoCommand { get; set; }
    public ICommand NavegarCategoriaCommand { get; set; }

    public IViewModel ViewModelActual
    {
        get => _viewmodelactual ?? new EmpleadoViewModel();
        set {
            _viewmodelactual = value;
            _viewmodelactual.Actualizar();
            Notificar();
        }
    }

    private readonly CategoriaViewModel _categoriaviewmodel = new();
    private readonly EmpleadoViewModel _empleadoviewmodel = new();
    private IViewModel? _viewmodelactual;

    public MainViewModel()
    {
        NavegarEmpleadoCommand = new RelayCommand(NavegarEmpleado);
        NavegarCategoriaCommand = new RelayCommand(NavegarCategoria);
        ViewModelActual = _empleadoviewmodel;
        _categoriaviewmodel.ActualizarEmpleados = ActualizarEmpleados;
        _empleadoviewmodel.ActualizarCategoria = ActualizarCategoria;
    }

    void NavegarCategoria()
    {
        ViewModelActual = _categoriaviewmodel;
    }

    void NavegarEmpleado()
    {
        ViewModelActual = _empleadoviewmodel;
    }

    void ActualizarEmpleados()
    {
        _empleadoviewmodel.Actualizar();
    }

    void ActualizarCategoria(Categoria entity)
    {
        Context.Entry(entity).Reload();
        _categoriaviewmodel.Actualizar();
    }

    void Notificar([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
