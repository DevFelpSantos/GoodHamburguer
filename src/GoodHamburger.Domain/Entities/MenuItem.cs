namespace GoodHamburger.Domain.Entities
{
    public class MenuItem : BaseEntity
    {
        public string Nome { get; private set; } = string.Empty;
        public decimal Preco { get; private set; }
        public string Tipo { get; private set; }
        public bool Ativo { get; private set; } = true;

        protected MenuItem() { }

        public MenuItem(int id, string nome, decimal preco, string tipo) : base(id)
        {

            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome é obrigatorio.");
            }

            if (preco <= 0)
            {
                throw new ArgumentException("O preco deve ser maior que zero.");
            }

            if (string.IsNullOrEmpty(tipo))
            {
                throw new ArgumentException("O tipo é obrigatorio.");
            }

            Nome = nome;
            Preco = preco;
            Tipo = tipo;
        }

        public void Atualizar(string nome, decimal preco, string tipo)
        {

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome inválido");

            if (preco <= 0)
                throw new ArgumentException("Preço inválido");

            if (string.IsNullOrWhiteSpace(tipo))
                throw new ArgumentException("Tipo inválido");

            Nome = nome;
            Preco = preco;
            Tipo = tipo;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Desativar()
        {
            Ativo = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}