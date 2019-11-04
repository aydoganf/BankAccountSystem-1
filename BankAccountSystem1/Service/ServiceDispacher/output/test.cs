namespace AydoganFBank.Service.Dispatcher.Data
{
	using System;
	using AydoganFBank.AccountManagement.Api;

	public class AccountInfo
	{
		public Int32 Id { get; set; }
		public String AccountNumber { get; set; }
		public AccountTypeInfo AccountType { get; set; }
		public Decimal Balance { get; set; }
		public AccountOwner AccountOwner { get; set; }
	}


	public class AccountOwner
	{
		public AccountOwnerType OwnerType { get; set; }
		public Int32 OwnerId { get; set; }
		public String DisplayName { get; set; }
	}


	public class AccountTypeInfo
	{
		public Int32 Id { get; set; }
		public String TypeName { get; set; }
		public String TypeKey { get; set; }
		public String AssetsUnit { get; set; }
	}


	public class ApplicationInfo
	{
		public Int32 Id { get; set; }
		public String Name { get; set; }
		public String Domain { get; set; }
		public Guid Guid { get; set; }
	}


	public class CompanyInfo
	{
		public Int32 Id { get; set; }
		public String CompanyName { get; set; }
		public PersonInfo ResponsablePerson { get; set; }
		public String Address { get; set; }
		public String PhoneNumber { get; set; }
		public String TaxNumber { get; set; }
	}


	public class CreditCardInfo
	{
		public Int32 Id { get; set; }
		public String CreditCardNumber { get; set; }
		public Decimal Limit { get; set; }
		public Int32 ExtreDay { get; set; }
		public Decimal Debt { get; set; }
		public Int32 Type { get; set; }
		public String ValidMonth { get; set; }
		public String ValidYear { get; set; }
		public String SecurityCode { get; set; }
		public Boolean IsInternetUsageOpen { get; set; }
		public CreditCardOwner CreditCardOwner { get; set; }
		public String CreditCardMaskedNumber { get; set; }
		public Decimal UsableLimit { get; set; }
		public DateTime UntilValidDate { get; set; }
	}


	public class CreditCardOwner
	{
		public Int32 OwnerId { get; set; }
		public CreditCardOwnerType CreditCardOwnerType { get; set; }
		public String AssetsUnit { get; set; }
		public String DisplayText { get; set; }
	}


	public class PersonInfo
	{
		public Int32 Id { get; set; }
		public String FirstName { get; set; }
		public String LastName { get; set; }
		public String EmailAddress { get; set; }
		public String IdentityNumber { get; set; }
		public String FullName { get; set; }
	}


	public class TokenInfo
	{
		public Int32 Id { get; set; }
		public String Token { get; set; }
		public DateTime ValidUntil { get; set; }
		public Int32 ApplicationId { get; set; }
		public Boolean IsValid { get; set; }
		public PersonInfo PersonInfo { get; set; }
	}


	public class TransactionDetailInfo
	{
		public Int32 Id { get; set; }
		public String Description { get; set; }
		public TransactionInfo TransactionInfo { get; set; }
		public TransactionDirection TransactionDirection { get; set; }
	}


	public class TransactionDetailOwner
	{
		public Int32 OwnerId { get; set; }
		public TransactionDetailOwnerType OwnerType { get; set; }
	}


	public class TransactionHolder
	{
		public TransactionInfo TransactionInfo { get; set; }
		public DateTime CreateDate { get; set; }
	}


	public class TransactionInfo
	{
		public TransactionOwner TransactionOwner { get; set; }
		public TransactionOwner FromTransactionOwner { get; set; }
		public TransactionOwner ToTransactionOwner { get; set; }
		public Decimal Amount { get; set; }
		public DateTime TransactionDate { get; set; }
		public TransactionTypeInfo TransactionType { get; set; }
		public TransactionStatusInfo TransactionStatus { get; set; }
	}


	public class TransactionOwner
	{
		public Int32 OwnerId { get; set; }
		public TransactionOwnerType OwnerType { get; set; }
		public String TransactionDetailDisplayName { get; set; }
		public String AssetsUnit { get; set; }
	}


	public class TransactionOwnerWithDetails
	{
	}


	public class TransactionStatusInfo
	{
		public String StatusName { get; set; }
		public Int32 StatusId { get; set; }
		public String StatusKey { get; set; }
	}


	public class TransactionTypeInfo
	{
		public Int32 TypeId { get; set; }
		public String TypeKey { get; set; }
		public String TypeName { get; set; }
	}


}

namespace AydoganFBank.Service.Dispatcher.Data.Builder
{
	using System;
	using System.Collections.Generic;
	using AydoganFBank.Service.Dispatcher.Data;
	using AydoganFBank.AccountManagement.Api;

	public class ServiceDataBuilder
	{
		public AccountInfo AccountInfoBuilder(IAccountInfo iaccountinfo)
		{
			if (iaccountinfo == null) return null;
			return new AccountInfo()
			{
				Id = iaccountinfo.Id,
				AccountNumber = iaccountinfo.AccountNumber,
				AccountType = AccountTypeInfoBuilder(iaccountinfo.AccountType),
				Balance = iaccountinfo.Balance,
				AccountOwner = AccountOwnerBuilder(iaccountinfo.AccountOwner),
			};
		}

