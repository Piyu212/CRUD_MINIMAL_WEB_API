
using CRUD_Minimal_API.Data;
using CRUD_Minimal_API.Models;
using CRUD_Minimal_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FluentValidation;
using System.Net;

namespace CRUD_Minimal_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //First End Point MapGet that will get all the coupons
            app.MapGet("/api/coupon", (ILogger<Program> _logger) => {
                APIResponse response = new();
                _logger.Log(LogLevel.Information, "Getting all the coupons");
                response.Result = CouponStore.couponList;
                response.IsSucess = true;
                response.StatusCode = HttpStatusCode.OK;
               return Results.Ok(response);
                }).WithName("GetCoupons").Produces<APIResponse>(200);

            // End Point MapGet that will retrive coupons based on id
            app.MapGet("/api/coupon.{id:int}", (ILogger<Program> _logger, int id) => {
                APIResponse response = new() { IsSucess = false,  StatusCode = HttpStatusCode.BadRequest };
                response.Result = CouponStore.couponList.FirstOrDefault(u => u.Id == id);
                response.IsSucess = true;
                response.StatusCode = HttpStatusCode.OK;
                return Results.Ok(response);  //FirstOrDefault helps find the first item in a list that matches a condition.
            }).WithName("GetCoupon").Produces<APIResponse>(200);

            // Creating POST end point here creating coupon
            app.MapPost("/api/coupon", async (IMapper _mapper,
                IValidator<CouponCreateDTO> _validation, [FromBody] CouponCreateDTO coupon_C_DTO) =>
            {
                APIResponse response = new() { IsSucess = false, StatusCode = HttpStatusCode.BadRequest };

                var validationResult = await _validation.ValidateAsync(coupon_C_DTO);
                // Check if the coupon has an invalid Id or name
                if (!validationResult.IsValid)  //If the coupon's Id is not 0 or the name is missing,
                {
                    response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
                    return Results.BadRequest(response); //it returns a 400 Bad Request with a message "Invalid Id or coupon name".
                }

                //validation for - do not want to created same coupon again
                if (CouponStore.couponList.FirstOrDefault(u => u.Name.ToLower() == coupon_C_DTO.Name.ToLower()) != null)
                {
                    response.ErrorMessages.Add("Coupon name alredy exists");
                    return Results.BadRequest(response);

                }

                //Converting Coupon to DTO
                Coupon coupon = _mapper.Map<Coupon>(coupon_C_DTO); //it automatically may coupon to coupon dt

                coupon.Id = CouponStore.couponList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1; // Assign a new unique Id to the coupon
                CouponStore.couponList.Add(coupon);  // Add the new coupon to the list
                CouponDTO couponDTO = _mapper.Map<CouponDTO>(coupon);

                response.Result = couponDTO;
                response.IsSucess = true;
                response.StatusCode = HttpStatusCode.Created;
                return Results.Ok(response);

                //return Results.CreatedAtRoute("GetCoupon",new { Id = coupon.Id } ,couponDTO);  //CreatedAtRoute tells the client that the coupon was created and where to find it.
                //return Results.Created($"/api/coupon/{coupon.Id}", coupon);  // Return the newly created coupon as the response
            }).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<APIResponse>(201).Produces(400);

            // Creating PUT end point here Updating coupon
            app.MapPut("/api/coupon", async (IMapper _mapper,
               IValidator<CouponUpdateDTO> _validation, [FromBody] CouponUpdateDTO coupon_U_DTO) =>
            {
                APIResponse response = new() { IsSucess = false, StatusCode = HttpStatusCode.BadRequest };

                var validationResult = await _validation.ValidateAsync(coupon_U_DTO);
                // Check if the coupon has an invalid Id or name
                if (!validationResult.IsValid)  //If the coupon's Id is not 0 or the name is missing,
                {
                    response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
                    return Results.BadRequest(response); //it returns a 400 Bad Request with a message "Invalid Id or coupon name".
                }

                Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.Id == coupon_U_DTO.Id); //retriving coupon based on id
                couponFromStore.IsAactive = coupon_U_DTO.IsAactive;
                couponFromStore.Name = coupon_U_DTO.Name;
                couponFromStore.Percent = coupon_U_DTO.Percent;
                couponFromStore.LastUpdated = DateTime.Now;

                response.Result = _mapper.Map<CouponDTO>(couponFromStore);
                response.IsSucess = true;
                response.StatusCode = HttpStatusCode.OK;
                return Results.Ok(response);
            }).WithName("UpdateCoupon").Accepts<CouponUpdateDTO>("application/json").Produces<APIResponse>(200).Produces(400); ;

            // Creating DELETE end point here deleting coupon
            app.MapDelete("/api/coupon.{id:int}", (int id) => {
                APIResponse response = new() { IsSucess = false, StatusCode = HttpStatusCode.BadRequest };

                Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.Id == id); //retriving coupon based on id
                if(couponFromStore!=null)
                {
                    CouponStore.couponList.Remove(couponFromStore);
                    response.IsSucess = true;
                    response.StatusCode = HttpStatusCode.NoContent;
                    return Results.Ok(response);
                }
                else
                {
                    response.ErrorMessages.Add("Invalid id");
                    return Results.BadRequest(response);

                }
             
            });

            app.UseHttpsRedirection();
            app.Run();
        }
    }
}


/* POST and PUT madhe will received coupon object and and parameters that pay we can
 create yet are added the coupon,
but jenva apn coupon delte karto we receive id 
 */

//The Produces method in ASP.NET Core and minimal APIs is crucial for specifying the expected response types and status codes

/*
 A logger is a utility that helps developers track what their programs are doing while they run. 
It's like a diary that records important events, errors, or other noteworthy actions within the software.
 */

//AutoMapper: This is a library that helps in automatically mapping properties from one object to another.