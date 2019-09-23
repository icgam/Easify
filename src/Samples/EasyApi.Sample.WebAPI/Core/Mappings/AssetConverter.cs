using System;
using AutoMapper;
using EasyApi.Sample.WebAPI.Domain;

namespace EasyApi.Sample.WebAPI.Core.Mappings
{
    public sealed class AssetConverter : ITypeConverter<AssetEntity, AssetDO>
    {
        private readonly IRateProvider _rateProvider;

        public AssetConverter(IRateProvider rateProvider)
        {
            _rateProvider = rateProvider ?? throw new ArgumentNullException(nameof(rateProvider));
        }

        public AssetDO Convert(AssetEntity source, AssetDO destination, ResolutionContext context)
        {
            if (source == null) return null;

            return new AssetDO
            {
                Id = source.Id,
                Rating = _rateProvider.GetRating(source.Id)
            };
        }
    }
}