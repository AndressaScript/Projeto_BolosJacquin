using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Projeto_BolosJacquin.Models;

[Index("Nome", Name = "UQ__Categori__7D8FE3B2E16B8FF6", IsUnique = true)]
public partial class Categoria
{
    [Key]
    [Column("CategoriaID")]
    public Guid CategoriaId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [InverseProperty("Categoria")]
    public virtual ICollection<Produtos> Produtos { get; set; } = new List<Produtos>();
}
