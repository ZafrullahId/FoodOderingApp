using System;
using EF_Core.Models.Dto;
using System.Collections.Generic;
using System.Linq;
using EF_Core.Models.Entity;
using System.Threading.Tasks;
using EF_Core.Interface.Services;
using EF_Core.Interface.Repositories;
using EF_Core.Models.DTOs.ResponseModels;
using EF_Core.Models.DTOs.RequesteModels;

namespace EF_Core.Implimentations.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;
        public CustomerService(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }
        public async Task<BaseResponse> DeleteAsync(string email)
        {
            var customer = await _customerRepo.GetByEmailAsync(email);
            if (customer == null)
            {
                return new BaseResponse
                {
                    Message = "Account not found",
                    Success = false
                };
            }
             await _customerRepo.DeleteAsync(customer.User.Email);
             return new BaseResponse
            {
              Message = "Your Account has being Deleted",
              Success = true
            };
        }
        

        public async Task<CustomerResponsModel> CheckWalletAsync(string email)
        {
            var customer = await _customerRepo.GetByEmailAsync(email);
            if(customer == null)
            {
                return new CustomerResponsModel
                {
                    Message = "Your Wallet was not found",
                    Success = false
                };
            }
            return new CustomerResponsModel
            {
                Message = $"Your Wallet balance is #{customer.Wallet}",
                Success = true,
                Data = new CustomerDto
                {
                    Wallet = customer.Wallet
                }
            };
        }

        public async Task<BaseResponse> EditAsync(string email, UpdateCustomerRequestModel model)
        {
             var customer = await _customerRepo.GetByEmailAsync(email);
            if (customer == null)
            {
                return new BaseResponse
                {
                    Message = "Profile Not Found",
                    Success = false
                };
            }
            customer.NextOfKin = model.NextOfKin;
            customer.Gender = model.Gender;
            customer.User.FirstName = model.FirstName;
            customer.User.LastName = model.LastName;
            customer.User.PhoneNumber = model.PhoneNumber;
            customer.UpdatedAt = DateTime.Now;
            var response = await _customerRepo.UpdateAsync(customer); 
                return new BaseResponse
                {
                    Message = "Profile Successfully Updated",
                    Success = true
                };
        }

        public async Task<CustomerResponsModel> FindAsync(string email, string password)
        {
            
            var customer = await _customerRepo.GetByEmailAsync(email);
            if (customer != null && customer.User.Password == password)
            {
                return new CustomerResponsModel()
                {
                     Message = "Valid Login Credentials",
                    Success = true,
                    Data = new CustomerDto
                    {
                        Id = customer.Id,
                        NextOfKin = customer.NextOfKin,
                        Gender = customer.Gender,
                        Wallet = customer.Wallet,
                        User = new UserDto
                        {
                            
                            Name = $"{customer.User.LastName} {customer.User.FirstName}",
                            Email = customer.User.Email,
                            PhoneNumber = customer.User.PhoneNumber,
                        }
                    }
                };
            }
                return new CustomerResponsModel
                {
                    Message = "Invalid Login Credentials",
                    Success = false
                };
               
        }

        public async Task<CustomerResponsModel> FindByEmailAsync(string email)
        {
            var customer = await _customerRepo.GetByEmailAsync(email);
            if (customer != null)
            {
                return new CustomerResponsModel
                {
                     Message = "Found",
                    Success = true,
                    Data = new CustomerDto
                    {
                        Id = customer.Id,
                        NextOfKin = customer.NextOfKin,
                        Gender = customer.Gender,
                        Wallet = customer.Wallet,
                        User = new UserDto
                        {
                            Name = $"{customer.User.LastName} {customer.User.FirstName}",
                            Email = customer.User.Email,
                            PhoneNumber = customer.User.PhoneNumber,
                        }
                    }
                };
            }
                return new CustomerResponsModel
                {
                    Message = "Not Found",
                    Success = false
                };
               
        }

        public async Task<List<CustomerResponsModel>> GetAllAsync()
        {
            var customers = await _customerRepo.ListAsync();
           return customers.Select(customer => new CustomerResponsModel
            {
                Data = new CustomerDto
                {
                        Id = customer.Id,
                        NextOfKin = customer.NextOfKin,
                        Gender = customer.Gender,
                     User = new UserDto()
                     {
                        Name = $"{customer.User.LastName} {customer.User.FirstName}",
                        Email = customer.User.Email,
                        PhoneNumber = customer.User.PhoneNumber,
                     }
                }
            }).ToList();
        }

        public async Task<BaseResponse> RegisterAsync(CreateCustomerRequestModel model)
        {
            var check = await _customerRepo.CheckIfExist(model.Email);
            if(check == true)
            {
                return new BaseResponse
                {
                    Message = "Email Exists Already",
                    Success = false
                };
            }
            var customer = new Customer
            {
                NextOfKin = model.NextOfKin,
                Wallet = 0.00m,
                Gender = model.Gender,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                User = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }
            };
            await _customerRepo.CreateAsync(customer);
            return new BaseResponse
            {
                Message = "Successfully Registered",
                Success = true,
            };
        }

        public async Task<BaseResponse> UpdateWalletAsync(string email, UpdateCustomerRequestModel model)
        {
             var customer = await _customerRepo.GetByEmailAsync(email);
            customer.Wallet += model.Wallet;
             await _customerRepo.UpdateAsync(customer);

             return new BaseResponse
             {
                Message = "Wallet Updated Successfully",
                Success = false,
             };
        }

        public async Task<CustomerResponsModel> VeiwProfileAsync(string email)
        {
             var customer = await _customerRepo.GetByEmailAsync(email);
             if(customer == null)
             {
                return new CustomerResponsModel
                {
                    Message = "Profile not found",
                    Success = false
                };
             }

            return new CustomerResponsModel
            {
                Message = "Profile was found",
                Success = true,
                Data = new CustomerDto
                {
                    NextOfKin = customer.NextOfKin,
                    Gender = customer.Gender,
                    Wallet = customer.Wallet,
                    User = new UserDto()
                    {
                        Name = customer.User.FirstName + " " + customer.User.LastName,
                        Email = customer.User.Email,
                        PhoneNumber = customer.User.PhoneNumber
                    }
                }
                
            };
        }


        public async Task<BaseResponse> FundWalletAsync(string email, decimal amount)
        {
            var customer = await _customerRepo.GetByEmailAsync(email);
            customer.Wallet += amount;
            await _customerRepo.UpdateAsync(customer);
            return new BaseResponse
            {
                Message = "Wallet Successfully Funded",
                Success = false
            };
        }
    }
}
