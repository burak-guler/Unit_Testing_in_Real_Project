
using Microsoft.EntityFrameworkCore;
using UdemyRealWorldUnitTest.Web.Models;

namespace UdemyRealWorldUnitTest.Web.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly UdemyUnitTestDBContext _context;
        private readonly DbSet<TEntity> _dbset;

        public Repository(UdemyUnitTestDBContext context)
        {
            _context = context;

            _dbset = _context.Set<TEntity>();
        }

        public async Task Create(TEntity entity)
        {  
            await _dbset.AddAsync(entity);  
            await _context.SaveChangesAsync();  
        }

        public void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            //_dbset.Update(entity);
            _context.SaveChanges();
        }
    }
}
