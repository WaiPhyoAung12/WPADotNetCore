using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPADotNetCore.Databases.Models;
using WPADotNetCore.Databases.ViewModels;

namespace WPADotNetCore.Domain.Kpay
{
    public class KpayService
    {
        private readonly AppDbContext _db = new AppDbContext();

        public string Registration(UserViewModel user)
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
                return result > 0 ? "Registraion Success" : "Fail in Registration";
            }
            else
            {
                return "Please Complete Information";
            }
        }
        public string Login(LoginRequestModel loginRequestModel)
        {
            if (loginRequestModel.FullName != null && loginRequestModel.Pin != null)
            {
                var result = _db.TblUsers.FirstOrDefault(x => x.FullName == loginRequestModel.FullName && x.Pin == loginRequestModel.Pin);
                if (result is not null)
                    return "Login Success";
                else
                    return "Fail Login";
            }
            else
            {
                return "Please Fill Information";
            }
        }
        public string Deposit(DepositRequestModel depositRequestModel)
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
                        Description="Deposit"
                    };
                    _db.TblTransactions.Add(transaction);
                    var result = _db.SaveChanges();
                    if (result > 0)
                    {
                        return "Successfully Deposit";
                    }

                    else
                        return "Error in Deposit";
                }
                return "User Not Found";
            }
            else
            {
                return "Please Fill Information";
            }
        }
        public string WithDraw(WithDrawRequestModel withDrawRequestModel)
        {
            if (withDrawRequestModel.FullName != null && withDrawRequestModel.WithDrawAmount > 0)
            {
                var user = _db.TblUsers.FirstOrDefault(x => x.FullName == withDrawRequestModel.FullName && x.MobileNo == withDrawRequestModel.MobileNo && x.Pin == withDrawRequestModel.Pin);
                if (user != null)
                {
                    if (user.Balance - withDrawRequestModel.WithDrawAmount < 1000)
                        return "Insufficient Amount";
                    user.Balance -= withDrawRequestModel.WithDrawAmount;
                    _db.TblUsers.Update(user);
                    TblTransaction transaction = new TblTransaction()
                    {
                        UserId = user.Id,
                        Amount = withDrawRequestModel.WithDrawAmount,
                        Status = "Debit",
                        CreatedDate = DateTime.Now,
                        Description="WithDraw"
                    };
                    _db.TblTransactions.Add(transaction);
                    var result = _db.SaveChanges();
                    if (result > 0)
                    {
                        return "Successfully WithDraw";
                    }

                    else
                        return "Error in updating";
                }
                return "User Not Found";
            }
            else
            {
                return "Please Fill Information";
            }
        }
        public string Transfer(TransferRequestModel transferRequestModel)
        {
            var validationResult = TransferValidation(transferRequestModel);
            if (validationResult == "Success")
            {
                var fromTblUser = _db.TblUsers.FirstOrDefault(x => x.FullName == transferRequestModel.FromUser && x.Pin == transferRequestModel.Pin && x.MobileNo == transferRequestModel.FromMobileNo);
                if (fromTblUser == null)
                    return "From User Not Found Please Register First";
                var toTblUser = _db.TblUsers.FirstOrDefault(x => x.FullName == transferRequestModel.ToUser && x.MobileNo == transferRequestModel.ToMobileNo);
                if (toTblUser == null)
                    return "To User Not Found";

                if (fromTblUser.Balance - transferRequestModel.TransferBalance < 1000)
                {
                    return "Insufficient Balance";
                }
                fromTblUser.Balance -= transferRequestModel.TransferBalance;
                _db.TblUsers.Update(fromTblUser);

                TblTransaction transaction = new TblTransaction()
                {
                    UserId = fromTblUser.Id,
                    Amount = transferRequestModel.TransferBalance,
                    Status = "Debit",
                    CreatedDate = DateTime.Now,
                    Description="Transfer"
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
                    Description="Transfer"
                };
                _db.TblTransactions.Add(transaction2);
                var result = _db.SaveChanges();
                if (result > 0)
                {
                    return "Success";
                }
                return "Fail Transfer Process";
            }
            return validationResult;
        }
        public BalanceViewModel GetUserBalance(UserViewModel userViewModel)
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
                    return balance;
                }

            }
            return null;
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
        public string PinChange(PinChangeRequestModel pinChangeRequestModel)
        {

            if (pinChangeRequestModel.FullName != null & pinChangeRequestModel.MobileNo != null && pinChangeRequestModel.OldPin != null && pinChangeRequestModel.NewPin != null)
            {
                if (pinChangeRequestModel.OldPin == pinChangeRequestModel.NewPin)
                    return "Old Password and New Password are the Same";

                var user = _db.TblUsers.FirstOrDefault(x => x.FullName == pinChangeRequestModel.FullName && x.MobileNo == pinChangeRequestModel.MobileNo && x.Pin == pinChangeRequestModel.OldPin);
                if (user != null)
                {
                    user.Pin = pinChangeRequestModel.NewPin;
                    _db.TblUsers.Update(user);
                    var result = _db.SaveChanges();
                    if (result > 0)
                        return "Successfully Changed";
                    else
                        return "Fail in Changing Pin";
                }
                return "User Not Found";
            }
            else
            {
                return "Please Fill Information";
            }
        }
        public string PhoneNumberChange(PhoneNoChangeRequestModel phoneNoChange)
        {

            if (phoneNoChange.FullName != null & phoneNoChange.OldPhoneNo != null && phoneNoChange.Pin != null && phoneNoChange.NewPhoneNo != null)
            {
                if (phoneNoChange.OldPhoneNo == phoneNoChange.NewPhoneNo)
                    return "Old Phone No and New Phone No are the Same";

                var user = _db.TblUsers.FirstOrDefault(x => x.FullName == phoneNoChange.FullName && x.MobileNo == phoneNoChange.OldPhoneNo && x.Pin == phoneNoChange.Pin);
                if (user != null)
                {
                    user.MobileNo = phoneNoChange.NewPhoneNo;
                    _db.TblUsers.Update(user);
                    var result = _db.SaveChanges();
                    if (result > 0)
                        return "Successfully Changed";
                    else
                        return "Fail in Changing Phone No";
                }
                return "User Not Found";
            }
            else
            {
                return "Please Fill Information";
            }
        }
    }
}
