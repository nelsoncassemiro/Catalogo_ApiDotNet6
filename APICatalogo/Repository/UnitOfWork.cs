﻿using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProdutoRepository _produtoRepo;
        private CategoriaRepository _categoriaRepo;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            
            get 
            {
                //get { return _produtoRepo; }
                return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context); 
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                //get { return _categoriaRepo; }
                return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context);
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
