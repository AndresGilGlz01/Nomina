using Nomina.Models;
using Nomina.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Nomina.Repositories;
public class CategoriaRepository : IRepository<Categoria>
{
    private NominaContext _context;

    public CategoriaRepository(NominaContext context)
    {
        _context = context;
    }

    public void Add(Categoria entity)
    {
        _context.Add(entity);
        _context.SaveChanges();
    }

    public void Delete(Categoria entity)
    {
        _context.Remove(entity);
        _context.SaveChanges();
    }

    public IEnumerable<Categoria> Filter(string pattern)
    {
        return _context
            .Categoria
            .Where(cat => cat.Nombre.Contains(pattern));
    }

    public IEnumerable<Categoria> GetAll()
    {
        return _context.Categoria;
    }

    public Categoria GetById(int id)
    {
        return _context
            .Categoria
            .FirstOrDefault(x => x.Id == id)!;
    }

    public void Update(Categoria entity)
    {
        _context.Categoria.Update(entity);
        _context.SaveChanges();
    }
}
