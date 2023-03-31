using Nomina.Helpers;
using Nomina.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Nomina.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    #region fields
    private readonly CategoriaViewModel _categoriaviewmodel = new(Context);
    private readonly EmpleadoViewModel _empleadoviewmodel = new(Context);
    private IViewModel? _viewmodelactual;
    #endregion

    #region properties
    public static NominaContext Context { get; set; } = new();

    public IViewModel ViewModelActual
    {
        get => _viewmodelactual ?? new EmpleadoViewModel(Context);
        set {
            _viewmodelactual = value;
            _viewmodelactual.Actualizar();
            Notificar();
        }
    }
    #endregion

    #region commands
    public ICommand NavegarEmpleadoCommand { get; set; }
    public ICommand NavegarCategoriaCommand { get; set; }
    #endregion

    #region events
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion

    public MainViewModel()
    {
        NavegarEmpleadoCommand = new RelayCommand(NavegarEmpleado);
        NavegarCategoriaCommand = new RelayCommand(NavegarCategoria);
        ViewModelActual = _empleadoviewmodel;
        _categoriaviewmodel.ActualizarEmpleados = ActualizarEmpleados;
        _empleadoviewmodel.ActualizarCategoria = ActualizarCategoria;
    }

    #region methods
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
    #endregion
}
