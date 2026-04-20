using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Entities
{
    public class MenuItem : BaseEntity
    {
        public string Nome { get; private set; }
        public decimal Preco { get; private set; }
        public string Tipo { get; private set; }
        public bool Ativo { get; private set; } = true;

        protected MenuItem() { }

        public MenuItem(int id, string nome, decimal preco, string tipo) : base(id)
        {

            if (string.IsNullOrEmpty(nome))
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
            this.Preco = preco;
            this.Tipo = tipo;
        }

        public void Atualizar(string nome, decimal preco, string tipo)
        {
            Nome = nome;
            this.Preco = preco;
            this.Tipo = tipo;
            UpdateAt = DateTime.UtcNow;
        }

        public void Desaticar()
        {
            Ativo = false;
            UpdateAt = DateTime.UtcNow;
        }
    }
}