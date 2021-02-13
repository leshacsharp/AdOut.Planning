using AdOut.Extensions.Exceptions;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Managers
{
    public class PlanAdManager : IPlanAdManager
    {
        private readonly IPlanAdRepository _planAdRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IAdRepository _adRepository;

        public PlanAdManager(
            IPlanAdRepository planAdRepository,
            IPlanRepository planRepository,
            IAdRepository adRepository)
        {
            _planAdRepository = planAdRepository;
            _planRepository = planRepository;
            _adRepository = adRepository;
        }

        public async Task AddAdToPlanAsync(string planId, string adId, int order)
        {
            var plan = await _planRepository.GetByIdAsync(planId);
            if (plan == null)
            {
                throw new ObjectNotFoundException($"Plan with id={planId} was not found");
            }

            var ad = await _adRepository.GetByIdAsync(adId);
            if (ad == null)
            {
                throw new ObjectNotFoundException($"Ad with id={adId} was not found");
            }

            var planHasAdWithSameOrder = await _planAdRepository.Read(pa => pa.PlanId == planId && pa.Order == order).AnyAsync();
            if (planHasAdWithSameOrder)
            {
                throw new BadRequestException($"Plans can't contain ads with same orders");
            }

            var planAd = new PlanAd()
            {
                Plan = plan,
                Ad = ad,
                Order = order
            };

            _planAdRepository.Create(planAd);
        }

        public async Task DeleteAdFromPlanAsync(string planId, string adId)
        {
            var planAd = await _planAdRepository.GetByIdAsync(planId, adId);
            if (planAd == null)
            {
                throw new ObjectNotFoundException($"PlanAd with id=(planId={planId},adId={adId}) was not found");
            }

            var planAdsCount = await _planAdRepository.Read(pa => pa.PlanId == planId).CountAsync();
            if (planAdsCount == 1)
            {
                throw new BadRequestException($"Plan can't exists without ads. Plan with id={planId} has one ad");
            }

            _planAdRepository.Delete(planAd);
        }

        public async Task UpdateAdInPlanAsync(string planId, string adId, int order)
        {
            var planAd = await _planAdRepository.GetByIdAsync(planId, adId);
            if (planAd == null)
            {
                throw new ObjectNotFoundException($"Plan Ad with id=(planId={planId},adId={adId}) was not found");
            }

            planAd.Order = order;

            _planAdRepository.Update(planAd);
        }
    }
}
