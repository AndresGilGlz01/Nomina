using System;
using System.Collections.Generic;

namespace Nomina.Repositories;
public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    IEnumerable<T> Filter(string pattern);
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
