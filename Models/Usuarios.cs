using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Projeto_BolosJacquin.Models;

[Index("Email", Name = "UQ__Usuarios__A9D105346DC99828", IsUnique = true)]
public partial class Usuarios
{
    [Key]
    [Column("UsuarioID")]
    public Guid UsuarioId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Sobrenome { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    public string SenhaHash { get; set; } = null!;

    public bool Situacao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataCadastro { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Perfil { get; set; } = null!;

    [InverseProperty("Usuario")]
    public virtual ICollection<Avaliacoes> Avaliacoes { get; set; } = new List<Avaliacoes>();
}
