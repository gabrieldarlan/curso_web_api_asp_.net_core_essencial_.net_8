﻿using APICatalogos.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogos.Models;

[Table("Produtos")]
public class Produto : IValidatableObject
{
    [Key]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(80, ErrorMessage = "O nome deve ter entre 5 e 80 caracteres", MinimumLength = 5)]
    [PrimeiraLetraMaiuscula]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "A descrição deve ter no máximo {1} caracteres.")]
    public string? Descricao { get; set; }

    [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 2)]
    public string? ImagemUrl { get; set; }
    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }
    // as duas instrucoes abaixo indicam que um produto possui uma categoria

    public int CategoriaId { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        //if (!string.IsNullOrEmpty(this.Nome))
        //{
        //    var primeiraLetra = this.Nome[0].ToString();
        //    if (primeiraLetra != primeiraLetra.ToUpper())
        //    {
        //        yield return new ValidationResult("A primeira letra do produto deve ser maiúscula", new[]
        //        {
        //            nameof(this.Nome)
        //        });
        //    }
        //}

        if (this.Estoque <= 0)
        {
            yield return new ValidationResult("O estoque deve ser maior que zeros", new[]
             {
                    nameof(this.Estoque)
                });
        }
    }
}

