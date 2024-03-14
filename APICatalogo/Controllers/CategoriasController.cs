using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _context;

        public CategoriasController(IUnitOfWork context)
        {
            _context = context;
        }



        [HttpGet("produtos")]
        public async Task <ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
        {
            try
            {
                var prodCategoria = await _context.CategoriaRepository.GetCategoriasProdutos();
                if(prodCategoria.Count() == 0)
                {
                    return NotFound("Não existe produto Cadastrado para categoria");
                }
                return Ok(prodCategoria);
                //aplicando filtro
                //return _context.Categorias.Include(p => p.CategoriaId).Where(c => c.CategoriaId <= 5).ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {

                var categorias = await _context.CategoriaRepository.GetCategorias(categoriasParameters);
                
                    var metadata = new
                    {
                        categorias.TotalCount,
                        categorias.PageSize,
                        categorias.CurrentPage,
                        categorias.TotalPages,
                        categorias.HasNext,
                        categorias.HasPrevious
                    };

                    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
                

                if (categorias is null)
                {
                    return NotFound("Categorias não encontrada ...");
                }
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        //[HttpGet]
        //public ActionResult <ICollection<Categoria>> Get()
        //{
        //    try
        //    {

        //        var categorias = _context.CategoriaRepository.Get().ToList();

        //        if (categorias is null)
        //        {
        //            return NotFound("Categorias não encontrada ...");
        //        }
        //        return Ok(categorias);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);

        //    }
            
        //}

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task <ActionResult<Categoria>> Get(int id)
        {
            try
            {
                //var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
                var categoria = await _context.CategoriaRepository.GetById(c => c.CategoriaId == id);
                if (categoria is null)
                {
                    return NotFound("Categoria não encontrada ...");
                }
                return Ok(categoria);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post(Categoria categoria)
        {
            if(categoria is null)
            {
                return BadRequest("Dados inválidos");
            }

            _context.CategoriaRepository.Add(categoria);
            await _context.Commit();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);

        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest("Dados inválidos");
            }

            _context.CategoriaRepository.Update(categoria);
            await _context.Commit();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            //var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
            var categoria = await _context.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if(categoria == null)
            {
                return NotFound($"Categoria id= {id} não encontrada");
            }

            _context.CategoriaRepository.Delete(categoria);
            await _context.Commit();
            return Ok(categoria);
        }
    }
}
