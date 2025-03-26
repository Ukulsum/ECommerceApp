using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.AddressesDTOs;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Services
{
    public class AddressService
    {
        private readonly ApplicationDbContext _context;
        public AddressService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ApiResponse<AddressResponseDTO>> CreateAddressAsync(AddressResponseDTO addressDto)
        {
            try
            {
                // Check if Customer exists
                var customer = await _context.Customers.FindAsync(addressDto.CustomerId);
                if (customer == null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Customer not found.");
                }

                // Manual mapping from DTO to Model
                var address = new Address
                {
                    CustomerId = addressDto.Id,
                    AddressLine1 = addressDto.AddressLine1,
                    AddressLine2 = addressDto.AddressLine2,
                    City = addressDto.City,
                    State = addressDto.State,
                    PostalCode = addressDto.PostalCode,
                    Country = addressDto.Country,
                };

                // Add address to the database
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                // Map to AddressResponseDTO
                var addressResponse = new AddressResponseDTO
                {
                    Id = address.Id,
                    CustomerId = address.CustomerId,
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    Country= address.Country,
                };

                return new ApiResponse<AddressResponseDTO>(200, addressResponse);
            }
            catch(Exception ex)
            {
                // Log the exception
                return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }


        public async Task<ApiResponse<AddressResponseDTO>> GetAddressByIdAsync(int id)
        {
            try
            {
                var address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(add => add.Id == id);

                if (address == null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Address not found.");
                }

                //Map to AddressResponseDTO
                var addressResponse = new AddressResponseDTO
                {
                    Id = address.Id,
                    CustomerId = address.CustomerId,
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City= address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    Country = address.Country,
                };

                return new ApiResponse<AddressResponseDTO>(200, addressResponse);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }


        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateAddressAsync(AddressUpdateDTO addressDto)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(add => add.Id == addressDto.AddressId && add.CustomerId == addressDto.CustomerId);

                if (address == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found.");
                }


                // Update address properties
                address.AddressLine1 = addressDto.AddressLine1;
                address.AddressLine2 = addressDto.AddressLine2;
                address.City = addressDto.City;
                address.State = addressDto.State;
                address.PostalCode = addressDto.PostalCode;
                address.Country = addressDto.Country;

                await _context.SaveChangesAsync();

                //Prepare confirmation message
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Address with Id {addressDto.AddressId} updated successfully."
                };
                
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
    }
}
