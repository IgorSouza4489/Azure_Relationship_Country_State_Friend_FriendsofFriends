using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAt.Models
{
    public class AmigosHerd
    {
        [Key]
        public int IdAmigosHerd { get; set; }

        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Foto { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string EstadoOrigem { get; set; }
        public string PaisOrigem { get; set; }

        public int Id { get; set; }
        public virtual Amigo Amigo { get; set; }
    }
}
