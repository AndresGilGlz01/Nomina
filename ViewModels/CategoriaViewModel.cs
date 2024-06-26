﻿using Nomina.Helpers;
using Nomina.Models;
using Nomina.Repositories;
using Nomina.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Nomina.ViewModels;

public class CategoriaViewModel : IViewModel
{
    readonly CategoriaRepository _repository;
    Operacion _operacion;
    ObservableCollection<Categoria> _categorias = new();
    string _errores = string.Empty;
    string _error = string.Empty;
    Categoria _categoria = new();
    Categoria? _categoriatemp;

    public ObservableCollection<Categoria> Categorias
    {
        get => _categorias;
        set { _categorias = value; Notificar(); }
    }

    public Categoria Categoria
    {
        get => _categoria;
        set { _categoria = value; Notificar(); }
    }

    public int CantidadCategorias
    {
        get => _categorias.Count;
    }

    public Operacion Operacion
    {
        get => _operacion;
        set { _operacion = value; Notificar(); }
    }

    public string Error
    {
        get => _error;
        set { _error = value; Notificar(); }
    }

    public string Errores
    {
        get => _errores;
        set { _errores = value; Notificar(); }
    }

    public Action? ActualizarEmpleados { get; set; }

    public ICommand VerRegistrarCommand { get; set; }
    public ICommand VerModificarCommand { get; set; }
    public ICommand VerEliminarCommand { get; set; }
    public ICommand RegistrarCommand { get; set; }
    public ICommand EliminarCommand { get; set; }
    public ICommand RegresarCommand { get; set; }
    public ICommand FiltrarCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public CategoriaViewModel(NominaContext context)
    {
        _repository = new(context);
        VerModificarCommand = new RelayCommand<int>(VerModificar);
        VerEliminarCommand = new RelayCommand<int>(VerEliminar);
        VerRegistrarCommand = new RelayCommand(VerRegistrar);
        RegistrarCommand = new RelayCommand(Registrar);
        EliminarCommand = new RelayCommand(Eliminar);
        RegresarCommand = new RelayCommand(Regresar);
        FiltrarCommand = new RelayCommand<string>(Filtrar);
        Actualizar();
    }

    private void Filtrar(string pattern)
    {
        throw new NotImplementedException();
    }

    private void Regresar()
    {
        if (Operacion == Operacion.Update && _categoriatemp is not null)
        {
            Categoria.Id = _categoriatemp.Id;
            Categoria.Nombre = _categoriatemp.Nombre;
            Categoria.SueldoMaximo = Categoria.SueldoMaximo;
            Categoria.TotalEmpleados = _categoriatemp.TotalEmpleados;
            Notificar(nameof(Empleado));
        }
        Operacion = Operacion.View;
    }

    private void Eliminar()
    {
        if (Categoria is not null)
        {
            _repository.Delete(Categoria);
            Actualizar();
            Operacion = Operacion.View;
        }
    }

    private void Registrar()
    {
        Categoria temp = Categoria;
        Categoria = _repository.GetById(Categoria.Id);

        if (Categoria is not null)
        {
            _repository.Update(Categoria);
            ActualizarEmpleados!();
        }
        else
        {
            _repository.Add(temp);
        }
        Actualizar();
        Operacion = Operacion.View;
    }

    private void VerRegistrar()
    {
        Categoria = new();
        Operacion = Operacion.Create;
    }

    private void VerEliminar(int id)
    {
        Categoria = _repository.GetById(id);
        Operacion = Operacion.Delete;
    }

    private void VerModificar(int id)
    {
        Categoria = _repository.GetById(id);
        _categoriatemp = new Categoria()
        {
            Id = Categoria.Id,
            Nombre = Categoria.Nombre,
            SueldoMaximo = Categoria.SueldoMaximo,
            TotalEmpleados = Categoria.TotalEmpleados,
        };
        Operacion = Operacion.Update;
    }

    public void Actualizar()
    {
        Categorias.Clear();
        foreach (var item in _repository.GetAll())
        {
            Categorias.Add(item);
        }
        Notificar(nameof(Categorias));
    }

    public void Notificar([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