		public List<AccountInfo> AccountInfoListBuilder(List<IAccountInfo> iaccountinfos)
		{
			List<AccountInfo> ret = new List<AccountInfo>();
			foreach (var item in iaccountinfos)
				ret.Add(AccountInfoBuilder(item));
			return ret;
		}

		public AccountOwner AccountOwnerBuilder(IAccountOwner iaccountowner)
		{
			if (iaccountowner == null) return null;
			return new AccountOwner()
			{
				OwnerType = iaccountowner.OwnerType,
				OwnerId = iaccountowner.OwnerId,
				DisplayName = iaccountowner.DisplayName,
			};
		}

		public List<AccountOwner> AccountOwnerListBuilder(List<IAccountOwner> iaccountowners)
		{
			List<AccountOwner> ret = new List<AccountOwner>();
			foreach (var item in iaccountowners)
				ret.Add(AccountOwnerBuilder(item));
			return ret;
		}

		public AccountTypeInfo AccountTypeInfoBuilder(IAccountTypeInfo iaccounttypeinfo)
		{
			if (iaccounttypeinfo == null) return null;
			return new AccountTypeInfo()
			{
				Id = iaccounttypeinfo.Id,
				TypeName = iaccounttypeinfo.TypeName,
				TypeKey = iaccounttypeinfo.TypeKey,
				AssetsUnit = iaccounttypeinfo.AssetsUnit,
			};
		}

		public List<AccountTypeInfo> AccountTypeInfoListBuilder(List<IAccountTypeInfo> iaccounttypeinfos)
		{
			List<AccountTypeInfo> ret = new List<AccountTypeInfo>();
			foreach (var item in iaccounttypeinfos)
				ret.Add(AccountTypeInfoBuilder(item));
			return ret;
		}

		public ApplicationInfo ApplicationInfoBuilder(IApplicationInfo iapplicationinfo)
		{
			if (iapplicationinfo == null) return null;
			return new ApplicationInfo()
			{
				Id = iapplicationinfo.Id,
				Name = iapplicationinfo.Name,
				Domain = iapplicationinfo.Domain,
				Guid = iapplicationinfo.Guid,
			};
		}

		public List<ApplicationInfo> ApplicationInfoListBuilder(List<IApplicationInfo> iapplicationinfos)
		{
			List<ApplicationInfo> ret = new List<ApplicationInfo>();
			foreach (var item in iapplicationinfos)
				ret.Add(ApplicationInfoBuilder(item));
			return ret;
		}

		public CompanyInfo CompanyInfoBuilder(ICompanyInfo icompanyinfo)
		{
			if (icompanyinfo == null) return null;
			return new CompanyInfo()
			{
				Id = icompanyinfo.Id,
				CompanyName = icompanyinfo.CompanyName,
				ResponsablePerson = PersonInfoBuilder(icompanyinfo.ResponsablePerson),
				Address = icompanyinfo.Address,
				PhoneNumber = icompanyinfo.PhoneNumber,
				TaxNumber = icompanyinfo.TaxNumber,
			};
		}

		public List<CompanyInfo> CompanyInfoListBuilder(List<ICompanyInfo> icompanyinfos)
		{
			List<CompanyInfo> ret = new List<CompanyInfo>();
			foreach (var item in icompanyinfos)
				ret.Add(CompanyInfoBuilder(item));
			return ret;
		}

		public CreditCardInfo CreditCardInfoBuilder(ICreditCardInfo icreditcardinfo)
		{
			if (icreditcardinfo == null) return null;
			return new CreditCardInfo()
			{
				Id = icreditcardinfo.Id,
				CreditCardNumber = icreditcardinfo.CreditCardNumber,
				Limit = icreditcardinfo.Limit,
				ExtreDay = icreditcardinfo.ExtreDay,
				Debt = icreditcardinfo.Debt,
				Type = icreditcardinfo.Type,
				ValidMonth = icreditcardinfo.ValidMonth,
				ValidYear = icreditcardinfo.ValidYear,
				SecurityCode = icreditcardinfo.SecurityCode,
				IsInternetUsageOpen = icreditcardinfo.IsInternetUsageOpen,
				CreditCardOwner = CreditCardOwnerBuilder(icreditcardinfo.CreditCardOwner),
				CreditCardMaskedNumber = icreditcardinfo.CreditCardMaskedNumber,
				UsableLimit = icreditcardinfo.UsableLimit,
				UntilValidDate = icreditcardinfo.UntilValidDate,
			};
		}

		public List<CreditCardInfo> CreditCardInfoListBuilder(List<ICreditCardInfo> icreditcardinfos)
		{
			List<CreditCardInfo> ret = new List<CreditCardInfo>();
			foreach (var item in icreditcardinfos)
				ret.Add(CreditCardInfoBuilder(item));
			return ret;
		}

		public CreditCardOwner CreditCardOwnerBuilder(ICreditCardOwner icreditcardowner)
		{
			if (icreditcardowner == null) return null;
			return new CreditCardOwner()
			{
				OwnerId = icreditcardowner.OwnerId,
				CreditCardOwnerType = icreditcardowner.CreditCardOwnerType,
				AssetsUnit = icreditcardowner.AssetsUnit,
				DisplayText = icreditcardowner.DisplayText,
			};
		}

