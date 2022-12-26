using AutoMapper;
using Core.Models;
using PTO_Server.Dto;

namespace PTO_Server.Helpers
{
    public class ProductUrlResolver : IValueResolver<Products, ProductsToReturnDto, string>
    {
        private readonly IConfiguration _configuration;
        public ProductUrlResolver(IConfiguration config)
        {
            _configuration = config;
        }
        public string Resolve(Products source, ProductsToReturnDto destination, string destMember, 
            ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _configuration.GetSection("PTO_App")["ApiUrl"].ToString() + source.PictureUrl;
            }
            return null;
        }
    }
}
