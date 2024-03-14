using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace APICatalogo.Repository
{
    //Classe genérica, onde T é um parâmetro de tipo. 
    //Posso trabalhar com diferentes tipos de entidades. Por exemplo, pode ter um Repository<Produto> para lidar com a entidade Produto.
    //IRepository<T>: indica que a classe Repository<T> implementa a interface IRepository<T>. Interfaces definem um contrato que as classes que as implementam devem seguir.
    //where T : class: Isso é uma restrição de tipo que especifica que o tipo T deve ser uma classe (um tipo de referência, não um tipo de valor).
    //Essa restrição é aplicada ao uso do tipo T nesta classe genérica.
    public class Repository<T> : IRepository<T> where T : class
    {
        //declara um campo protegido chamado _context do tipo AppDbContext. O _context armazenará a instância do contexto do banco de dados.
        protected AppDbContext _context;

        // construtor da classe Repository<T>. Ele aceita um parâmetro do tipo AppDbContext chamado context. O construtor é chamado quando cria uma instância da classe Repository<T>.
        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State= EntityState.Modified;
            _context.Set<T>().Update(entity);
        }
    }
}
