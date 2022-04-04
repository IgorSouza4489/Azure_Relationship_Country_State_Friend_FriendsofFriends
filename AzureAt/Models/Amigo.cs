using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAt.Models
{
    public class Amigo
    {       
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Foto { get; set; }
        [NotMapped]
        public IFormFile Imagem { get; set; }
        public string Email { get; set; }       
        public string Telefone { get; set; }        
        public DateTime Aniversario { get; set; }
        public string EstadoOrigem { get; set; }
        public string PaisOrigem { get; set; }
        public int EstadoId { get; set; }
        public virtual Estado Estado { get; set; }
        public ICollection<Amiguinhos> ListaAmiguinhos { get; set; }

    }
}
