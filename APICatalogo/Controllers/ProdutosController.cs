using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _context;

        public ProdutosController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosPrecos()
        {
            var produtos = await _context.ProdutoRepository.GetProdutosPorPreco();
            return Ok(produtos);
        }

        //[HttpGet]
        //public ActionResult <IEnumerable<Produto>> Get()
        //{
        //    try
        //    {
        //        var produtos = _context.ProdutoRepository.Get().ToList();
        //        //var produtos = _context.Produto.AsNoTracking.ToList();
        //        //aplicando filtro
        //        //var produtos = _context.Produtos.Take(10).ToList();
        //        if (produtos.Count == 0)
        //        {
        //           return NotFound("Produtos não encontrados");
        //        }
        //        return Ok(produtos);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            try
            {
                var produtos = await _context.ProdutoRepository.GetProdutos(produtosParameters);

                var metadata = new
                {
                    produtos.TotalCount,
                    produtos.PageSize,
                    produtos.CurrentPage,
                    produtos.TotalPages,
                    produtos.HasNext,
                    produtos.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
                //var produtos = _context.Produto.AsNoTracking.ToList();
                //aplicando filtro
                //var produtos = _context.Produtos.Take(10).ToList();
                if (produtos.Count == 0)
                {
                    return NotFound("Produtos não encontrados");
                }
                return Ok(produtos);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {

            try
            {
                var produto = await _context.ProdutoRepository.GetById(p => p.ProdutoId == id);
                //var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
                if (produto == null)
                {
                    return NotFound("Produto não encontrado");
                }
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Post(Produto produto)
        {
            if (produto is null)
            {
                return BadRequest("Dados inválidos");
            }
            _context.ProdutoRepository.Add(produto);
            await _context.Commit();
            //_context.Produtos.Add(produto);
            //_context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest("Dados inválidos");
                }
                if (!_context.CategoriaRepository.Get().Any(c => c.CategoriaId == produto.CategoriaId))
                {
                    return BadRequest($"A Categoria com ID= {produto.CategoriaId} não existe. Verifique a CategoriaId.");
                }

                _context.ProdutoRepository.Update(produto);
                await _context.Commit();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest (ex.Message);
            }
            
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var produto = await _context.ProdutoRepository.GetById(P => P.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound($"Produto com id= {id} não localizado ...");
                }

                _context.ProdutoRepository.Delete(produto);
                await _context.Commit();

                return Ok(produto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