		public List<CreditCardOwner> CreditCardOwnerListBuilder(List<ICreditCardOwner> icreditcardowners)
		{
			List<CreditCardOwner> ret = new List<CreditCardOwner>();
			foreach (var item in icreditcardowners)
				ret.Add(CreditCardOwnerBuilder(item));
			return ret;
		}

		public PersonInfo PersonInfoBuilder(IPersonInfo ipersoninfo)
		{
			if (ipersoninfo == null) return null;
			return new PersonInfo()
			{
				Id = ipersoninfo.Id,
				FirstName = ipersoninfo.FirstName,
				LastName = ipersoninfo.LastName,
				EmailAddress = ipersoninfo.EmailAddress,
				IdentityNumber = ipersoninfo.IdentityNumber,
				FullName = ipersoninfo.FullName,
			};
		}

		public List<PersonInfo> PersonInfoListBuilder(List<IPersonInfo> ipersoninfos)
		{
			List<PersonInfo> ret = new List<PersonInfo>();
			foreach (var item in ipersoninfos)
				ret.Add(PersonInfoBuilder(item));
			return ret;
		}

		public TokenInfo TokenInfoBuilder(ITokenInfo itokeninfo)
		{
			if (itokeninfo == null) return null;
			return new TokenInfo()
			{
				Id = itokeninfo.Id,
				Token = itokeninfo.Token,
				ValidUntil = itokeninfo.ValidUntil,
				ApplicationId = itokeninfo.ApplicationId,
				IsValid = itokeninfo.IsValid,
				PersonInfo = PersonInfoBuilder(itokeninfo.PersonInfo),
			};
		}

		public List<TokenInfo> TokenInfoListBuilder(List<ITokenInfo> itokeninfos)
		{
			List<TokenInfo> ret = new List<TokenInfo>();
			foreach (var item in itokeninfos)
				ret.Add(TokenInfoBuilder(item));
			return ret;
		}

		public TransactionDetailInfo TransactionDetailInfoBuilder(ITransactionDetailInfo itransactiondetailinfo)
		{
			if (itransactiondetailinfo == null) return null;
			return new TransactionDetailInfo()
			{
				Id = itransactiondetailinfo.Id,
				Description = itransactiondetailinfo.Description,
				TransactionInfo = TransactionInfoBuilder(itransactiondetailinfo.TransactionInfo),
				TransactionDirection = itransactiondetailinfo.TransactionDirection,
			};
		}

		public List<TransactionDetailInfo> TransactionDetailInfoListBuilder(List<ITransactionDetailInfo> itransactiondetailinfos)
		{
			List<TransactionDetailInfo> ret = new List<TransactionDetailInfo>();
			foreach (var item in itransactiondetailinfos)
				ret.Add(TransactionDetailInfoBuilder(item));
			return ret;
		}

		public TransactionDetailOwner TransactionDetailOwnerBuilder(ITransactionDetailOwner itransactiondetailowner)
		{
			if (itransactiondetailowner == null) return null;
			return new TransactionDetailOwner()
			{
				OwnerId = itransactiondetailowner.OwnerId,
				OwnerType = itransactiondetailowner.OwnerType,
			};
		}

		public List<TransactionDetailOwner> TransactionDetailOwnerListBuilder(List<ITransactionDetailOwner> itransactiondetailowners)
		{
			List<TransactionDetailOwner> ret = new List<TransactionDetailOwner>();
			foreach (var item in itransactiondetailowners)
				ret.Add(TransactionDetailOwnerBuilder(item));
			return ret;
		}

		public TransactionHolder TransactionHolderBuilder(ITransactionHolder itransactionholder)
		{
			if (itransactionholder == null) return null;
			return new TransactionHolder()
			{
				TransactionInfo = TransactionInfoBuilder(itransactionholder.TransactionInfo),
				CreateDate = itransactionholder.CreateDate,
			};
		}

		public List<TransactionHolder> TransactionHolderListBuilder(List<ITransactionHolder> itransactionholders)
		{
			List<TransactionHolder> ret = new List<TransactionHolder>();
			foreach (var item in itransactionholders)
				ret.Add(TransactionHolderBuilder(item));
			return ret;
		}

		public TransactionInfo TransactionInfoBuilder(ITransactionInfo itransactioninfo)
		{
			if (itransactioninfo == null) return null;
			return new TransactionInfo()
			{
				TransactionOwner = TransactionOwnerBuilder(itransactioninfo.TransactionOwner),
				FromTransactionOwner = TransactionOwnerBuilder(itransactioninfo.FromTransactionOwner),
				ToTransactionOwner = TransactionOwnerBuilder(itransactioninfo.ToTransactionOwner),
				Amount = itransactioninfo.Amount,
				TransactionDate = itransactioninfo.TransactionDate,
				TransactionType = TransactionTypeInfoBuilder(itransactioninfo.TransactionType),
				TransactionStatus = TransactionStatusInfoBuilder(itransactioninfo.TransactionStatus),
			};
		}

		public List<TransactionInfo> TransactionInfoListBuilder(List<ITransactionInfo> itransactioninfos)
		{
			List<TransactionInfo> ret = new List<TransactionInfo>();
			foreach (var item in itransactioninfos)
				ret.Add(TransactionInfoBuilder(item));
			return ret;
		}

