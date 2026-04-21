namespace GoodHamburger.Tests.Integration
{
    using Xunit;
    using Microsoft.EntityFrameworkCore;
    using GoodHamburger.Infrastructure.Data;
    using GoodHamburger.Infrastructure.Repositories;
    using GoodHamburger.Application.Services;
    using GoodHamburger.Application.DTOs;
    using GoodHamburger.Application.Validators;
    using FluentValidation;
    public class PedidoServiceIntegrationTests : IAsyncLifetime
    {
        private GoodHamburgerContext _context;
        private PedidoService _pedidoService;


        public async Task InitializeAsync()
        {
            // Criar banco em memória
            var options = new DbContextOptionsBuilder<GoodHamburgerContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new GoodHamburgerContext(options);

            // Criar tabelas e dados
            await _context.Database.EnsureCreatedAsync();

            // Criar repositories
            var pedidoRepository = new PedidoRepository(_context);
            var menuItemRepository = new MenuItemRepository(_context);

            // Criar service
            var descontoService = new DescontoService();
            var validator = new CriarPedidoValidator();

            _pedidoService = new PedidoService(
                pedidoRepository,
                menuItemRepository,
                descontoService,
                validator
            );
        }

        public async Task DisposeAsync()
        {
            // Limpar depois do teste
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        // ═════════════════════════════════════════
        // TESTES
        // ═════════════════════════════════════════

        [Fact]
        public async Task CriarPedido_ComItensValidos_SalvaEmBancoERetorna()
        {
            // Arrange
            var request = new CriarPedidoRequestDto(new List<int> { 1, 4, 5 });

            // Act
            var resultado = await _pedidoService.CriarPedidoAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(9.50m, resultado.Subtotal);
            Assert.Equal(20m, resultado.PercentualDesconto);
            Assert.Equal(7.60m, resultado.Total);

            // Verificar banco
            var pedidoEmBanco = await _pedidoService.ObterPedidoPorIdAsync(resultado.Id, CancellationToken.None);
            Assert.NotNull(pedidoEmBanco);
            Assert.Equal(3, pedidoEmBanco.Itens.Count);
        }

        [Fact]
        public async Task CriarPedido_ComDuplicatas_ThrowValidationException()
        {
            // Arrange
            var request = new CriarPedidoRequestDto(new List<int> { 1, 1, 4 });

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(
                () => _pedidoService.CriarPedidoAsync(request, CancellationToken.None)
            );

            Assert.NotEmpty(ex.Errors);
        }

        [Fact]
        public async Task ListarPedidos_AposCriarDois_RetornaAmbos()
        {
            // Arrange
            var request1 = new CriarPedidoRequestDto(new List<int> { 1 });
            var request2 = new CriarPedidoRequestDto(new List<int> { 2 });

            // Act
            await _pedidoService.CriarPedidoAsync(request1, CancellationToken.None);
            await _pedidoService.CriarPedidoAsync(request2, CancellationToken.None);

            var pedidos = await _pedidoService.ListarPedidosAsync(CancellationToken.None);

            // Assert
            Assert.Equal(2, pedidos.Count);
        }
    }
}