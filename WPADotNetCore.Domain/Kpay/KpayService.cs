using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WPADotNetCore.Databases.Models;
using WPADotNetCore.Databases.ViewModels;
using WPADotNetCore.Domain.Models;

namespace WPADotNetCore.Domain.Kpay
{
    public class KpayService
    {
        private readonly AppDbContext _db = new AppDbContext();

        public async Task<Result<UserViewModel>> Registration(UserViewModel user)
        {
            if (user.MobileNo != null && user.FullName != null && user.Pin != null)
            {
                TblUser tblUser = new TblUser()
                {
                    FullName = user.FullName,
                    MobileNo = user.MobileNo,
                    Pin = user.Pin,
                    CreatedDate = DateTime.Now,
                };
                _db.TblUsers.Add(tblUser);
                int result = _db.SaveChanges();

                if (result > 0)
                    return Result<UserViewModel>.Success("Registration Success", user);
                return Result<UserViewModel>.Error("Error Occur");
            }
            else
            {
                return Result<UserViewModel>.ValidationError("Please Fill Information First");
            }
        }
        public Result<LoginRequestModel> Login(LoginRequestModel loginRequestModel)
        {
            if (loginRequestModel.FullName != null && loginRequestModel.Pin != null)
            {
                var result = _db.TblUsers.FirstOrDefault(x => x.FullName == loginRequestModel.FullName && x.Pin == loginRequestModel.Pin);
                if (result is not null)
                    return Result<LoginRequestModel>.Success("Login Success", loginRequestModel);
                else
                    return Result<LoginRequestModel>.Error("Unsuccess Login");
            }
            else
            {
                return Result<LoginRequestModel>.ValidationError("Please Fill Information First");
            }
        }
        public Result<DepositRequestModel> Deposit(DepositRequestModel depositRequestModel)
        {
            if (depositRequestModel.FullName != null && depositRequestModel.MobileNo != null && depositRequestModel.DepositAmount > 0)
            {
                var user = _db.TblUsers.FirstOrDefault(x => x.FullName == depositRequestModel.FullName && x.MobileNo == depositRequestModel.MobileNo);
                if (user != null)
                {
                    user.Balance += depositRequestModel.DepositAmount;
                    user.UpdatedDate = DateTime.Now;
                    _db.TblUsers.Update(user);
                    TblTransaction transaction = new TblTransaction()
                    {
                        UserId = user.Id,
                        Amount = depositRequestModel.DepositAmount,
                        Status = "Credit",
                        CreatedDate = DateTime.Now,
                        Description = "Deposit"
                    };
                    _db.TblTransactions.Add(transaction);
                    var result = _db.SaveChanges();
                    if (result > 0)
                        return Result<DepositRequestModel>.Success("Deposit Process Success", depositRequestModel);

                    else
                        return Result<DepositRequestModel>.Error("Error While Saving");
                }
                return Result<DepositRequestModel>.Error("User Not Found");
            }
            else
            {
                return Result<DepositRequestModel>.ValidationError("Please Fill Information First");
            }
        }
        public Result<WithDrawRequestModel> WithDraw(WithDrawRequestModel withDrawRequestModel)
        {
            if (withDrawRequestModel.FullName != null && withDrawRequestModel.WithDrawAmount > 0)
            {
                var user = _db.TblUsers.FirstOrDefault(x => x.FullName == withDrawRequestModel.FullName && x.MobileNo == withDrawRequestModel.MobileNo && x.Pin == withDrawRequestModel.Pin);
                if (user != null)
                {
                    if (user.Balance - withDrawRequestModel.WithDrawAmount < 1000)
                        return Result<WithDrawRequestModel>.ValidationError("Infufficient Balance");
                    user.Balance -= withDrawRequestModel.WithDrawAmount;
                    _db.TblUsers.Update(user);
                    TblTransaction transaction = new TblTransaction()
                    {
                        UserId = user.Id,
                        Amount = withDrawRequestModel.WithDrawAmount,
                        Status = "Debit",
                        CreatedDate = DateTime.Now,
                        Description = "WithDraw"
                    };
                    _db.TblTransactions.Add(transaction);
                    var result = _db.SaveChanges();
                    if (result > 0)
                    {
                        return Result<WithDrawRequestModel>.Success("Withdraw Process Success", withDrawRequestModel);
                    }

                    else
                        return Result<WithDrawRequestModel>.Error("User Not Found");
                }
                return Result<WithDrawRequestModel>.Error("Error While withdrawing");
            }
            else
            {
                return Result<WithDrawRequestModel>.ValidationError("Please Fill Information First");
            }
        }
        public Result<TransferRequestModel> Transfer(TransferRequestModel transferRequestModel)
        {
            var validationResult = TransferValidation(transferRequestModel);
            if (validationResult == "Success")
            {
                var fromTblUser = _db.TblUsers.FirstOrDefault(x => x.FullName == transferRequestModel.FromUser && x.Pin == transferRequestModel.Pin && x.MobileNo == transferRequestModel.FromMobileNo);
                if (fromTblUser == null)
                    return Result<TransferRequestModel>.ValidationError("User Not Found");

                var toTblUser = _db.TblUsers.FirstOrDefault(x => x.FullName == transferRequestModel.ToUser && x.MobileNo == transferRequestModel.ToMobileNo);
                if (toTblUser == null)
                    return Result<TransferRequestModel>.ValidationError("To User Not Found");

                if (fromTblUser.Balance - transferRequestModel.TransferBalance < 1000)
                {
                    return Result<TransferRequestModel>.ValidationError("Insufficient Balance");
                }
                fromTblUser.Balance -= transferRequestModel.TransferBalance;
                _db.TblUsers.Update(fromTblUser);

                TblTransaction transaction = new TblTransaction()
                {
                    UserId = fromTblUser.Id,
                    Amount = transferRequestModel.TransferBalance,
                    Status = "Debit",
                    CreatedDate = DateTime.Now,
                    Description = "Transfer"
                };

                _db.TblTransactions.Add(transaction);
                toTblUser.Balance += transferRequestModel.TransferBalance;
                _db.TblUsers.Update(toTblUser);


                TblTransaction transaction2 = new TblTransaction()
                {
                    UserId = toTblUser.Id,
                    Amount = transferRequestModel.TransferBalance,
                    Status = "Credit",
                    CreatedDate = DateTime.Now,
                    Description = "Transfer"
                };
                _db.TblTransactions.Add(transaction2);
                var result = _db.SaveChanges();
                if (result > 0)
                {
                    return Result<TransferRequestModel>.Success("Successfully Transfered", transferRequestModel);
                }
                return Result<TransferRequestModel>.Error("Error in transfer");
            }
            return Result<TransferRequestModel>.ValidationError(validationResult);
        }
        public Result<BalanceViewModel> GetUserBalance(UserViewModel userViewModel)
        {
            if (userViewModel.FullName != null && userViewModel.MobileNo != null && userViewModel.Pin != null)
            {
                var user = _db.TblUsers.FirstOrDefault(x => x.FullName == userViewModel.FullName && x.MobileNo == userViewModel.MobileNo && x.Pin == userViewModel.Pin);
                if (user != null)
                {
                    BalanceViewModel balance = new BalanceViewModel()
                    {
                        FullName = user.FullName,
                        MobileNo = userViewModel.MobileNo,
                        Balance = user.Balance,
                    };
                    return Result<BalanceViewModel>.Success("Success",balance);
                }
                return Result<BalanceViewModel>.ValidationError("User Not Found");

            }
            return Result<BalanceViewModel>.ValidationError("Please Fill Information First");
        }
        public string TransferValidation(TransferRequestModel transferRequestModel)
        {
            if (transferRequestModel.FromUser is null)
                return "From User Cannot be Empty";

            else if (transferRequestModel.FromMobileNo is null)
                return "From Mobile No Cannot be Empty";

            else if (transferRequestModel.ToUser is null)
                return "To User Cannot be Empty";

            else if (transferRequestModel.ToMobileNo is null)
                return "To Mobile Cannot be Empty";

            else if (transferRequestModel.FromUser.Equals(transferRequestModel.ToUser))
                return "From User and To User Cannot Same";

            else if (transferRequestModel.FromMobileNo.Equals(transferRequestModel.ToMobileNo))
                return "From Mobile No and To Mobile No Cannot Same";

            else if (transferRequestModel.TransferBalance <= 0)
                return "Transfer Balance Cannot Empty";

            else if (transferRequestModel.Pin is null)
                return "Pin Cannot Empty";

            else
                return "Success";
        }
        public Result<PinChangeRequestModel> PinChange(PinChangeRequestModel pinChangeRequestModel)
        {

            if (pinChangeRequestModel.FullName != null & pinChangeRequestModel.MobileNo != null && pinChangeRequestModel.OldPin != null && pinChangeRequestModel.NewPin != null)
            {
                if (pinChangeRequestModel.OldPin == pinChangeRequestModel.NewPin)
                    return Result<PinChangeRequestModel>.ValidationError("Cannot be the same old pin and new pin");

                var user = _db.TblUsers.FirstOrDefault(x => x.FullName == pinChangeRequestModel.FullName && x.MobileNo == pinChangeRequestModel.MobileNo && x.Pin == pinChangeRequestModel.OldPin);
                if (user != null)
                {
                    user.Pin = pinChangeRequestModel.NewPin;
                    _db.TblUsers.Update(user);
                    var result = _db.SaveChanges();
                    if (result > 0)
                        return Result<PinChangeRequestModel>.Success("Successfully changed",pinChangeRequestModel);
                    else
                        return Result<PinChangeRequestModel>.Error("Error While Saving");
                }
                return Result<PinChangeRequestModel>.ValidationError("Uer Not Found");
            }
            else
            {
                return Result<PinChangeRequestModel>.ValidationError("Please Fill Information First");
            }
        }
        public Result<PhoneNoChangeRequestModel> PhoneNumberChange(PhoneNoChangeRequestModel phoneNoChange)
        {

            if (phoneNoChange.FullName != null & phoneNoChange.OldPhoneNo != null && phoneNoChange.Pin != null && phoneNoChange.NewPhoneNo != null)
            {
                if (phoneNoChange.OldPhoneNo == phoneNoChange.NewPhoneNo)
                    return Result<PhoneNoChangeRequestModel>.ValidationError("Old Phone No and New Phone No are the Same");

                var user = _db.TblUsers.FirstOrDefault(x => x.FullName == phoneNoChange.FullName && x.MobileNo == phoneNoChange.OldPhoneNo && x.Pin == phoneNoChange.Pin);
                if (user != null)
                {
                    user.MobileNo = phoneNoChange.NewPhoneNo;
                    _db.TblUsers.Update(user);
                    var result = _db.SaveChanges();
                    if (result > 0)
                        return Result<PhoneNoChangeRequestModel>.Success("Success",phoneNoChange);
                    else
                        return Result<PhoneNoChangeRequestModel>.Error("User Not Found");
                }
                return Result<PhoneNoChangeRequestModel>.Error("User Not Found");
            }
            else
            {
                return Result<PhoneNoChangeRequestModel>.ValidationError("Please Full Fill Information First");
            }
        }
    }
}
