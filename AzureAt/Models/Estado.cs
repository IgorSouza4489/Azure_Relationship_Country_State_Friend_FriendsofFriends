using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAt.Models
{
    public class Estado
    {
        public int EstadoId { get; set; }
        public string Nome { get; set; }
        public string Foto { get; set; }


        public int PaisId { get; set; }
        public string PaisOrigem { get; set; }
        public virtual Pais Pais { get; set; }
        public virtual ICollection<Amigo> ListaAmigos { get; set; }


    }
}
