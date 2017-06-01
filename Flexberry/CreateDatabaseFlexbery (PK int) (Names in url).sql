USE FlexberryPractice

CREATE TABLE [Purchases] (

	 [PrimaryKey] int  NOT NULL,

	 [Date] DATETIME  NOT NULL,

	 [User_Id] int  NOT NULL,

	 [File_Id] int  NOT NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [ContentTypes] (

	 [PrimaryKey] int  NOT NULL,

	 [Type] VARCHAR(255)  NOT NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [FileUser] (

	 [Files_Id] int  NOT NULL,

	 [Users_Id] int  NOT NULL)


CREATE TABLE [Operations] (

	 [PrimaryKey] int  NOT NULL,

	 [SomeValue] INT  NOT NULL,

	 [SecondaryFile_Id] int  NULL,

	 [MainFile_Id] int  NOT NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [Users] (

	 [PrimaryKey] int  NOT NULL,

	 [Name] VARCHAR(255)  NOT NULL,

	 [Email] VARCHAR(255)  NOT NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [Files] (

	 [PrimaryKey] int  NOT NULL,

	 [Description] VARCHAR(255)  NULL,

	 [Url] VARCHAR(255)  NOT NULL,

	 [Price] FLOAT  NOT NULL,

	 [ContentType_Id] int  NOT NULL,

	 [PdfFile_Id] int  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMNETLOCKDATA] (

	 [LockKey] VARCHAR(300)  NOT NULL,

	 [UserName] VARCHAR(300)  NOT NULL,

	 [LockDate] DATETIME  NULL,

	 PRIMARY KEY ([LockKey]))


CREATE TABLE [STORMSETTINGS] (

	 [PrimaryKey] int  NOT NULL,

	 [Module] varchar(1000)  NULL,

	 [Name] varchar(255)  NULL,

	 [Value] text  NULL,

	 [User] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMAdvLimit] (

	 [PrimaryKey] int  NOT NULL,

	 [User] varchar(255)  NULL,

	 [Published] bit  NULL,

	 [Module] varchar(255)  NULL,

	 [Name] varchar(255)  NULL,

	 [Value] text  NULL,

	 [HotKeyData] int  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMFILTERSETTING] (

	 [PrimaryKey] int  NOT NULL,

	 [Name] varchar(255)  NOT NULL,

	 [DataObjectView] varchar(255)  NOT NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMWEBSEARCH] (

	 [PrimaryKey] int  NOT NULL,

	 [Name] varchar(255)  NOT NULL,

	 [Order] INT  NOT NULL,

	 [PresentView] varchar(255)  NOT NULL,

	 [DetailedView] varchar(255)  NOT NULL,

	 [FilterSetting_m0] int  NOT NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMFILTERDETAIL] (

	 [PrimaryKey] int  NOT NULL,

	 [Caption] varchar(255)  NOT NULL,

	 [DataObjectView] varchar(255)  NOT NULL,

	 [ConnectMasterProp] varchar(255)  NOT NULL,

	 [OwnerConnectProp] varchar(255)  NULL,

	 [FilterSetting_m0] int  NOT NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMFILTERLOOKUP] (

	 [PrimaryKey] int  NOT NULL,

	 [DataObjectType] varchar(255)  NOT NULL,

	 [Container] varchar(255)  NULL,

	 [ContainerTag] varchar(255)  NULL,

	 [FieldsToView] varchar(255)  NULL,

	 [FilterSetting_m0] int  NOT NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [UserSetting] (

	 [PrimaryKey] int  NOT NULL,

	 [AppName] varchar(256)  NULL,

	 [UserName] varchar(512)  NULL,

	 [UserGuid] int  NULL,

	 [ModuleName] varchar(1024)  NULL,

	 [ModuleGuid] int  NULL,

	 [SettName] varchar(256)  NULL,

	 [SettGuid] int  NULL,

	 [SettLastAccessTime] DATETIME  NULL,

	 [StrVal] varchar(256)  NULL,

	 [TxtVal] varchar(max)  NULL,

	 [IntVal] int  NULL,

	 [BoolVal] bit  NULL,

	 [GuidVal] int  NULL,

	 [DecimalVal] decimal(20,10)  NULL,

	 [DateTimeVal] DATETIME  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [ApplicationLog] (

	 [PrimaryKey] int  NOT NULL,

	 [Category] varchar(64)  NULL,

	 [EventId] INT  NULL,

	 [Priority] INT  NULL,

	 [Severity] varchar(32)  NULL,

	 [Title] varchar(256)  NULL,

	 [Timestamp] DATETIME  NULL,

	 [MachineName] varchar(32)  NULL,

	 [AppDomainName] varchar(512)  NULL,

	 [ProcessId] varchar(256)  NULL,

	 [ProcessName] varchar(512)  NULL,

	 [ThreadName] varchar(512)  NULL,

	 [Win32ThreadId] varchar(128)  NULL,

	 [Message] varchar(2500)  NULL,

	 [FormattedMessage] varchar(max)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMAG] (

	 [PrimaryKey] int  NOT NULL,

	 [Name] varchar(80)  NOT NULL,

	 [Login] varchar(50)  NULL,

	 [Pwd] varchar(50)  NULL,

	 [IsUser] bit  NOT NULL,

	 [IsGroup] bit  NOT NULL,

	 [IsRole] bit  NOT NULL,

	 [ConnString] varchar(255)  NULL,

	 [Enabled] bit  NULL,

	 [Email] varchar(80)  NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMLG] (

	 [PrimaryKey] int  NOT NULL,

	 [Group_m0] int  NOT NULL,

	 [User_m0] int  NOT NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMI] (

	 [PrimaryKey] int  NOT NULL,

	 [User_m0] int  NOT NULL,

	 [Agent_m0] int  NOT NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [Session] (

	 [PrimaryKey] int  NOT NULL,

	 [UserKey] int  NULL,

	 [StartedAt] datetime  NULL,

	 [LastAccess] datetime  NULL,

	 [Closed] bit  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMS] (

	 [PrimaryKey] int  NOT NULL,

	 [Name] varchar(100)  NOT NULL,

	 [Type] varchar(100)  NULL,

	 [IsAttribute] bit  NOT NULL,

	 [IsOperation] bit  NOT NULL,

	 [IsView] bit  NOT NULL,

	 [IsClass] bit  NOT NULL,

	 [SharedOper] bit  NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMP] (

	 [PrimaryKey] int  NOT NULL,

	 [Subject_m0] int  NOT NULL,

	 [Agent_m0] int  NOT NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMF] (

	 [PrimaryKey] int  NOT NULL,

	 [FilterText] varchar(MAX)  NULL,

	 [Name] varchar(255)  NULL,

	 [FilterTypeNView] varchar(255)  NULL,

	 [Subject_m0] int  NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMAC] (

	 [PrimaryKey] int  NOT NULL,

	 [TypeAccess] varchar(7)  NULL,

	 [Filter_m0] int  NULL,

	 [Permition_m0] int  NOT NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMLO] (

	 [PrimaryKey] int  NOT NULL,

	 [Class_m0] int  NOT NULL,

	 [Operation_m0] int  NOT NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMLA] (

	 [PrimaryKey] int  NOT NULL,

	 [View_m0] int  NOT NULL,

	 [Attribute_m0] int  NOT NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMLV] (

	 [PrimaryKey] int  NOT NULL,

	 [Class_m0] int  NOT NULL,

	 [View_m0] int  NOT NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))


CREATE TABLE [STORMLR] (

	 [PrimaryKey] int  NOT NULL,

	 [StartDate] datetime  NULL,

	 [EndDate] datetime  NULL,

	 [Agent_m0] int  NOT NULL,

	 [Role_m0] int  NOT NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([PrimaryKey]))




 ALTER TABLE [Purchases] ADD CONSTRAINT [Purchases_FUsers_0] FOREIGN KEY ([User_Id]) REFERENCES [Users]
CREATE INDEX Purchases_IUser_Id on [Purchases] ([User_Id])

 ALTER TABLE [Purchases] ADD CONSTRAINT [Purchases_FFiles_0] FOREIGN KEY ([File_Id]) REFERENCES [Files]
CREATE INDEX Purchases_IFile_Id on [Purchases] ([File_Id])

 ALTER TABLE [FileUser] ADD CONSTRAINT [FileUser_FFiles_0] FOREIGN KEY ([Files_Id]) REFERENCES [Files]
CREATE INDEX FileUser_IFiles_Id on [FileUser] ([Files_Id])

 ALTER TABLE [FileUser] ADD CONSTRAINT [FileUser_FUsers_0] FOREIGN KEY ([Users_Id]) REFERENCES [Users]
CREATE INDEX FileUser_IUsers_Id on [FileUser] ([Users_Id])

 ALTER TABLE [Operations] ADD CONSTRAINT [Operations_FFiles_0] FOREIGN KEY ([SecondaryFile_Id]) REFERENCES [Files]
CREATE INDEX Operations_ISecondaryFile_Id on [Operations] ([SecondaryFile_Id])

 ALTER TABLE [Operations] ADD CONSTRAINT [Operations_FFiles_1] FOREIGN KEY ([MainFile_Id]) REFERENCES [Files]
CREATE INDEX Operations_IMainFile_Id on [Operations] ([MainFile_Id])

 ALTER TABLE [Files] ADD CONSTRAINT [Files_FFiles_0] FOREIGN KEY ([PdfFile_Id]) REFERENCES [Files]
CREATE INDEX Files_IPdfFile_Id on [Files] ([PdfFile_Id])

 ALTER TABLE [Files] ADD CONSTRAINT [Files_FContentTypes_0] FOREIGN KEY ([ContentType_Id]) REFERENCES [ContentTypes]
CREATE INDEX Files_IContentType_Id on [Files] ([ContentType_Id])

 ALTER TABLE [STORMWEBSEARCH] ADD CONSTRAINT [STORMWEBSEARCH_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

 ALTER TABLE [STORMFILTERDETAIL] ADD CONSTRAINT [STORMFILTERDETAIL_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

 ALTER TABLE [STORMFILTERLOOKUP] ADD CONSTRAINT [STORMFILTERLOOKUP_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

 ALTER TABLE [STORMLG] ADD CONSTRAINT [STORMLG_FSTORMAG_0] FOREIGN KEY ([Group_m0]) REFERENCES [STORMAG]

 ALTER TABLE [STORMLG] ADD CONSTRAINT [STORMLG_FSTORMAG_1] FOREIGN KEY ([User_m0]) REFERENCES [STORMAG]

 ALTER TABLE [STORMI] ADD CONSTRAINT [STORMI_FSTORMAG_0] FOREIGN KEY ([User_m0]) REFERENCES [STORMAG]

 ALTER TABLE [STORMI] ADD CONSTRAINT [STORMI_FSTORMAG_1] FOREIGN KEY ([Agent_m0]) REFERENCES [STORMAG]

 ALTER TABLE [STORMP] ADD CONSTRAINT [STORMP_FSTORMS_0] FOREIGN KEY ([Subject_m0]) REFERENCES [STORMS]

 ALTER TABLE [STORMP] ADD CONSTRAINT [STORMP_FSTORMAG_0] FOREIGN KEY ([Agent_m0]) REFERENCES [STORMAG]

 ALTER TABLE [STORMF] ADD CONSTRAINT [STORMF_FSTORMS_0] FOREIGN KEY ([Subject_m0]) REFERENCES [STORMS]

 ALTER TABLE [STORMAC] ADD CONSTRAINT [STORMAC_FSTORMF_0] FOREIGN KEY ([Filter_m0]) REFERENCES [STORMF]

 ALTER TABLE [STORMAC] ADD CONSTRAINT [STORMAC_FSTORMP_0] FOREIGN KEY ([Permition_m0]) REFERENCES [STORMP]

 ALTER TABLE [STORMLO] ADD CONSTRAINT [STORMLO_FSTORMS_0] FOREIGN KEY ([Class_m0]) REFERENCES [STORMS]

 ALTER TABLE [STORMLO] ADD CONSTRAINT [STORMLO_FSTORMS_1] FOREIGN KEY ([Operation_m0]) REFERENCES [STORMS]

 ALTER TABLE [STORMLA] ADD CONSTRAINT [STORMLA_FSTORMS_0] FOREIGN KEY ([View_m0]) REFERENCES [STORMS]

 ALTER TABLE [STORMLA] ADD CONSTRAINT [STORMLA_FSTORMS_1] FOREIGN KEY ([Attribute_m0]) REFERENCES [STORMS]

 ALTER TABLE [STORMLV] ADD CONSTRAINT [STORMLV_FSTORMS_0] FOREIGN KEY ([Class_m0]) REFERENCES [STORMS]

 ALTER TABLE [STORMLV] ADD CONSTRAINT [STORMLV_FSTORMS_1] FOREIGN KEY ([View_m0]) REFERENCES [STORMS]

 ALTER TABLE [STORMLR] ADD CONSTRAINT [STORMLR_FSTORMAG_0] FOREIGN KEY ([Agent_m0]) REFERENCES [STORMAG]

 ALTER TABLE [STORMLR] ADD CONSTRAINT [STORMLR_FSTORMAG_1] FOREIGN KEY ([Role_m0]) REFERENCES [STORMAG]
