USE [AydoganFBank]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[OwnerId] [int] NOT NULL,
	[OwnerType] [int] NOT NULL,
	[AccountNumber] [nvarchar](50) NOT NULL,
	[AccountTypeId] [int] NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountTransaction]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountTransaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[TransactionTypeId] [int] NOT NULL,
	[TransactionStatusId] [int] NOT NULL,
	[OwnerId] [int] NOT NULL,
	[OwnerType] [int] NOT NULL,
	[FromOwnerType] [int] NULL,
	[FromOwnerId] [int] NULL,
	[ToOwnerType] [int] NULL,
	[ToOwnerId] [int] NULL,
 CONSTRAINT [PK_AccountTransaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountType]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountType](
	[AccountTypeId] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[TypeKey] [nvarchar](50) NOT NULL,
	[AssetsUnit] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_AccountType] PRIMARY KEY CLUSTERED 
(
	[AccountTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application](
	[ApplicationId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Domain] [nvarchar](50) NOT NULL,
	[Token] [nvarchar](250) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[TokenValidationMinute] [int] NULL,
	[TokenSlidingMinute] [int] NULL,
	[TokenSlidingCheckMinute] [int] NULL,
 CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](250) NOT NULL,
	[ResponsablePersonId] [int] NOT NULL,
	[Address] [nvarchar](250) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[TaxNumber] [nvarchar](50) NOT NULL,
	[AccountId] [int] NOT NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreditCard]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditCard](
	[CreditCardId] [int] IDENTITY(1,1) NOT NULL,
	[CreditCardNumber] [nvarchar](50) NOT NULL,
	[Limit] [decimal](18, 2) NOT NULL,
	[Debt] [decimal](18, 2) NOT NULL,
	[ExtreDay] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[ValidMonth] [nvarchar](50) NOT NULL,
	[ValidYear] [nvarchar](50) NOT NULL,
	[SecurityCode] [nvarchar](50) NULL,
	[IsInternetUsageOpen] [bit] NOT NULL,
	[OwnerType] [int] NOT NULL,
	[OwnerId] [int] NOT NULL,
 CONSTRAINT [PK_CreditCard] PRIMARY KEY CLUSTERED 
(
	[CreditCardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreditCardExtre]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditCardExtre](
	[CreditCardExtreId] [int] IDENTITY(1,1) NOT NULL,
	[CreditCardId] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[MonthName] [nvarchar](50) NOT NULL,
	[Year] [int] NOT NULL,
	[TotalPayment] [decimal](18, 2) NOT NULL,
	[MinPayment] [decimal](18, 2) NOT NULL,
	[IsDischarged] [bit] NOT NULL,
	[IsMinDischarged] [bit] NOT NULL,
 CONSTRAINT [PK_CreditCardExtre] PRIMARY KEY CLUSTERED 
(
	[CreditCardExtreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreditCardExtreDischarge]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditCardExtreDischarge](
	[CreditCardExtreDischargeId] [int] IDENTITY(1,1) NOT NULL,
	[DischargeAmount] [decimal](18, 2) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreditCardExtreId] [int] NOT NULL,
	[AccountTransactionId] [int] NOT NULL,
	[CreditCardId] [int] NOT NULL,
 CONSTRAINT [PK_CreditCardExtreDischarge] PRIMARY KEY CLUSTERED 
(
	[CreditCardExtreDischargeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreditCardPayment]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditCardPayment](
	[CreditCardPaymentId] [int] IDENTITY(1,1) NOT NULL,
	[InstalmentIndex] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[InstalmentDate] [datetime] NOT NULL,
	[AccountTransactionId] [int] NOT NULL,
	[CreditCardId] [int] NOT NULL,
 CONSTRAINT [PK_CreditCardPayment] PRIMARY KEY CLUSTERED 
(
	[CreditCardPaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[PersonId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[EmailAddress] [nvarchar](50) NULL,
	[IdentityNumber] [nvarchar](11) NOT NULL,
	[Password] [nvarchar](250) NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Token]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Token](
	[TokenId] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](250) NOT NULL,
	[PersonId] [int] NOT NULL,
	[ValidUntil] [datetime] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CanBeUsed] [bit] NOT NULL,
 CONSTRAINT [PK_Token] PRIMARY KEY CLUSTERED 
(
	[TokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionDetail]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionDetail](
	[TransactionDetailId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[TransactionDirection] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[AccountTransactionId] [int] NOT NULL,
	[OwnerType] [int] NOT NULL,
	[OwnerId] [int] NOT NULL,
	[OccurrenceDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TransactionDetail] PRIMARY KEY CLUSTERED 
(
	[TransactionDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionOrder]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionOrder](
	[TransactionOrderId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionTypeId] [int] NOT NULL,
	[OrderDescription] [nvarchar](150) NULL,
	[CreateDate] [datetime] NOT NULL,
	[OperationDate] [datetime] NOT NULL,
	[FromAccountId] [int] NOT NULL,
	[ToAccountId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[TransactionOrderStatusId] [int] NOT NULL,
 CONSTRAINT [PK_TransactionOrder] PRIMARY KEY CLUSTERED 
(
	[TransactionOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionStatus]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionStatus](
	[TransactionStatusId] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](50) NOT NULL,
	[StatusKey] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TransactionStatus] PRIMARY KEY CLUSTERED 
(
	[TransactionStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionType]    Script Date: 23-Nov-19 19:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionType](
	[TransactionTypeId] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[TypeKey] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TransactionOrderType] PRIMARY KEY CLUSTERED 
(
	[TransactionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AccountTransaction]  WITH CHECK ADD  CONSTRAINT [FK_AccountTransaction_TransactionStatus] FOREIGN KEY([TransactionStatusId])
REFERENCES [dbo].[TransactionStatus] ([TransactionStatusId])
GO
ALTER TABLE [dbo].[AccountTransaction] CHECK CONSTRAINT [FK_AccountTransaction_TransactionStatus]
GO
ALTER TABLE [dbo].[AccountTransaction]  WITH CHECK ADD  CONSTRAINT [FK_AccountTransaction_TransactionType] FOREIGN KEY([TransactionTypeId])
REFERENCES [dbo].[TransactionType] ([TransactionTypeId])
GO
ALTER TABLE [dbo].[AccountTransaction] CHECK CONSTRAINT [FK_AccountTransaction_TransactionType]
GO
ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK_Company_Person] FOREIGN KEY([ResponsablePersonId])
REFERENCES [dbo].[Person] ([PersonId])
GO
ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK_Company_Person]
GO
ALTER TABLE [dbo].[TransactionOrder]  WITH CHECK ADD  CONSTRAINT [FK_TransactionOrder_Account] FOREIGN KEY([FromAccountId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[TransactionOrder] CHECK CONSTRAINT [FK_TransactionOrder_Account]
GO
ALTER TABLE [dbo].[TransactionOrder]  WITH CHECK ADD  CONSTRAINT [FK_TransactionOrder_Account1] FOREIGN KEY([ToAccountId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[TransactionOrder] CHECK CONSTRAINT [FK_TransactionOrder_Account1]
GO
ALTER TABLE [dbo].[TransactionOrder]  WITH CHECK ADD  CONSTRAINT [FK_TransactionOrder_TransactionType] FOREIGN KEY([TransactionTypeId])
REFERENCES [dbo].[TransactionType] ([TransactionTypeId])
GO
ALTER TABLE [dbo].[TransactionOrder] CHECK CONSTRAINT [FK_TransactionOrder_TransactionType]
GO
