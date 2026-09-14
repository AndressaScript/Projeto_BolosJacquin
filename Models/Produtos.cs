using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Projeto_BolosJacquin.Models;

public partial class Produtos
{
    [Key]
    [Column("ProdutoID")]
    public Guid ProdutoId { get; set; }

    [Column("CategoriaID")]
    public Guid CategoriaId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Preco { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string EnderecoImagem { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string DescricaoCurta { get; set; } = null!;

    [StringLength(450)]
    [Unicode(false)]
    public string DescricaoLonga { get; set; } = null!;

    public bool Disponibilidade { get; set; }

    public bool Situacao { get; set; }

    [InverseProperty("Produto")]
    public virtual ICollection<Avaliacoes> Avaliacoes { get; set; } = new List<Avaliacoes>();

    [ForeignKey("CategoriaId")]
    [InverseProperty("Produtos")]
    public virtual Categoria Categoria { get; set; } = null!;
}
