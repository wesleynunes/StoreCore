using StoreCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StoreCore.Models.admin
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "O campo categoria é obrigátorio")]
        [MaxLength(50, ErrorMessage = "O campo categoria recebe no máximo 50 caracteres")]
        [Display(Name = "Categoria")]
        public string CategoryName { get; set; }

        [Display(Name = "Data de criação")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Data de atualização")]
        public DateTime UpdateDate { get; set; }

        [Required(ErrorMessage = "O campo usuário é requerido!")]
        public Guid UserId { get; set; }

        [Display(Name = "Usuário")]
        public virtual ApplicationUser User { get; set; }

    }
}
