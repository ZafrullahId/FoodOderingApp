using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF_Core.Interface.Repositories;
using EF_Core.Interface.Services;
using EF_Core.Models.Dto;
using EF_Core.Models.DTOs.RequesteModels;
using EF_Core.Models.DTOs.ResponseModels;
using EF_Core.Models.Entity;

namespace EF_Core.Implimentations.Repositories
{

    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IFoodRepo _foodRepo;
        public OrderService(IOrderRepo orderRepo,ICustomerRepo customerRepo,IFoodRepo foodRepo)
        {
            _orderRepo = orderRepo;
            _customerRepo = customerRepo;
            _foodRepo = foodRepo;
        }

         public async Task<List<FoodOrder>> OrdersAsync()
        {
           var orders = await _orderRepo.OrdersAsync();
           return orders.OrderBy(x => x.CreatedAt).ToList();
        }

        public async Task<BaseResponse> CreateOrderAsync(CreateOrderRequestModel model,string email,int id)
        {
            var customer = await _customerRepo.GetByEmailAsync(email);
            var food = await _foodRepo.GetByIdAsync(id);
            if(customer == null)
            {
                return new BaseResponse
                {
                    Message = "Something went wrong",
                    Success = false,
                };
            }
            var order = new Order
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CustomerId = customer.Id,
                IsDelivered = model.IsDelivered,
                Address = new Address
                {
                    PostalCode = model.PostalCode,
                    City = model.City,
                    State = model.State,
                    Street = model.Street
                }
            };
            await _orderRepo.CreateAsync(order);
            var foodOrder = new FoodOrder
            {
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                FoodId = food.Id,
                OrderId = order.Id,
            };
            await _orderRepo.InsertAsync(foodOrder);
            return new BaseResponse
            {
                Message = "Successfully Ordered",
                Success = true
            };
        }
        // public async Task<BaseResponse> CreateFoodOrder()
        // {

        // }
        public async Task<BaseResponse> CalculatePriceAsync(string email, decimal price)
        {
            var customer = await _customerRepo.GetByEmailAsync(email);
            if(customer.Wallet < price)
            {
                return new BaseResponse
                {
                    Message = "Insuficient Balance",
                    Success = false
                };
            }
            customer.Wallet -= price;
            await _customerRepo.UpdateAsync(customer);
                return new BaseResponse
                {
                    Message = $"Your Balance is {customer.Wallet}",
                    Success = true
                };
        }

        public async Task<OrderResponseModel> GetOrderById(int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            var d = order.Address.Id;
            if(order == null)
            {
                return new OrderResponseModel
                {
                    Message = "Order Successfully found",
                    Success = false,
                };
            }
            return new OrderResponseModel
            {
                    Data = new OrderDto
                    {
                        IsDelivered = order.IsDelivered,
                        State = order.Address.State,
                        City = order.Address.City,
                        Street = order.Address.Street,
                        PostalCode = order.Address.PostalCode,
                        customerDto = new CustomerDto
                        {
                            Id = order.Customer.Id,
                            NextOfKin = order.Customer.NextOfKin,
                            Gender = order.Customer.Gender,
                            Wallet = order.Customer.Wallet,
                             User = new UserDto
                            {
                                Name = $"{order.Customer.User.LastName} {order.Customer.User.FirstName}",
                                Email = order.Customer.User.Email,
                                PhoneNumber = order.Customer.User.PhoneNumber,
                            }
                        }
                    }
            };
        }

        public async Task<BaseResponse> UpdateStatus(int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            if(order == null)
            {
                return new BaseResponse
                {
                    Message = "Order not found",
                    Success = false
                };
            }
            order.IsDelivered = true;
            await _orderRepo.UpdateAsync(order);
            return new BaseResponse
            {
                Message = "Order updated Successfully",
                Success = true,
            };
        }
    }
}