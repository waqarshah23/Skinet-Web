using Microsoft.EntityFrameworkCore;
using Infrastructure.Middleware;

namespace PTO_Server.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        public readonly DataContext _context = null;
        public readonly DbSet<T> _entity =  null;
        public Repository()
        {
            _context = new DataContext();
            _entity = _context.Set<T>();
        }
        public Repository(DataContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await _entity.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            var entity = await _entity.FindAsync(id);
            //if (entity == null)
            //{
            //    throw new (nameof(id));
            //}
            return entity;
        }

        public async Task<bool> Add(T entity)
        {
            try
            {
                await _entity.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Update(T entity)
        {
            // await _entity(entity);
            try
            {
                _entity.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _entity.FindAsync(id);
            if(entity != null)
            {
                _entity.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false ;
        }
    }
}
