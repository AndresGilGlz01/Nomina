using Nomina.Helpers;
using Nomina.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Nomina.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly CategoriaViewModel _categoriaviewmodel;
    private readonly EmpleadoViewModel _empleadoviewmodel;
    private IViewModel _viewmodelactual;
    private NominaContext _context;

    public IViewModel ViewModelActual
    {
        get => _viewmodelactual ?? new EmpleadoViewModel(_context);
        set {
            _viewmodelactual = value;
            _viewmodelactual.Actualizar();
            Notificar();
        }
    }

    public ICommand NavegarEmpleadoCommand { get; set; }
    public ICommand NavegarCategoriaCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainViewModel(NominaContext context)
    {
        _context = context;
        NavegarEmpleadoCommand = new RelayCommand(NavegarEmpleado);
        NavegarCategoriaCommand = new RelayCommand(NavegarCategoria);

        _categoriaviewmodel = new(_context);
        _empleadoviewmodel = new(_context);

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
        _context.Entry(entity).Reload();
        _categoriaviewmodel.Actualizar();
    }

    void Notificar([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
