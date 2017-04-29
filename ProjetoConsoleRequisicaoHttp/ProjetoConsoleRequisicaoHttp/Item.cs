namespace ProjetoConsoleRequisicaoHttp
{
    public class Item
    {
        public int id { get; set; }
        public string descricao { get; set; }
        public bool feito { get; set; }
        public int idtodo { get; set; }

        public virtual Todo todo { get; set; }
    }
}