		public TransactionOwner TransactionOwnerBuilder(ITransactionOwner itransactionowner)
		{
			if (itransactionowner == null) return null;
			return new TransactionOwner()
			{
				OwnerId = itransactionowner.OwnerId,
				OwnerType = itransactionowner.OwnerType,
				TransactionDetailDisplayName = itransactionowner.TransactionDetailDisplayName,
				AssetsUnit = itransactionowner.AssetsUnit,
			};
		}

		public List<TransactionOwner> TransactionOwnerListBuilder(List<ITransactionOwner> itransactionowners)
		{
			List<TransactionOwner> ret = new List<TransactionOwner>();
			foreach (var item in itransactionowners)
				ret.Add(TransactionOwnerBuilder(item));
			return ret;
		}

		public TransactionOwnerWithDetails TransactionOwnerWithDetailsBuilder(ITransactionOwnerWithDetails itransactionownerwithdetails)
		{
			if (itransactionownerwithdetails == null) return null;
			return new TransactionOwnerWithDetails()
			{
			};
		}

		public List<TransactionOwnerWithDetails> TransactionOwnerWithDetailsListBuilder(List<ITransactionOwnerWithDetails> itransactionownerwithdetailss)
		{
			List<TransactionOwnerWithDetails> ret = new List<TransactionOwnerWithDetails>();
			foreach (var item in itransactionownerwithdetailss)
				ret.Add(TransactionOwnerWithDetailsBuilder(item));
			return ret;
		}

		public TransactionStatusInfo TransactionStatusInfoBuilder(ITransactionStatusInfo itransactionstatusinfo)
		{
			if (itransactionstatusinfo == null) return null;
			return new TransactionStatusInfo()
			{
				StatusName = itransactionstatusinfo.StatusName,
				StatusId = itransactionstatusinfo.StatusId,
				StatusKey = itransactionstatusinfo.StatusKey,
			};
		}

		public List<TransactionStatusInfo> TransactionStatusInfoListBuilder(List<ITransactionStatusInfo> itransactionstatusinfos)
		{
			List<TransactionStatusInfo> ret = new List<TransactionStatusInfo>();
			foreach (var item in itransactionstatusinfos)
				ret.Add(TransactionStatusInfoBuilder(item));
			return ret;
		}

		public TransactionTypeInfo TransactionTypeInfoBuilder(ITransactionTypeInfo itransactiontypeinfo)
		{
			if (itransactiontypeinfo == null) return null;
			return new TransactionTypeInfo()
			{
				TypeId = itransactiontypeinfo.TypeId,
				TypeKey = itransactiontypeinfo.TypeKey,
				TypeName = itransactiontypeinfo.TypeName,
			};
		}

		public List<TransactionTypeInfo> TransactionTypeInfoListBuilder(List<ITransactionTypeInfo> itransactiontypeinfos)
		{
			List<TransactionTypeInfo> ret = new List<TransactionTypeInfo>();
			foreach (var item in itransactiontypeinfos)
				ret.Add(TransactionTypeInfoBuilder(item));
			return ret;
		}

	}
}

namespace AydoganFBank.Service.Dispatcher.Api
{
	using System;
	using AydoganFBank.Service.Dispatcher.Data;
	using System.Collections.Generic;
	using AydoganFBank.AccountManagement.Api;

	public interface IAccountManagerService
	{
		AccountInfo CreatePersonAccount(String accountTypeKey, Int32 personId);

		AccountInfo CreateCompanyAccount(String accountTypeKey, Int32 companyId);

		AccountInfo GetAccountInfo(Int32 accountId);

		AccountInfo GetAccountInfoByAccountNumber(String accountNumber);

		AccountInfo WithdrawMoneyFromOwn(Int32 accountId, Decimal amount);

		AccountInfo DepositToOwnAccount(Int32 accountId, Decimal amount);

		List<AccountInfo> GetAccountsByPerson(Int32 personId);

		Object TransferAssets(Int32 fromAccountId, Int32 toAccountId, Decimal amount, TransactionTypeEnum transactionType);

		AccountTypeInfo GetAccountTypeInfo(Int32 accountTypeId);

		AccountTypeInfo GetAccountTypeByKey(String key);

		Object DeleteAccount(Int32 accountId);

	}

	public interface ICompanyManagerService
	{
		CompanyInfo CreateCompany(String companyName, Int32 responsablePersonId, String address, String phoneNumber, String taxNumber);

		CompanyInfo GetCompanyInfo(Int32 companyId);

		CompanyInfo GetCompanyByResponsableId(Int32 responsablePersonId);

		CompanyInfo GetCompanyByResponsableIdentityNumber(String responsableIdentityNumber);

		CompanyInfo ChangeCompanyAddress(Int32 companyId, String address);

		CompanyInfo ChangeCompanyPhoneNumber(Int32 companyId, String phoneNumber);

		CompanyInfo GetCompanyByTaxNumber(String taxNumber);

		List<AccountInfo> GetCompanyAccounts(Int32 companyId);

	}

	public interface ICreditCardManagerService
	{
		CreditCardInfo DoCreditCardPayment(Int32 creditCardId, Decimal amount, Int32 instalmentCount, Int32 toAccountId);

		CreditCardInfo CreateAccountCreditCard(Decimal limit, Int32 extreDay, Int32 type, String validMonth, String validYear, String securityCode, Boolean isInternetUsageOpen, Int32 accountId);

