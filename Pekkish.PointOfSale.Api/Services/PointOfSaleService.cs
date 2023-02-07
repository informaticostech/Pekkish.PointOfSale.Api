﻿using Microsoft.IdentityModel.Tokens;
using Pekkish.PointOfSale.DAL.Entities;
using System.Dynamic;
using static Pekkish.PointOfSale.Api.Models.PointOfSale.Constants;
using static Pekkish.PointOfSale.Api.Models.Wati.Constants;

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
        Task<dynamic> OrderSave(int orderId, decimal total, List<AppWatiOrderDetail> productList);
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

        #region Vendor
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
        #endregion

        #region Location
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
        #endregion

        #region Brand
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
        #endregion

        #region Product Category
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
                    item.Name = item.Name.Replace('&', '_');
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
        #endregion

        #region Product
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
                    item.Name = item.Name.Replace('&', '_');
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
        #endregion

        #region Order
        public async Task<dynamic> OrderSave(int orderId, decimal total, List<AppWatiOrderDetail> productList)
        {
            dynamic result = new ExpandoObject();

            try
            {                
                var watiOrder = _context.AppWatiOrders.Single(x => x.Id == orderId);
                var watiConversation = _context.AppWatiConversations.Single(x => x.Id == watiOrder.WatiConversationId);
                var orderDetail = new AppOrderDetail();

                #region POS Order
                var order = new AppOrder();
                order.TenantId = watiOrder.TenantId;
                order.Brand = "TBC";
                order.LocationId = watiOrder.LocationId;
                order.OrderStatusId = (int)PosOrderStatusEnum.Pending;
                order.SalesChannelId = (int)PosSalesChannelEnum.WhatsAppPekkish;
                order.PaymentMethodId = (int)PosPaymentTypeEnum.PayLater;
                order.OrderFulfillmentId = (int)PosFulfillmentTypeEnum.Pickup;      //test
                order.EffectiveDate = DateTime.Now.AddMinutes(20);
                order.SubTotal = total;
                order.DeliveryFee = 0;
                order.DriverTip = 0;    //test
                order.TaxRatePerc = 0;
                order.PaidCash = 0;     //test
                order.PaidCard = 0;     //test
                order.PaidEft = 0;      //test
                order.PaidOnline = 0;   //test
                order.PaidTotal = 0;
                order.PaidChange = 0;
                order.ExternalId = watiOrder.WaId.ToString();   //test
                order.ExternalCustomerName = watiOrder.Name;
                order.IsMultiBrand = false;
                order.CreationTime = DateTime.Now;
                order.IsDeleted = false;

                _context.AppOrders.Add(order);
                _context.SaveChanges();
                #endregion

                #region Wati Clean Up
                watiOrder.PosOrderId = order.Id;
                watiOrder.WatiOrderStatusId = (int)WatiFoodOrderStatusEnum.Completed;

                watiConversation.CompletedDate = DateTime.Now;
                watiConversation.WatiConversationStatusId = (int)WatiConversationStatusEnum.Completed;
                _context.SaveChanges();
                #endregion
                               
                #region OrderBrand
                List<int> brandIdList = (from list in productList select list.BrandId).Distinct().ToList();
                var brandListDb = _context.AppBrands.ToList();
                int brandCounter = 0;
                AppBrand brand;

                foreach (var item in brandIdList)
                {
                    //Logistics
                    var brandCartList = productList.Where(x => x.BrandId == item).ToList();
                    brand = brandListDb.Single(x => x.Id == item);
                    brandCounter = brandCounter + 1;

                    //Order Brand
                    var orderBrand = new AppOrderBrand();
                    orderBrand.OrderId = order.Id;
                    orderBrand.BrandId = item;
                    _context.AppOrderBrands.Add(orderBrand);
                    _context.SaveChanges();

                    #region Order Request
                    var orderRequest = new AppOrderRequest();
                    orderRequest.TenantId = order.TenantId;
                    orderRequest.OrderId = order.Id;
                    orderRequest.Iteration = 1;
                    orderRequest.OrderStatusId = (int)PosOrderStatusEnum.Pending;
                    orderRequest.Brand = brand.Name;                    
                    orderRequest.IsMultiBrand = false;
                    orderRequest.CreationTime = DateTime.Now;
                    orderRequest.IsDeleted = false;

                    foreach (var cartItem in brandCartList)
                    {
                        orderRequest.Total = orderRequest.Total + (cartItem.Amount * cartItem.Quantity);
                    }

                    _context.AppOrderRequests.Add(orderRequest);
                    _context.SaveChanges();
                    #endregion

                    var orderRequestBrand = new AppOrderRequestBrand();
                    orderRequestBrand.OrderId = order.Id;
                    orderRequestBrand.BrandId = item;
                    orderRequestBrand.OrderRequestId = orderRequest.Id;
                    _context.AppOrderRequestBrands.Add(orderRequestBrand);
                    _context.SaveChanges();

                    #region Products
                    foreach (var product in productList)
                    {
                        orderDetail = new AppOrderDetail();
                        orderDetail.TenantId = order.TenantId;
                        orderDetail.Orderid = order.Id;
                        orderDetail.OrderRequestCrono = 1;
                        orderDetail.OrderRequestId = orderRequest.Id;
                        orderDetail.Name = product.Name;
                        orderDetail.Amount = product.Amount;
                        orderDetail.AmountBase = product.Amount;
                        orderDetail.Quantity = product.Quantity;
                        orderDetail.Comment = product.Comment;
                        orderDetail.ProductId = product.ProductId;
                        orderDetail.BrandId = product.BrandId;
                        orderDetail.AmountNormal = product.Amount;
                        orderDetail.AmountBaseNormal = product.Amount;
                        orderDetail.RateIncrease = 0;
                        orderDetail.DiscountRate = 0;
                        orderDetail.DiscountValue = 0;
                        orderDetail.AmountNoDiscount = 0;
                        orderDetail.CreationTime = DateTime.Now;
                        orderDetail.IsDeleted = false;
                        _context.AppOrderDetails.Add(orderDetail);
                        _context.SaveChanges();

                        //TODO Product Extra List
                    }
                    #endregion
                }

                order.Brand = await OrderBrandGet(order.Id);

                order.IsMultiBrand = await OrderIsMultiBrandGet(order.Id);

                _context.SaveChanges();
                #endregion

                result.success = true;
                result.orderId = order.Id;

                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.error = ex.Message;

                return result;
            }
        }
        public async Task<string> OrderBrandGet(int orderId)
        {
            return await Task.Run(() =>
            {
                string returnValue = "";
                AppOrder order = _context.AppOrders.Single(x => x.Id == orderId);
                List<AppOrderBrand> orderBrandList = _context.AppOrderBrands.Where(x => x.OrderId == orderId).ToList();
                List<AppBrand> brandList = _context.AppBrands.Where(x => x.TenantId == order.TenantId).ToList();

                if (orderBrandList.Count == 1)
                    returnValue = brandList.SingleOrDefault(x => x.Id == orderBrandList[0].BrandId)?.NameShort;
                else
                {
                    for (int c = 0; c < orderBrandList.Count; c++)
                    {
                        returnValue += $"{brandList.SingleOrDefault(x => x.Id == orderBrandList[c].BrandId)?.NameShort}";

                        if (c != (orderBrandList.Count - 1))
                            returnValue += " | ";
                    }
                }

                return returnValue;
            });
        }
        public async Task<bool> OrderIsMultiBrandGet(int orderId)
        {
            return await Task.Run(() =>
            {
                bool returnValue = false;

                List<AppOrderBrand> orderBrandList = _context.AppOrderBrands.Where(x => x.OrderId == orderId).ToList();

                if (orderBrandList.Count > 1)
                    returnValue = true;

                return returnValue;
            });
        }
        #endregion
    }
}
