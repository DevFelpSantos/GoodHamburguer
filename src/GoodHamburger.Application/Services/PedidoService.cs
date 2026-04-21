namespace GoodHamburger.Application.Services
{
    using GoodHamburger.Application.DTOs;
    using GoodHamburger.Domain.Entities;
    using GoodHamburger.Domain.Interfaces;
    using GoodHamburger.Application.Services.Interfaces;
    using FluentValidation;

    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IDescontoService _descontoService;
        private readonly IValidator<CriarPedidoRequestDto> _validator;

        public PedidoService(
            IPedidoRepository pedidoRepository,
            IMenuItemRepository menuItemRepository,
            IDescontoService descontoService,
            IValidator<CriarPedidoRequestDto> validator)
        {
            _pedidoRepository = pedidoRepository;
            _menuItemRepository = menuItemRepository;
            _descontoService = descontoService;
            _validator = validator;
        }

        public async Task<PedidoResponseDto> CriarPedidoAsync(CriarPedidoRequestDto request, CancellationToken ct = default)
        {
            var validationResult = await _validator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var itens = new List<MenuItem>();
            foreach (var id in request.IdsItens)
            {
                var item = await _menuItemRepository.GetByIdAsync(id, ct);
                if (item == null)
                {
                    throw new KeyNotFoundException($"Item com ID {id} não encontrado.");
                }
                itens.Add(item);
            }

            var subtotal = itens.Sum(i => i.Preco);
            var pedido = new Pedido(subtotal, 0);

            foreach (var item in itens)
            {
                var itemPedido = new ItemPedido(item.Id, item);
                pedido.AdicionarItem(itemPedido);
            }

            var percentualDesconto = _descontoService.CalcularPercentualDesconto(pedido.Itens.ToList());
            pedido.AtualizarValores(subtotal, percentualDesconto);

            await _pedidoRepository.AddAsync(pedido, ct);
            await _pedidoRepository.SaveChangesAsync(ct);

            return MapToDto(pedido);


        }
        public async Task<List<PedidoResponseDto>> ListarPedidosAsync(CancellationToken ct = default)
        {
            var pedidos = await _pedidoRepository.GetAllAsync(ct);
            return pedidos.Select(MapToDto).ToList();
        }

        public async Task<PedidoResponseDto?> ObterPedidoPorIdAsync(int id, CancellationToken ct = default)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id, ct);
            return pedido == null ? null : MapToDto(pedido);
        }

        public async Task<PedidoResponseDto?> AtualizarPedidoAsync(int id, CriarPedidoRequestDto request, CancellationToken ct = default)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id, ct);
            if (pedido == null)
                throw new Exception($"Pedido com ID {id} não encontrado.");

            var validationResult = await _validator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var itens = new List<MenuItem>();
            foreach (var itemId in request.IdsItens)
            {
                var item = await _menuItemRepository.GetByIdAsync(itemId, ct);
                if (item == null)
                    throw new Exception($"Item com ID {itemId} não encontrado.");
                itens.Add(item);
            }

            pedido.Itens.Clear();

            foreach (var item in itens)
            {
                var itemPedido = new ItemPedido(item.Id, item);
                pedido.AdicionarItem(itemPedido);
            }

            var subtotal = itens.Sum(i => i.Preco);
            var percentualDesconto = _descontoService.CalcularPercentualDesconto(pedido.Itens.ToList());
            pedido.AtualizarValores(subtotal, percentualDesconto);

            _pedidoRepository.Update(pedido);
            await _pedidoRepository.SaveChangesAsync(ct);

            return MapToDto(pedido);
        }

        public async Task<bool> DeletarPedidoAsync(int id, CancellationToken ct = default)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id, ct);
            if (pedido == null)
                return false;

            _pedidoRepository.Delete(pedido);
            return await _pedidoRepository.SaveChangesAsync(ct);
        }

        private static PedidoResponseDto MapToDto(Pedido pedido)
        {
            return new PedidoResponseDto(
                pedido.Id,
                pedido.Itens.Select(i => new ItemPedidoResponseDto(
                    i.MenuItemId,
                    i.NomeItem,
                    i.PrecoUnitario,
                    i.Tipo
                )).ToList(),
                pedido.Subtotal,
                pedido.PercentualDesconto,
                pedido.ValorDesconto,
                pedido.Total,
                pedido.CreatedAt,
                pedido.UpdatedAt
            );
        }
    }
}