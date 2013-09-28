--创建超市信息表，数据库类型：MySQL
--执行前,请先创建数据库: SuperMarket

create table [客户表]
(
[客户号] varchar(38) primary key, 
[姓名] varchar(20) not null, 
[性别] BOOL null,
[出生日期] datetime null,
[联系电话] varchar(30) not null,
[联系地址] varchar(200) null,
[积分] int
)
;
create table [雇员表]
(
[工号] varchar(38) primary key, 
[姓名] varchar(20) not null, 
[性别] BOOL not null,
[出生日期] datetime null,
[入职时间] datetime not null,
[职务名称] varchar(10) null
)
;

create table [设备表]
(
[编号] varchar(38) primary key,
[设备名称] varchar(50) not null,
[型号] varchar(50) null,
[购置时间] datetime null,
[备注] varchar(250) null
)
;

create table [商品信息表]
(
[条码号] varchar(38) primary key,
[商品名称] varchar(50) not null,
[厂商名称] varchar(50) null,
[保质期] int null
)
;
create table [存货信息表]
(
[存货记录号] integer primary key AUTO_INCREMENT,
[条码号] varchar(38) not null,
[售价] double  not null,
[成本价] double not null,
[生产日期] datetime ,
[上货时间] datetime ,
[库存数量] int not null
)
;

create table [商品销售单据表]
(
[销售单号] integer primary key AUTO_INCREMENT,
[销售日期] datetime not null,
[终端号] varchar(38) null,
[客户号] varchar(38) null,
[销售员号] varchar(38) null,
[销售类别] varchar(10)
)
;

create table [商品销售记录表]
(
[销售记录号]  integer primary key AUTO_INCREMENT,
[销售单号] int not null,
[商品条码] varchar(38) not null,
[单价] double not null,
[数量] int not null
)
;


