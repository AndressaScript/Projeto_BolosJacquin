using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Projeto_BolosJacquin.Models;

[Index("UsuarioId", "ProdutoId", Name = "UQ__Avaliaco__02F56795B8082FB6", IsUnique = true)]
public partial class Avaliacoes
{
    [Key]
    [Column("AvaliacaoID")]
    public Guid AvaliacaoId { get; set; }

    [Column("UsuarioID")]
    public Guid UsuarioId { get; set; }

    [Column("ProdutoID")]
    public Guid ProdutoId { get; set; }

    public int Nota { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Comentario { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Situacao { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string? MotivoOcultacao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataCriacao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DataUltimaAlteracao { get; set; }

    [ForeignKey("ProdutoId")]
    [InverseProperty("Avaliacoes")]
    public virtual Produtos Produto { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("Avaliacoes")]
    public virtual Usuarios Usuario { get; set; } = null!;
}
