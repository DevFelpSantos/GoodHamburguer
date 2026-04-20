namespace GoodHamburger.Application.Validators
{
    using FluentValidation;
    using GoodHamburger.Application.DTOs;
    public class CriarPedidoValidator : AbstractValidator<CriarPedidoRequestDto>
    {
        public CriarPedidoValidator()
        {
            RuleFor(x => x.IdsItens)
            .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.")
            .Custom((ids, context) =>
            {
                if (ids.Count != ids.Distinct().Count())
                {
                    context.AddFailure("IdsItens", "Não é permitido adicionar itens duplicados ao pedido.");
                }

                var sanduiches = ids.Where(id => id >= 1 && id <= 3).Count();
                var acompanhamentos = ids.Where(id => id == 4).Count();
                var bebidas = ids.Where(id => id == 5).Count();

                if (sanduiches > 1 || acompanhamentos > 1 || bebidas > 1)
                {
                    context.AddFailure("IdsItens", "O pedido deve conter no máximo 1 sanduíche, 1 acompanhamento e 1 bebida.");
                }

            });
        }
    }
}