		CreditCardInfo DoCreditCardPayment(String creditCardNumber, String validMonth, String validYear, String securityCode, Decimal amount, Int32 instalmentCount, Int32 toAccountId);

		List<CreditCardInfo> GetCreditCardListByPerson(Int32 personId);

		CreditCardInfo GetCreditCardByAccount(String accountNumber);

	}

	public interface IPersonManagerService
	{
		PersonInfo CreatePerson(String firstName, String lastName, String emailAddress, String identityNumber);

		PersonInfo ChangePersonLastName(Int32 personId, String lastName);

		PersonInfo ChangePersonEmailAddress(Int32 personId, String emailAddress);

		PersonInfo GetPersonInfo(Int32 personId);

		PersonInfo GetPersonByIdentityNumber(String identityNumber);

		List<PersonInfo> GetAllPersonList();

	}

	public interface ISecurityManagerService
	{
		TokenInfo GetTokenInfo(Int32 tokenId);

		TokenInfo GetTokenByValue(String value);

		TokenInfo GetTokenByValueAndApplication(String value, Int32 applicationId);

		TokenInfo CreateToken(Int32 personId, Int32 applicationId);

		TokenInfo ValidateToken(String tokenValue, Int32 applicationId);

		TokenInfo LoginByEmail(String email, String password, Int32 applicationId);

		TokenInfo Login(String identity, String passwordSalt, Int32 applicationId);

		ApplicationInfo GetApplicationInfo(Int32 applicationId);

		ApplicationInfo GetApplicationByToken(String token);

		ApplicationInfo CreateApplication(String name, String domain);

	}

	public interface ITransactionManagerService
	{
		List<TransactionInfo> GetCreditCardLastDateRangeTransactionInfoList(Int32 creditCardId, DateTime startDate, DateTime endDate);

		List<TransactionInfo> GetAccountTransactionDateRangeTransactionInfoList(Int32 accountId, DateTime startDate, DateTime endDate);

		List<TransactionDetailInfo> GetCreditCardLastDateRangeTransactionDetailInfoList(Int32 creditCardId, DateTime startDate, DateTime endDate);

		List<TransactionDetailInfo> GetAccountLastDateRangeTransactionDetailInfoList(Int32 accountId, DateTime startDate, DateTime endDate);

		List<TransactionDetailInfo> GetCreditCardDateRangeTransactionDetailInfoList(Int32 creditCardId, DateTime startDate, DateTime endDate);

		List<TransactionDetailInfo> GetAccountDateRangeTransactionDetailInfoList(Int32 accountId, DateTime startDate, DateTime endDate);

		List<TransactionInfo> GetAccountLastIncomingDateRangeAccountTransactionInfoList(Int32 accountId, DateTime startDate, DateTime endDate);

		List<TransactionInfo> GetAccountLastOutgoingDateRangeAccountTransactionInfoList(Int32 accountId, DateTime startDate, DateTime endDate);

		List<TransactionDetailInfo> GetAccountLastDateRangeAndIncomingTransactionDetailInfoList(Int32 accountId, DateTime startDate, DateTime endDate);

		List<TransactionDetailInfo> GetAccountLastDateRangeAndOutgoingTransactionDetailInfoList(Int32 accountId, DateTime startDate, DateTime endDate);

	}

}

namespace AydoganFBank.Service.Dispatcher.Services
{
	using System;
	using System.Collections.Generic;
	using AydoganFBank.Service.Dispatcher.Data;
	using AydoganFBank.Service.Dispatcher.Data.Builder;
	using AydoganFBank.Service.Dispatcher.Api;
	using AydoganFBank.AccountManagement.Api;
	using AydoganFBank.AccountManagement.Service;

	public class AccountManagerService : IAccountManagerService
	{
		private readonly IAccountManager accountManager;
		private readonly ServiceDataBuilder serviceDataBuilder;

		public AccountManagerService(IAccountManager accountManager, ServiceDataBuilder serviceDataBuilder)
		{
			this.accountManager = accountManager;
			this.serviceDataBuilder = serviceDataBuilder;
		}

		public AccountInfo CreatePersonAccount(String accountTypeKey, Int32 personId)
		{
			var result = accountManager.CreatePersonAccount(accountTypeKey, personId);
			return serviceDataBuilder.AccountInfoBuilder(result);
		}

		public AccountInfo CreateCompanyAccount(String accountTypeKey, Int32 companyId)
		{
			var result = accountManager.CreateCompanyAccount(accountTypeKey, companyId);
			return serviceDataBuilder.AccountInfoBuilder(result);
		}

		public AccountInfo GetAccountInfo(Int32 accountId)
		{
			var result = accountManager.GetAccountInfo(accountId);
			return serviceDataBuilder.AccountInfoBuilder(result);
		}

		public AccountInfo GetAccountInfoByAccountNumber(String accountNumber)
		{
			var result = accountManager.GetAccountInfoByAccountNumber(accountNumber);
			return serviceDataBuilder.AccountInfoBuilder(result);
		}

		public AccountInfo WithdrawMoneyFromOwn(Int32 accountId, Decimal amount)
		{
			var result = accountManager.WithdrawMoneyFromOwn(accountId, amount);
			return serviceDataBuilder.AccountInfoBuilder(result);
		}

