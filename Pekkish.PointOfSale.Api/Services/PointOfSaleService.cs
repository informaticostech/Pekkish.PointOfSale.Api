using Pekkish.PointOfSale.DAL.Entities;
using static Pekkish.PointOfSale.Api.Models.PointOfSale.Constants;

namespace Pekkish.PointOfSale.Api.Services
{
    public interface IPointOfSaleService 
    {        
        Task<List<SaasTenant>> VendorList();
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

        public async Task<List<SaasTenant>> VendorList()
        {
            return await Task.Run(() =>
            {
                var result = _context.SaasTenants.Where(x=>x.ActivationState == (int)TenantActivationStatus.Active).OrderBy(x=>x.Name).ToList();

                return result;
            });            
        }
    }
}
