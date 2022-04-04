using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAt.Models
{
    public class Pais
    {
        public int PaisId { get; set; }
        public string Nome { get; set; }
        public string Foto { get; set; }
        [NotMapped]
        public IFormFile Imagem { get; set; }
        public virtual ICollection<Estado> ListaEstados { get; set; }

    }
}