		public AccountInfo DepositToOwnAccount(Int32 accountId, Decimal amount)
		{
			var result = accountManager.DepositToOwnAccount(accountId, amount);
			return serviceDataBuilder.AccountInfoBuilder(result);
		}

		public List<AccountInfo> GetAccountsByPerson(Int32 personId)
		{
			var result = accountManager.GetAccountsByPerson(personId);
			return serviceDataBuilder.AccountInfoListBuilder(result);
		}

		public Object TransferAssets(Int32 fromAccountId, Int32 toAccountId, Decimal amount, TransactionTypeEnum transactionType)
		{
			var result = accountManager.TransferAssets(fromAccountId, toAccountId, amount, transactionType);
			return result;
		}

		public AccountTypeInfo GetAccountTypeInfo(Int32 accountTypeId)
		{
			var result = accountManager.GetAccountTypeInfo(accountTypeId);
			return serviceDataBuilder.AccountTypeInfoBuilder(result);
		}

		public AccountTypeInfo GetAccountTypeByKey(String key)
		{
			var result = accountManager.GetAccountTypeByKey(key);
			return serviceDataBuilder.AccountTypeInfoBuilder(result);
		}

		public Object DeleteAccount(Int32 accountId)
		{
			var result = accountManager.DeleteAccount(accountId);
			return result;
		}

	}
	public class CompanyManagerService : ICompanyManagerService
	{
		private readonly ICompanyManager companyManager;
		private readonly ServiceDataBuilder serviceDataBuilder;

		public CompanyManagerService(ICompanyManager companyManager, ServiceDataBuilder serviceDataBuilder)
		{
			this.companyManager = companyManager;
			this.serviceDataBuilder = serviceDataBuilder;
		}

		public CompanyInfo CreateCompany(String companyName, Int32 responsablePersonId, String address, String phoneNumber, String taxNumber)
		{
			var result = companyManager.CreateCompany(companyName, responsablePersonId, address, phoneNumber, taxNumber);
			return serviceDataBuilder.CompanyInfoBuilder(result);
		}

		public CompanyInfo GetCompanyInfo(Int32 companyId)
		{
			var result = companyManager.GetCompanyInfo(companyId);
			return serviceDataBuilder.CompanyInfoBuilder(result);
		}

		public CompanyInfo GetCompanyByResponsableId(Int32 responsablePersonId)
		{
			var result = companyManager.GetCompanyByResponsableId(responsablePersonId);
			return serviceDataBuilder.CompanyInfoBuilder(result);
		}

		public CompanyInfo GetCompanyByResponsableIdentityNumber(String responsableIdentityNumber)
		{
			var result = companyManager.GetCompanyByResponsableIdentityNumber(responsableIdentityNumber);
			return serviceDataBuilder.CompanyInfoBuilder(result);
		}

		public CompanyInfo ChangeCompanyAddress(Int32 companyId, String address)
		{
			var result = companyManager.ChangeCompanyAddress(companyId, address);
			return serviceDataBuilder.CompanyInfoBuilder(result);
		}

		public CompanyInfo ChangeCompanyPhoneNumber(Int32 companyId, String phoneNumber)
		{
			var result = companyManager.ChangeCompanyPhoneNumber(companyId, phoneNumber);
			return serviceDataBuilder.CompanyInfoBuilder(result);
		}

		public CompanyInfo GetCompanyByTaxNumber(String taxNumber)
		{
			var result = companyManager.GetCompanyByTaxNumber(taxNumber);
			return serviceDataBuilder.CompanyInfoBuilder(result);
		}

		public List<AccountInfo> GetCompanyAccounts(Int32 companyId)
		{
			var result = companyManager.GetCompanyAccounts(companyId);
			return serviceDataBuilder.AccountInfoListBuilder(result);
		}

	}
	public class CreditCardManagerService : ICreditCardManagerService
	{
		private readonly ICreditCardManager creditCardManager;
		private readonly ServiceDataBuilder serviceDataBuilder;

		public CreditCardManagerService(ICreditCardManager creditCardManager, ServiceDataBuilder serviceDataBuilder)
		{
			this.creditCardManager = creditCardManager;
			this.serviceDataBuilder = serviceDataBuilder;
		}

		public CreditCardInfo DoCreditCardPayment(Int32 creditCardId, Decimal amount, Int32 instalmentCount, Int32 toAccountId)
		{
			var result = creditCardManager.DoCreditCardPayment(creditCardId, amount, instalmentCount, toAccountId);
			return serviceDataBuilder.CreditCardInfoBuilder(result);
		}

		public CreditCardInfo CreateAccountCreditCard(Decimal limit, Int32 extreDay, Int32 type, String validMonth, String validYear, String securityCode, Boolean isInternetUsageOpen, Int32 accountId)
		{
			var result = creditCardManager.CreateAccountCreditCard(limit, extreDay, type, validMonth, validYear, securityCode, isInternetUsageOpen, accountId);
			return serviceDataBuilder.CreditCardInfoBuilder(result);
		}

		public CreditCardInfo DoCreditCardPayment(String creditCardNumber, String validMonth, String validYear, String securityCode, Decimal amount, Int32 instalmentCount, Int32 toAccountId)
		{
			var result = creditCardManager.DoCreditCardPayment(creditCardNumber, validMonth, validYear, securityCode, amount, instalmentCount, toAccountId);
			return serviceDataBuilder.CreditCardInfoBuilder(result);
		}

