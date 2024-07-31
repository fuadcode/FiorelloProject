using Fiorello.ViewModels;

namespace Fiorello.Services.Interfaces
{
    public interface IBasketService
    {
        int GetBasketCount();
        List<BasketVM> GetBasketList();
    }
}
