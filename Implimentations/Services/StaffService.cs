using System;
using EF_Core.Models.Dto;
using EF_Core.Models.Enum;
using System.Linq;
using System.Collections.Generic;
using EF_Core.Models.Entity;
using System.Threading.Tasks;
using EF_Core.Interface.Services;
using EF_Core.Interface.Repositories;
using EF_Core.Models.DTOs.ResponseModels;
using EF_Core.Models.DTOs.RequesteModels;

namespace EF_Core.Implimentations.Repositories
{

    public class StaffService : IStaffService
    {
        private readonly IStaffRepo _staffRepo;
        public StaffService(IStaffRepo staffRepo)
        {
            _staffRepo = staffRepo;
        }
        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var staff = await _staffRepo.GetByIdAsync(id);
            if (staff == null)
            {
                return new BaseResponse
                {
                    Message = "Staff Not Found",
                    Success = false
                };
            }

            await _staffRepo.DeleteAsync(staff);
                return new BaseResponse
                {
                    Message = "Staff Deleted Successfully",
                    Success = true
                };
        }

        public async Task<BaseResponse> EditAsync(int id, UpdateStaffRequestModel model)
        {
             var staff = await _staffRepo.GetByIdAsync(id);
            if (staff == null)
            {
                return new BaseResponse
                {
                    Message = "Staff Profile Not Found",
                    Success = false,
                };
            }
            model.NextOfKin = staff.NextOfKin;
            model.DathOfBirth = staff.DateOfBirth;
            model.Gender = staff.Gender;
            model.Role = staff.Role;
            model.FirstName = staff.User.FirstName;
            model.LastName = staff.User.LastName;
            model.PhoneNumber = staff.User.PhoneNumber;
            model.PostalCode = staff.User.Address.PostalCode;
            model.NumberLine = staff.User.Address.NumberLine;
            model.Street = staff.User.Address.Street;
            model.State = staff.User.Address.State;
            model.City = staff.User.Address.City;
            model.Country = staff.User.Address.Country;
            var response = await _staffRepo.UpdateAsync(staff);
            return new BaseResponse
            {
                    Message = "Staff Profile Updated",
                    Success = true
            };
        }

        public async Task<StaffResponsModel> FindAsync(string email, string password)
        {
            var staff = await _staffRepo.GetByEmailAsync(email);
            if (staff != null && staff.User.Password == password && staff.Role == Role.Admin)
            {
                return new StaffResponsModel
                {
                    Message = "Successfully LogIn",
                    Success = true,
                    Data = new StaffDto
                    {
                        NextOfKin = staff.NextOfKin,
                        DathOfBirth = staff.DateOfBirth,
                        Gender = staff.Gender,
                        Role = staff.Role,
                        User = new UserDto
                        {
                            Name = $"{staff.User.FirstName} {staff.User.LastName}",
                            Email = staff.User.Email,
                            PhoneNumber = staff.User.PhoneNumber,
                            City = staff.User.Address.City,
                            Country = staff.User.Address.Country,
                            State = staff.User.Address.State,
                            NumberLine = staff.User.Address.NumberLine,
                            PostalCode = staff.User.Address.PostalCode,
                            Street = staff.User.Address.Street
                        }
                    }
                 };
            }
            
                return new StaffResponsModel
                {
                    Message = "Invalid Login Credential",
                    Success = false
                };
        }

        public async Task<StaffResponsModel> FindByIdAsync(int id)
        {
           var staff = await _staffRepo.GetByIdAsync(id);
           if(staff == null)
           {
                return new StaffResponsModel
                {
                    Message = "Staff Not Found",
                    Success = false
                };
           }
            return new StaffResponsModel
            {
                Message = "Staff Found",
                Success = true,
                Data = new StaffDto
                {
                    NextOfKin = staff.NextOfKin,
                    DathOfBirth = staff.DateOfBirth,
                    Gender = staff.Gender,
                    User = new UserDto
                    {
                        Name = $"{staff.User.FirstName} {staff.User.LastName}",
                        Email = staff.User.Email,
                        PhoneNumber = staff.User.PhoneNumber,
                        City = staff.User.Address.City,
                        Country = staff.User.Address.Country,
                        State = staff.User.Address.State,
                        NumberLine = staff.User.Address.NumberLine,
                        PostalCode = staff.User.Address.PostalCode,
                        Street = staff.User.Address.Street
                    }
                }
            };
        }

