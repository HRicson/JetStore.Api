using JetStore.Context;
using JetStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JetStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos()
        {
            try
            {
                List<ProdutoDTO> dtoList = new();
                var produtoList = await _context.Produtos.ToListAsync();

                if (produtoList == null)
                    return NotFound();

                foreach (var produto in produtoList)
                {
                    dtoList.Add(new ProdutoDTO()
                    {
                        Id = produto.Id,
                        Nome = produto.Nome,
                        Descricao = produto.Descricao,
                        Estoque = produto.Estoque,
                        Status = produto.Status,
                        Preco = produto.Preco,
                        ImagemBase64String = Convert.ToBase64String(produto.Imagem),
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = dtoList
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDTO>> GetProduto(Guid id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);

                if (produto == null)
                    return NotFound();

                return Ok(new
                {
                    success = true,
                    data = new ProdutoDTO()
                    {
                        Id = produto.Id,
                        Nome = produto.Nome,
                        Descricao = produto.Descricao,
                        Estoque = produto.Estoque,
                        Status = produto.Status,
                        Preco = produto.Preco,
                        ImagemBase64String = Convert.ToBase64String(produto.Imagem),
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(Guid id, ProdutoDTO dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest();

                var produto = await _context.Produtos.FindAsync(id);

                if (produto == null)
                    return NotFound();

                if (!ProdutoModel.TryUpdate(dto, produto))
                    return BadRequest();

                _context.Entry(produto).State = EntityState.Modified;

               var result = await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = dto
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                bool produtoEncontrado = _context.Produtos.Any(p => p.Id == id);

                if (!produtoEncontrado)
                    return NotFound();
                else
                    return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> PostProduto(ProdutoDTO produto)
        {
            if (!ProdutoModel.TryNew(produto, out ProdutoModel produtoModel))
                return BadRequest();

            try
            {
                _context.Produtos.Add(produtoModel);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProduto", new { id = produto.Id }, produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(Guid id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);

                if (produto == null)
                    return NotFound();

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
