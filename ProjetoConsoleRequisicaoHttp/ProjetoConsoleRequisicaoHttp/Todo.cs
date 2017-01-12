using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoConsoleRequisicaoHttp
{
    public class Todo
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public bool feito { get; set; }
        
        public virtual List<Item> item { get; set; }
    }
}

