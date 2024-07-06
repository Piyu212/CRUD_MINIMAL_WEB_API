using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Minimal_API;
using Minimal_API.Data;
using Minimal_API.Models;
using Minimal_API.Models.DTO;
using System.Net;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingConfig)); // Register AutoMapper
builder.Services.AddValidatorsFromAssemblyContaining<Program>(); // Register FluentValidation

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal_API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the root
    });
}
app.UseHttpsRedirection();

app.MapGet("/api/coupon", (ILogger<Program> _logger) => {
    APIResponse response = new();
    _logger.Log(LogLevel.Information, "Getting all coupons");
    response.Result = CouponStore.couponList;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);
}).WithName("GetCoupon").Produces<APIResponse>(200);

// Retrieve individual coupon based on ID
app.MapGet("/api/coupon/{id:int}", (ILogger<Program> _logger, int id) => {
    APIResponse response = new();
    response.Result = CouponStore.couponList.FirstOrDefault(u => u.Id == id);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);
}).WithName("GetCoupon").Produces<APIResponse>(200);

// POST Method - To create Coupon
app.MapPost("/api/coupon", async (IMapper _mapper,
    
    IValidator<CouponCreateDTO> _validator, [FromBody] CouponCreateDTO Coupon_C_DTO) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
    var validationResult = await _validator.ValidateAsync(Coupon_C_DTO);

    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }

    if (CouponStore.couponList.FirstOrDefault(u => u.Name.ToLower() == Coupon_C_DTO.Name.ToLower()) != null)
    {
        response.ErrorMessages.Add("Coupon Name already exists");
        return Results.BadRequest(response);
    }

    Coupon  coupon= _mapper.Map<Coupon>(Coupon_C_DTO);

    coupon.Id = CouponStore.couponList.OrderByDescending(u => u.Id).FirstOrDefault()?.Id + 1 ?? 1;
    CouponStore.couponList.Add(coupon);
    CouponDTO couponDTO = _mapper.Map<CouponDTO>(coupon);

    response.Result = couponDTO;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.Created;
    return Results.Ok(response);
}).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<APIResponse>(201).Produces(400);

// PUT Method - To update Coupon
app.MapPut("/api/coupon", async (IMapper _mapper, 
    IValidator<CouponUpdateDTO> _validator, [FromBody] CouponUpdateDTO Coupon_U_DTO) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
    var validationResult = await _validator.ValidateAsync(Coupon_U_DTO);

    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }

   Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.Id == Coupon_U_DTO.Id);
    couponFromStore.IsActive = Coupon_U_DTO.IsActive;
    couponFromStore.Name = Coupon_U_DTO.Name;
    couponFromStore.Percent = Coupon_U_DTO.Percent;
    couponFromStore.LastUpdated = DateTime.Now;

    response.Result = _mapper.Map<CouponDTO>(couponFromStore);//CouponStore.couponList.FirstOrDefault(u => u.Id == id);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);

}).WithName("UpdateCoupon").Accepts<CouponUpdateDTO>("application/json").Produces<APIResponse>(200).Produces(400);

// DELETE Method - To delete Coupon
app.MapDelete("/api/coupon/{id:int}", (int id) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };


    Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.Id == id);
    if (couponFromStore != null)
    {
        CouponStore.couponList.Remove(couponFromStore);
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.NoContent;
        return Results.Ok(response);
    }
    else
    {
        response.ErrorMessages.Add("Invalid Id");
        return Results.BadRequest(response);
    }
});


app.Run();