        public async Task<StaffResponsModel> FindByRoleAsync(Role role)
        {
           var staff = await _staffRepo.GetByRoleAsync(role);
           if(staff == null)
           {
                return new StaffResponsModel
                {
                    Message = "Staff Not Found",
                    Success = false
                };
           }
            return new StaffResponsModel
            {
                Message = "Staff Found",
                Success = true,
                Data = new StaffDto
                {
                    NextOfKin = staff.NextOfKin,
                    DathOfBirth = staff.DateOfBirth,
                    Gender = staff.Gender,
                    User = new UserDto
                    {
                        Name = $"{staff.User.FirstName} {staff.User.LastName}",
                        Email = staff.User.Email,
                        PhoneNumber = staff.User.PhoneNumber,
                        City = staff.User.Address.City,
                        Country = staff.User.Address.Country,
                        State = staff.User.Address.State,
                        NumberLine = staff.User.Address.NumberLine,
                        PostalCode = staff.User.Address.PostalCode,
                        Street = staff.User.Address.Street
                    }
                }
            };
        }

        public async Task<List<StaffResponsModel>> GetAllAsync()
        {
             var staff = await _staffRepo.ListAsync();
            if (staff == null)
            {
                return null;
            }
            return staff.Select(staff => new StaffResponsModel
            {
                Message = "List Of Staffs",
                Success = true,
                Data = new StaffDto
                {
                    Id = staff.Id,
                    NextOfKin = staff.NextOfKin,
                    DathOfBirth = staff.DateOfBirth,
                    Gender = staff.Gender,
                    Role = staff.Role,
                    User = new UserDto
                    {

                        Name = $"{staff.User.LastName} {staff.User.FirstName}",
                        Email = staff.User.Email,
                        PhoneNumber = staff.User.PhoneNumber,
                        City = staff.User.Address.City,
                        Country = staff.User.Address.Country,
                        State = staff.User.Address.State,
                        NumberLine = staff.User.Address.NumberLine,
                        PostalCode = staff.User.Address.PostalCode,
                        Street = staff.User.Address.Street
                    }
                }
            }).ToList();
                
        }

        public async Task<BaseResponse> RegisterAsync(CreateStaffRequestModel model)
        {
             var staff = new Staff()
            {
                NextOfKin = model.NextOfKin,
                DateOfBirth = model.DathOfBirth,
                Gender = model.Gender,
                Role = model.Role,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                User = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password,
                    Address = new Address()
                    {
                        CreatedAt = DateTime.Now,
                        PostalCode = model.PostalCode,
                        NumberLine = model.NumberLine,
                        Street = model.Street,
                        City = model.City,
                        State = model.State,
                        Country = model.Country,
                    }
                }
            };
            await _staffRepo.CreateAsync(staff);
            return new BaseResponse
            {
                Message = "Staff Successfully Added",
                Success = true,
            };
        }

        public async Task<StaffResponsModel> VeiwProfileAsync(int id)
        {
           var staff = await _staffRepo.GetByIdAsync(id);
           if(staff == null)
           {
                return new StaffResponsModel
                {
                    Message = "Staff Not Found",
                    Success = false
                };
           }
            return new StaffResponsModel
            {
                Message = "Staff Found",
                Success = true,
                Data = new StaffDto
                {
                    NextOfKin = staff.NextOfKin,
                    DathOfBirth = staff.DateOfBirth,
                    Gender = staff.Gender,
                    User = new UserDto
                    {
                        Name = $"{staff.User.FirstName} {staff.User.LastName}",
                        Email = staff.User.Email,
                        PhoneNumber = staff.User.PhoneNumber,
                        City = staff.User.Address.City,
                        Country = staff.User.Address.Country,
                        State = staff.User.Address.State,
                        NumberLine = staff.User.Address.NumberLine,
                        PostalCode = staff.User.Address.PostalCode,
                        Street = staff.User.Address.Street
                    }
                }
            };
        }
    }
}