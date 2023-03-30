using Microsoft.EntityFrameworkCore;
using Nomina.Models;
using Nomina.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Nomina.Repositories;
public class EmpleadoRepository : IRepository<Empleado>
{
    readonly NominaContext _context = MainViewModel.Context;

    public void Add(Empleado entity)
    {
        _context.Empleado.Add(entity);
        _context.SaveChanges();
        _context.Entry(entity).Reload();
    }

    public void Delete(Empleado entity)
    {
        _context.Database.ExecuteSqlRaw($"CALL sp_eliminar_empleado ({entity.Id});");
        _context.SaveChanges();
        _context.Entry(entity).Reload();
    }

    public IEnumerable<Empleado> Filter(string pattern)
    {
        return _context
            .Empleado
            .Where(emp => emp.Nombre.Contains(pattern));
    }

    public IEnumerable<Empleado> GetAll()
    {
        return _context
            .Empleado
            .Where(emp => emp.Activo == 1)
            .OrderBy(emp => emp.Nombre)
            .Include(emp => emp.IdCategoriaNavigation);
    }

    public IEnumerable<Categoria> GetAllCategorias()
    {
        return _context.Categoria;
    }

    public Empleado GetById(int id)
    {
        return _context
            .Empleado
            .FirstOrDefault(emp => emp.Id == id)!;
    }

    public void Update(Empleado entity)
    {
        _context.Empleado.Update(entity);
        _context.SaveChanges();
        _context.Entry(entity).Reload();
    }
}