		public List<CreditCardInfo> GetCreditCardListByPerson(Int32 personId)
		{
			var result = creditCardManager.GetCreditCardListByPerson(personId);
			return serviceDataBuilder.CreditCardInfoListBuilder(result);
		}

		public CreditCardInfo GetCreditCardByAccount(String accountNumber)
		{
			var result = creditCardManager.GetCreditCardByAccount(accountNumber);
			return serviceDataBuilder.CreditCardInfoBuilder(result);
		}

	}
	public class PersonManagerService : IPersonManagerService
	{
		private readonly IPersonManager personManager;
		private readonly ServiceDataBuilder serviceDataBuilder;

		public PersonManagerService(IPersonManager personManager, ServiceDataBuilder serviceDataBuilder)
		{
			this.personManager = personManager;
			this.serviceDataBuilder = serviceDataBuilder;
		}

		public PersonInfo CreatePerson(String firstName, String lastName, String emailAddress, String identityNumber)
		{
			var result = personManager.CreatePerson(firstName, lastName, emailAddress, identityNumber);
			return serviceDataBuilder.PersonInfoBuilder(result);
		}

		public PersonInfo ChangePersonLastName(Int32 personId, String lastName)
		{
			var result = personManager.ChangePersonLastName(personId, lastName);
			return serviceDataBuilder.PersonInfoBuilder(result);
		}

		public PersonInfo ChangePersonEmailAddress(Int32 personId, String emailAddress)
		{
			var result = personManager.ChangePersonEmailAddress(personId, emailAddress);
			return serviceDataBuilder.PersonInfoBuilder(result);
		}

		public PersonInfo GetPersonInfo(Int32 personId)
		{
			var result = personManager.GetPersonInfo(personId);
			return serviceDataBuilder.PersonInfoBuilder(result);
		}

		public PersonInfo GetPersonByIdentityNumber(String identityNumber)
		{
			var result = personManager.GetPersonByIdentityNumber(identityNumber);
			return serviceDataBuilder.PersonInfoBuilder(result);
		}

		public List<PersonInfo> GetAllPersonList()
		{
			var result = personManager.GetAllPersonList();
			return serviceDataBuilder.PersonInfoListBuilder(result);
		}

	}
	public class SecurityManagerService : ISecurityManagerService
	{
		private readonly ISecurityManager securityManager;
		private readonly ServiceDataBuilder serviceDataBuilder;

		public SecurityManagerService(ISecurityManager securityManager, ServiceDataBuilder serviceDataBuilder)
		{
			this.securityManager = securityManager;
			this.serviceDataBuilder = serviceDataBuilder;
		}

		public TokenInfo GetTokenInfo(Int32 tokenId)
		{
			var result = securityManager.GetTokenInfo(tokenId);
			return serviceDataBuilder.TokenInfoBuilder(result);
		}

		public TokenInfo GetTokenByValue(String value)
		{
			var result = securityManager.GetTokenByValue(value);
			return serviceDataBuilder.TokenInfoBuilder(result);
		}

		public TokenInfo GetTokenByValueAndApplication(String value, Int32 applicationId)
		{
			var result = securityManager.GetTokenByValueAndApplication(value, applicationId);
			return serviceDataBuilder.TokenInfoBuilder(result);
		}

		public TokenInfo CreateToken(Int32 personId, Int32 applicationId)
		{
			var result = securityManager.CreateToken(personId, applicationId);
			return serviceDataBuilder.TokenInfoBuilder(result);
		}

		public TokenInfo ValidateToken(String tokenValue, Int32 applicationId)
		{
			var result = securityManager.ValidateToken(tokenValue, applicationId);
			return serviceDataBuilder.TokenInfoBuilder(result);
		}

		public TokenInfo LoginByEmail(String email, String password, Int32 applicationId)
		{
			var result = securityManager.LoginByEmail(email, password, applicationId);
			return serviceDataBuilder.TokenInfoBuilder(result);
		}

		public TokenInfo Login(String identity, String passwordSalt, Int32 applicationId)
		{
			var result = securityManager.Login(identity, passwordSalt, applicationId);
			return serviceDataBuilder.TokenInfoBuilder(result);
		}

		public ApplicationInfo GetApplicationInfo(Int32 applicationId)
		{
			var result = securityManager.GetApplicationInfo(applicationId);
			return serviceDataBuilder.ApplicationInfoBuilder(result);
		}

		public ApplicationInfo GetApplicationByToken(String token)
		{
			var result = securityManager.GetApplicationByToken(token);
			return serviceDataBuilder.ApplicationInfoBuilder(result);
		}

		public ApplicationInfo CreateApplication(String name, String domain)
		{
			var result = securityManager.CreateApplication(name, domain);
			return serviceDataBuilder.ApplicationInfoBuilder(result);
		}

	}
	public class TransactionManagerService : ITransactionManagerService
	{
		private readonly ITransactionManager transactionManager;
		private readonly ServiceDataBuilder serviceDataBuilder;

		public TransactionManagerService(ITransactionManager transactionManager, ServiceDataBuilder serviceDataBuilder)
		{
			this.transactionManager = transactionManager;
			this.serviceDataBuilder = serviceDataBuilder;
		}

