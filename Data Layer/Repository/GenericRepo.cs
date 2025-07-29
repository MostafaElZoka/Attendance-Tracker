

using Data_Layer.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data_Layer.Repository
{
    internal class GenericRepo<T> : IGenericRepo<T> where T : class
    {

        private readonly AttendanceDbContext _context;
        private readonly DbSet<T> _dbset;

        public GenericRepo(AttendanceDbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
             await _dbset.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            return await _dbset.AnyAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbset.Update(entity);
        }
    }
}
