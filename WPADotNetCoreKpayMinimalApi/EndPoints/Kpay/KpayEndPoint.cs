using WPADotNetCore.Databases.Models;
using WPADotNetCore.Domain.Kpay;
using WPADotNetCore.Databases.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WPADotNetCore.KpayMinimalApi.EndPoints.Kpay
{
    public static class KpayEndPoint
    {
        public static void MapKpayEndPoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/registration", ([FromBody] UserViewModel userViewModel,KpayService _kpayService) =>
            {                
                var result = _kpayService.Registration(userViewModel);
                return Results.Ok(result);

            }).WithOpenApi();

            app.MapPost("/api/login", (LoginRequestModel loginRequestModel, KpayService _kpayService) =>
            {
               
                var result = _kpayService.Login(loginRequestModel);
                return Results.Ok(result);
            }).WithOpenApi();

            app.MapPost("/api/deposit", (DepositRequestModel deposit, KpayService _kpayService) =>
            {
                
                var result= _kpayService.Deposit(deposit);
                return Results.Ok(result);
            }).WithOpenApi();

            app.MapPost("/api/withdraw", (WithDrawRequestModel withDraw, KpayService _kpayService) =>
            {
               
                var result = _kpayService.WithDraw(withDraw);
                return Results.Ok(result);
            }).WithOpenApi();

            app.MapPost("/api/get-balance", ([FromBody] UserViewModel userViewModel, KpayService _kpayService) =>
            {
               
                var result = _kpayService.GetUserBalance(userViewModel);
                return Results.Ok(result);
            }).WithOpenApi();

            app.MapPost("/api/transfer", (TransferRequestModel transferRequest, KpayService _kpayService) =>
            {
               
                var result = _kpayService.Transfer(transferRequest);
                return Results.Ok(result);
            }).WithOpenApi();

            app.MapPost("/api/phone-no-change", (PhoneNoChangeRequestModel phoneNoChangeRequest, KpayService _kpayService) =>
            {
               
                var result = _kpayService.PhoneNumberChange(phoneNoChangeRequest);
                return Results.Ok(result);
            }).WithOpenApi();

            app.MapPost("/api/password-change", (PinChangeRequestModel pinChangeRequestModel, KpayService _kpayService) =>
            {
               
                var result = _kpayService.PinChange(pinChangeRequestModel);
                return Results.Ok(result);
            }).WithOpenApi();
        }       
    }
}
