using System;
using System.ComponentModel.DataAnnotations;

namespace JetStore.Models
{
    public class ProdutoModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Descricao { get; set; }

        [Required]
        public int Estoque { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public byte[] Imagem { get; set; }

        public static bool TryNew(ProdutoDTO dto, out ProdutoModel produto)
        {
            produto = new();

            try
            {
                produto.Id = Guid.NewGuid();
                produto.Nome = dto.Nome;
                produto.Descricao = dto.Descricao;
                produto.Estoque = dto.Estoque;
                produto.Status = dto.Status;
                produto.Preco = dto.Preco;
                produto.Imagem = Convert.FromBase64String(dto.ImagemBase64String);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryUpdate(ProdutoDTO dto, ProdutoModel produto)
        {
            try
            {
                produto.Nome = dto.Nome;
                produto.Descricao = dto.Descricao;
                produto.Estoque = dto.Estoque;
                produto.Status = dto.Status;
                produto.Preco = dto.Preco;
                produto.Imagem = Convert.FromBase64String(dto.ImagemBase64String);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
