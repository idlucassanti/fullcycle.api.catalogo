using FullCycle.Catalogo.Domain.Exceptions;
using System;

namespace FullCycle.Catalogo.Domain.Entities
{
    public class Categoria
    {
        public Categoria(string nome, string descricao, bool ativo = true)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            DataInsercao = DateTime.Now;

            Validate();
        }

        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataInsercao { get; private set; }

        public void Ativar()
        {
            Ativo = true;
            Validate();
        }

        public void Desativar()
        {
            Ativo = false;
            Validate();
        }

        public void Atualizar(string nome, string? descricao = null)
        {
            Nome = nome;
            Descricao = descricao ?? Descricao;

            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Nome))
                throw new DomainEntityException("Nome não pode ser vazio ou nulo.");

            if (Nome.Length < 3)
                throw new DomainEntityException("Nome precisa ser maior que 3 caracteres.");

            if (Nome.Length > 255)
                throw new DomainEntityException("Nome pode ter no máximo 255 caracteres.");

            if (Descricao == null)
                throw new DomainEntityException("Descrição não pode ser nulo.");

            if (Descricao.Length > 1000)
                throw new DomainEntityException("Descrição pode ter no máximo 1000 caracteres.");
        }
    }
}
