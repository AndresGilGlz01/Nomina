using Nomina.Helpers;
using Nomina.Models;
using Nomina.Repositories;
using Nomina.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Nomina.ViewModels;

public class EmpleadoViewModel : IViewModel
{
    #region fields
    private ObservableCollection<Categoria> _categorias = new();
    private ObservableCollection<Empleado> _empleados = new();
    private EmpleadoRepository _repository;
    private Empleado _empleado = new();
    private Empleado? _empleadotemp;
    private Operacion _operacion;
    private string _error = string.Empty;
    private string _errores = string.Empty;
    #endregion

    #region properties
    public Action<Categoria>? ActualizarCategoria { get; set; }

    public ObservableCollection<Empleado> Empleados
    {
        get => _empleados;
        set { _empleados = value; Notificar(); }
    }

    public int CantidadEmpleados
    {
        get { return _empleados.Count; }
    }

    public decimal PromedioSueldos
    {
        get { return _empleados.Average(emp => emp.Sueldo); }
    }

    public decimal TotalSueldos
    {
        get { return _empleados.Sum(emp => emp.Sueldo); }
    }

    public ObservableCollection<Categoria> Categorias
    {
        get => _categorias;
        set { _categorias = value; Notificar(); }
    }

    public Empleado Empleado
    {
        get => _empleado;
        set { _empleado = value; Notificar(); }
    }

    public Operacion Operacion
    {
        get => _operacion;
        set { _operacion = value; Notificar(); }
    }

    public string Errores
    {
        get => _errores;
        set { _errores = value; Notificar(); }
    }

    public string Error
    {
        get => _error;
        set { _error = value; Notificar(); }
    }
    #endregion

    #region Commands
    public ICommand VerRegistrarCommand { get; set; }
    public ICommand VerModificarCommand { get; set; }
    public ICommand VerEliminarCommand { get; set; }
    public ICommand ModificarCommand { get; set; }
    public ICommand RegistrarCommand { get; set; }
    public ICommand EliminarCommand { get; set; }
    public ICommand RegresarCommand { get; set; }
    public ICommand FiltrarCommand { get; set; }
    #endregion

    #region events
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion

    public EmpleadoViewModel(NominaContext context)
    {
        _repository = new(context);
        VerModificarCommand = new RelayCommand<int>(VerModificar);
        VerEliminarCommand = new RelayCommand<int>(VerEliminar);
        VerRegistrarCommand = new RelayCommand(VerRegistrar);
        RegistrarCommand = new RelayCommand(Registrar);
        ModificarCommand = new RelayCommand(Modificar);
        EliminarCommand = new RelayCommand(Eliminar);
        RegresarCommand = new RelayCommand(Regresar);
        FiltrarCommand = new RelayCommand<string>(Filtrar);
        Actualizar();
    }

    #region methods
    private void Filtrar(string pattern)
    {
        throw new NotImplementedException();
    }

    private void Regresar()
    {
        if (Operacion == Operacion.Update && _empleadotemp is not null)
        {
            Empleado.Id = _empleadotemp.Id;
            Empleado.Nombre = _empleadotemp.Nombre;
            Empleado.Sueldo = _empleadotemp.Sueldo;
            Empleado.Activo = _empleadotemp.Activo;
            Empleado.IdCategoria = _empleadotemp.IdCategoria;
            Notificar(nameof(Empleado));
        }
        Operacion = Operacion.View;
    }

    private void Eliminar()
    {
        if (Empleado is not null)
        {
            _repository.Delete(Empleado);
            ActualizarCategoria!(Empleado.IdCategoriaNavigation);
            Actualizar();
            Operacion = Operacion.View;
        }
    }

    private void Modificar()
    {
        Empleado = _repository.GetById(Empleado.Id);
        if (Empleado is not null)
        {
            _repository.Update(Empleado);
            Actualizar();
            Operacion = Operacion.View;
        }
    }

    private void Registrar()
    {
        _repository.Add(Empleado);
        ActualizarCategoria!(Empleado.IdCategoriaNavigation);
        Actualizar();
        Operacion = Operacion.View;
    }

    private void VerRegistrar()
    {
        Empleado = new() { Activo = 1 };
        Operacion = Operacion.Create;
    }

    private void VerEliminar(int id)
    {
        Empleado = _repository.GetById(id);
        Operacion = Operacion.Delete;
    }

    private void VerModificar(int id)
    {
        Empleado = _repository.GetById(id);
        _empleadotemp = new Empleado()
        {
            Id = Empleado.Id,
            Nombre = Empleado.Nombre,
            Sueldo = Empleado.Sueldo,
            Activo = Empleado.Activo,
            IdCategoria = Empleado.IdCategoria
        };
        Operacion = Operacion.Update;
    }

    public void Actualizar()
    {
        Empleados.Clear();
        Categorias.Clear();
        foreach (var item in _repository.GetAll())
        {
            Empleados.Add(item);
        }
        foreach (var item in _repository.GetAllCategorias())
        {
            Categorias.Add(item);
        }
        Notificar(nameof(Empleados));
        Notificar(nameof(Categorias));
    }

    public void Notificar([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
