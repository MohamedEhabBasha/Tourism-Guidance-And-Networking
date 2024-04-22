

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public T? GetById(int id) => _context.Set<T>().Find(id);

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }
        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public bool Exist(int id)
        {
            var item = _context.Set<T>().Find(id);
            if (item == null) return false;
            return true;
        }
        public async Task<string> SaveCover(IFormFile cover, string _imagesPath)
        {
            string coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

            string path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);

            return path;
        }
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> critera, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.Where(critera).ToListAsync();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> critera, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(critera);
        }

    }
}
