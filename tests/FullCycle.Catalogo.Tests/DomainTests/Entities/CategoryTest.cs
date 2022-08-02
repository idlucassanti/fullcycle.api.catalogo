using FullCycle.Catalogo.Domain.Entities;
using FullCycle.Catalogo.Domain.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace FullCycle.Catalogo.Tests.DomainTests.Entities
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(InstanciarCategoria))]
        public void InstanciarCategoria()
        {
            var inputData = new
            {
                Nome = "Nome",
                Descricao = "Descricao"
            };

            var dateBefore = DateTime.Now;
            var categoria = new Categoria(inputData.Nome, inputData.Descricao);
            var dateAfter = DateTime.Now;

            Assert.NotNull(categoria);
            Assert.NotEmpty(categoria.Nome);
            Assert.NotEmpty(categoria.Descricao);
            Assert.NotEqual(default(Guid), categoria.Id);
            Assert.NotEqual(default(DateTime), categoria.DataInsercao);
            Assert.True(categoria.Ativo);
            Assert.True(categoria.DataInsercao > dateBefore);
            Assert.True(categoria.DataInsercao < dateAfter);
        }

        [Fact(DisplayName = nameof(InstanciarCategoriaAtivoFalse))]
        public void InstanciarCategoriaAtivoFalse()
        {
            var inputData = new
            {
                Nome = "Nome",
                Descricao = "Descricao"
            };

            var dateBefore = DateTime.Now;
            var categoria = new Categoria(inputData.Nome, inputData.Descricao, false);
            var dateAfter = DateTime.Now;

            Assert.NotNull(categoria);
            Assert.NotEqual(default(Guid), categoria.Id);
            Assert.NotEqual(default(DateTime), categoria.DataInsercao);
            Assert.True(categoria.DataInsercao > dateBefore);
            Assert.True(categoria.DataInsercao < dateAfter);
            Assert.False(categoria.Ativo);
        }

        // validação das regras especificadas pelo PO
        [Theory(DisplayName = nameof(InstanciarCategoriaNomeVazioOuNuloException))]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstanciarCategoriaNomeVazioOuNuloException(string? nome)
        {
            Action action = () => new Categoria(nome, "Descricao");

            var exception = Assert.Throws<DomainEntityException>(action);

            Assert.Contains("Nome não pode ser vazio ou nulo.", exception.Message);
        }

        [Fact(DisplayName = nameof(InstanciarCategoriaDescricalNuloExpcetion))]
        public void InstanciarCategoriaDescricalNuloExpcetion()
        {
            Action action = () => new Categoria("Nome", null);

            var exception = Assert.Throws<DomainEntityException>(action);

            Assert.Contains("Descrição não pode ser nulo.", exception.Message);
        }

        [Theory(DisplayName = nameof(InstanciarCategoriaNomeMin3CaracteresException))]
        [InlineData("A")]
        [InlineData("Ab")]
        public void InstanciarCategoriaNomeMin3CaracteresException(string nome)
        {
            Action action = () => new Categoria(nome, "Descrição");

            var exception = Assert.Throws<DomainEntityException>(action); 

            Assert.Contains("Nome precisa ser maior que 3 caracteres.", exception.Message);
        }

        [Fact(DisplayName = nameof(InstanciarCategoriaNomeMax255CaracteresException))]
        public void InstanciarCategoriaNomeMax255CaracteresException()
        {
            var nomeInvalido = string.Join(null, Enumerable.Range(1, 256).Select(_ => "A").ToArray());

            Action action = () => new Categoria(nomeInvalido, "Descrição");

            var exception = Assert.Throws<DomainEntityException>(action);

            Assert.Contains("Nome pode ter no máximo 255 caracteres.", exception.Message);
        }

        [Fact(DisplayName = nameof(InstanciarCategoriaDescricaoMax1000CaracteresException))]
        public void InstanciarCategoriaDescricaoMax1000CaracteresException()
        {
            var descricaoInvalida = string.Join(null, Enumerable.Range(1, 1001).Select(_ => "A").ToArray());

            Action action = () => new Categoria("Nome", descricaoInvalida);

            var exception = Assert.Throws<DomainEntityException>(action);

            Assert.Contains("Descrição pode ter no máximo 1000 caracteres.", exception.Message);
        }

        // validação das funcionalidades da entidade
        [Fact(DisplayName = nameof(AtivarCategoria))]
        public void AtivarCategoria()
        {
            var categoria = new Categoria("Nome", "Descrição", false);

            categoria.Ativar();

            Assert.True(categoria.Ativo);
        }

        [Fact(DisplayName = nameof(DesativarCategoria))]
        public void DesativarCategoria()
        {
            var categoria = new Categoria("Nome", "Descrição");

            categoria.Desativar();

            Assert.False(categoria.Ativo);
        }

        [Theory(DisplayName = nameof(AtualizarCategoria))]
        [InlineData("Nova descrição")]
        [InlineData("")]
        public void AtualizarCategoria(string descricao)
        {
            var categoria = new Categoria("Nome", "Descricao");
            var newData = new { 
                Nome = "Novo nome", 
                Descricao = descricao
            };

            categoria.Atualizar(newData.Nome, newData.Descricao);

            Assert.Equal(categoria.Nome, newData.Nome);
            Assert.Equal(categoria.Descricao, newData.Descricao);
        }

        [Theory(DisplayName = nameof(AtualizarNomeVazioOuNuloException))]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void AtualizarNomeVazioOuNuloException(string? nome)
        {
            var categoria = new Categoria("Nome", "Descricao");

            Action action = () => categoria.Atualizar(nome);

            var exception = Assert.Throws<DomainEntityException>(action);

            Assert.Contains("Nome não pode ser vazio ou nulo.", exception.Message);
        }

        [Theory(DisplayName = nameof(AtualizarNomeMin3CaracteresException))]
        [InlineData("A")]
        [InlineData("Ab")]
        public void AtualizarNomeMin3CaracteresException(string nome)
        {
            var categoria = new Categoria("Nome", "Descricao");

            Action action = () => categoria.Atualizar(nome);

            var exception = Assert.Throws<DomainEntityException>(action);

            Assert.Contains("Nome precisa ser maior que 3 caracteres.", exception.Message);
        }

        [Fact(DisplayName = nameof(AtualizarNomeMax255CaracteresException))]
        public void AtualizarNomeMax255CaracteresException()
        {
            var categoria = new Categoria("Nome", "Descricao");

            var nomeInvalido = string.Join(null, Enumerable.Range(1, 256).Select(_ => "A").ToArray());

            Action action = () => categoria.Atualizar(nomeInvalido);

            var exception = Assert.Throws<DomainEntityException>(action);

            Assert.Contains("Nome pode ter no máximo 255 caracteres.", exception.Message);
        }

        [Fact(DisplayName = nameof(AtualizarDescricaoMax1000CaracteresException))]
        public void AtualizarDescricaoMax1000CaracteresException()
        {
            var categoria = new Categoria("Nome", "Descricao");

            var descricaoInvaliada = string.Join(null, Enumerable.Range(1, 1001).Select(_ => "A").ToArray());

            Action action = () => categoria.Atualizar("Novo nome", descricaoInvaliada);

            var exception = Assert.Throws<DomainEntityException>(action);

            Assert.Contains("Descrição pode ter no máximo 1000 caracteres.", exception.Message);
        }


    }
}