		public List<TransactionInfo> GetCreditCardLastDateRangeTransactionInfoList(Int32 creditCardId, DateTime startDate, DateTime endDate)
		{
			var result = transactionManager.GetCreditCardLastDateRangeTransactionInfoList(creditCardId, startDate, endDate);
			return serviceDataBuilder.TransactionInfoListBuilder(result);
		}

		public List<TransactionInfo> GetAccountTransactionDateRangeTransactionInfoList(Int32 accountId, DateTime startDate, DateTime endDate)
		{
			var result = transactionManager.GetAccountTransactionDateRangeTransactionInfoList(accountId, startDate, endDate);
			return serviceDataBuilder.TransactionInfoListBuilder(result);
		}

		public List<TransactionDetailInfo> GetCreditCardLastDateRangeTransactionDetailInfoList(Int32 creditCardId, DateTime startDate, DateTime endDate)
		{
			var result = transactionManager.GetCreditCardLastDateRangeTransactionDetailInfoList(creditCardId, startDate, endDate);
			return serviceDataBuilder.TransactionDetailInfoListBuilder(result);
		}

		public List<TransactionDetailInfo> GetAccountLastDateRangeTransactionDetailInfoList(Int32 accountId, DateTime startDate, DateTime endDate)
		{
			var result = transactionManager.GetAccountLastDateRangeTransactionDetailInfoList(accountId, startDate, endDate);
			return serviceDataBuilder.TransactionDetailInfoListBuilder(result);
		}

		public List<TransactionDetailInfo> GetCreditCardDateRangeTransactionDetailInfoList(Int32 creditCardId, DateTime startDate, DateTime endDate)
		{
			var result = transactionManager.GetCreditCardDateRangeTransactionDetailInfoList(creditCardId, startDate, endDate);
			return serviceDataBuilder.TransactionDetailInfoListBuilder(result);
		}

		public List<TransactionDetailInfo> GetAccountDateRangeTransactionDetailInfoList(Int32 accountId, DateTime startDate, DateTime endDate)
		{
			var result = transactionManager.GetAccountDateRangeTransactionDetailInfoList(accountId, startDate, endDate);
			return serviceDataBuilder.TransactionDetailInfoListBuilder(result);
		}

		public List<TransactionInfo> GetAccountLastIncomingDateRangeAccountTransactionInfoList(Int32 accountId, DateTime startDate, DateTime endDate)
		{
			var result = transactionManager.GetAccountLastIncomingDateRangeAccountTransactionInfoList(accountId, startDate, endDate);
			return serviceDataBuilder.TransactionInfoListBuilder(result);
		}

		public List<TransactionInfo> GetAccountLastOutgoingDateRangeAccountTransactionInfoList(Int32 accountId, DateTime startDate, DateTime endDate)
		{
			var result = transactionManager.GetAccountLastOutgoingDateRangeAccountTransactionInfoList(accountId, startDate, endDate);
			return serviceDataBuilder.TransactionInfoListBuilder(result);
		}

		public List<TransactionDetailInfo> GetAccountLastDateRangeAndIncomingTransactionDetailInfoList(Int32 accountId, DateTime startDate, DateTime endDate)
		{
			var result = transactionManager.GetAccountLastDateRangeAndIncomingTransactionDetailInfoList(accountId, startDate, endDate);
			return serviceDataBuilder.TransactionDetailInfoListBuilder(result);
		}

		public List<TransactionDetailInfo> GetAccountLastDateRangeAndOutgoingTransactionDetailInfoList(Int32 accountId, DateTime startDate, DateTime endDate)
		{
			var result = transactionManager.GetAccountLastDateRangeAndOutgoingTransactionDetailInfoList(accountId, startDate, endDate);
			return serviceDataBuilder.TransactionDetailInfoListBuilder(result);
		}

	}
}

namespace AydoganFBank.Service.Dispatcher.Context
{
	using AydoganFBank.Service.Dispatcher.Api;

	public interface IServiceContext
	{
		IAccountManagerService AccountManagerService { get; }
		ICompanyManagerService CompanyManagerService { get; }
		ICreditCardManagerService CreditCardManagerService { get; }
		IPersonManagerService PersonManagerService { get; }
		ISecurityManagerService SecurityManagerService { get; }
		ITransactionManagerService TransactionManagerService { get; }
	}

	public class ServiceContext : IServiceContext
	{
		public IAccountManagerService AccountManagerService { get; private set; }
		public ICompanyManagerService CompanyManagerService { get; private set; }
		public ICreditCardManagerService CreditCardManagerService { get; private set; }
		public IPersonManagerService PersonManagerService { get; private set; }
		public ISecurityManagerService SecurityManagerService { get; private set; }
		public ITransactionManagerService TransactionManagerService { get; private set; }

		public ServiceContext(IAccountManagerService AccountManagerService, ICompanyManagerService CompanyManagerService, ICreditCardManagerService CreditCardManagerService, IPersonManagerService PersonManagerService, ISecurityManagerService SecurityManagerService, ITransactionManagerService TransactionManagerService)
		{
			this.AccountManagerService = AccountManagerService;
			this.CompanyManagerService = CompanyManagerService;
			this.CreditCardManagerService = CreditCardManagerService;
			this.PersonManagerService = PersonManagerService;
			this.SecurityManagerService = SecurityManagerService;
			this.TransactionManagerService = TransactionManagerService;
		}

	}
}