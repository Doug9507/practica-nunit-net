using Empleados.Infraestructura.Data;
using Empleados.Infraestructura.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Empleados.Infraestructura.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        protected DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad);
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            if (incluirPropiedades != null)   // "Compania, Cargo, Departamento"
            {
                foreach (var ip in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(ip);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>,
                             IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            if (incluirPropiedades != null)   // "Compania, Cargo, Departamento"
            {
                foreach (var ip in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(ip);
                }
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();

        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}
