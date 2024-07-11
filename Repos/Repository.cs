using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZahimarProject.Models;

namespace ZahimarProject.Repos
{
    public class Repository<T>: IRepository <T> where T:class 
    {
        private readonly Context context;
        internal DbSet<T> dbSet;
        public Repository(Context _context )
        {
            context = _context;
            this.dbSet = context.Set<T>();
        }
        public virtual void Delete(T entity)
        {
           
            dbSet.Update(entity);
        }
        public virtual List<T> GetAll()
        {
            IQueryable<T> query = dbSet;

            return query.ToList();
        }
        public virtual T Get(Expression<Func<T,bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }
        public void Insert(T obj)
        {
            dbSet.Add(obj);
        }
        //public void Save()
        //{
        //    context.SaveChanges();
        //}
        public void Update(T obj)
        {
            dbSet.Update(obj);
        }
    }
}
