using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;

//Criei Interface IRepository.
//Essa interface define um contrato genérico que pode ser implementado para qualquer tipo de entidade (T).
//As operações básicas de CRUD (Create, Read, Update, Delete) são abstraídas, proporcionando flexibilidade e padronização ao trabalhar com diferentes tipos de entidades.
//Essa abstração é valiosa, especialmente ao usar injeção de dependência e ao escrever código mais genérico e reutilizável em seu projeto.

namespace APICatalogo.Repository
{
    public interface IRepository<T>
    {
        //O método Get retorna uma consulta (IQueryable<T>) que representa uma coleção de entidades do tipo 'T'.
        IQueryable<T> Get();

        //O método GetById retorna uma entidade do tipo 'T' com base em um predicado especifico. O predicado é uma expressão lambda que é usada para filtrar resultados e encontrar
        //uma entidade especifica.
        Task<T> GetById(Expression<Func<T, bool>> predicate);

        //O método Add é responsável por adicionar uma nova entidade do tipo T. Este método é usado para inserir dados na fonte de dados.
        void Add(T entity);

        //O método Update é utilizado para atualizar uma entidade existente do tipo T. O detalhe específico da implementação pode variar, mas geralmente envolve modificar os campos da entidade
        void Update(T entity);

        //O método Delete é responsável por excluir uma entidade do tipo T.Ele remove a entidade da fonte de dados.
        void Delete(T entity);

    }
}
