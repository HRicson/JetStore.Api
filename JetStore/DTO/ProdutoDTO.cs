using System;

public class ProdutoDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public int Estoque { get; set; }
    public bool Status { get; set; }
    public decimal Preco { get; set; }
    public string ImagemBase64String { get; set; }
}