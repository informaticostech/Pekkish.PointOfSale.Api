using Microsoft.IdentityModel.Tokens;
using Pekkish.PointOfSale.DAL.Entities;
using static Pekkish.PointOfSale.Api.Models.PointOfSale.Constants;

namespace Pekkish.PointOfSale.Api.Services
{
    public interface IPointOfSaleService 
    {        
        Task<List<AppTenantInfo>> VendorList();
        Task<List<AppLocation>> LocationList(Guid tenantId);
        Task<List<AppBrand>> BrandList(Guid tenantId);
        Task<AppBrand> BrandItemGet(int id);
        Task<List<AppProductCategory>> ProductCategoryList(int brandId);
        Task<AppProductCategory> ProductCategoryItemGet(int id);
        Task<List<AppProduct>> ProductList(int categoryId);
        Task<AppProduct> ProductItemGet(int id);
    }
    public class PointOfSaleService : IPointOfSaleService
    {
        private readonly PointOfSaleContext _context;

        public PointOfSaleService(PointOfSaleContext context) 
        {
            _context = context;
        }

        #region WhatApp Interaction
        
        #endregion

        public async Task<List<AppTenantInfo>> VendorList()
        {
            return await Task.Run(() =>
            {
                var result = (from SaasTenants in _context.SaasTenants
                             from TenantInfos in _context.AppTenantInfos

                             where SaasTenants.ActivationState == (int)TenantActivationStatus.Active
                             
                             orderby TenantInfos.Name
                             select TenantInfos).Distinct().ToList();

                return result;
            });            
        }
        public async Task<List<AppLocation>> LocationList(Guid tenantId)
        {
            return await Task.Run(() =>
            {
                var result = (from list in _context.AppLocations                              
                              where list.TenantId == tenantId
                              where list.IsDeleted == false                              

                              orderby list.Name
                              select list).ToList();

                return result;
            });
        }
        public async Task<List<AppBrand>> BrandList(Guid tenantId)
        {
            return await Task.Run(() =>
            {
                var result = (from list in _context.AppBrands
                              where list.TenantId == tenantId
                              where list.IsEnabled == true
                              where list.IsDeleted == false

                              orderby list.Name
                              select list).ToList();

                return result;
            });
        }
        public async Task<AppBrand> BrandItemGet(int id)
        {
            return await Task.Run(() =>
            {
                var result = _context.AppBrands.Single(x => x.Id == id);

                return result;
            });
        }
        public async Task<List<AppProductCategory>> ProductCategoryList(int brandId)
        {
            return await Task.Run(() =>
            {
                var result = (from list in _context.AppProductCategories
                              where list.BrandId == brandId
                              where list.IsEnabled == true
                              where list.IsDeleted == false

                              orderby list.Name
                              select list).ToList();

                Parallel.ForEach(result, item =>
                {
                    item.Name = (item.ExternalAppName.IsNullOrEmpty()) ? item.Name : item.ExternalAppName;
                    item.Name = (item.Name.Length > 24) ? item.Name.Substring(0, 24) : item.Name;
                });

                return result;
            });
        }
        public async Task<AppProductCategory> ProductCategoryItemGet(int id)
        {
            return await Task.Run(() =>
            {
                var result = _context.AppProductCategories.Single(x => x.Id == id);
                
                return result;
            });
        }      
        public async Task<List<AppProduct>> ProductList(int categoryId)
        {
            return await Task.Run(() =>
            {
                var result = (from list in _context.AppProducts
                              where list.ProductCategoryId == categoryId
                              where list.IsEnabled == true
                              where list.IsDeleted == false

                              orderby list.Name
                              select list).ToList();

                Parallel.ForEach(result, item =>
                {
                    item.Name = (item.Name.Length > 24) ? item.Name.Substring(0, 24) : item.Name;
                });

                return result;
            });
        }
        public async Task<AppProduct> ProductItemGet(int id)
        {
            return await Task.Run(() =>
            {
                var result = _context.AppProducts.Single(x => x.Id == id);

                return result;
            });
        }
    }
}
