create table  t_company(
   comp_id      int(10) not null,
   comp_code    varchar(32),
   cname        varchar(32),
   status       int(1) not null  default 0 ,  
   create_date 	timestamp(0) not null,
   primary key (comp_id) 
);

create table  t_shop(
   shop_id      int(10)     not null auto_increment,
   cname        varchar(32) not null ,
   comp_id      int(10) not null,
   status       int(1) not null  default 0 ,  
   create_date 	timestamp(0) not null,
   primary key (shop_id) 
);


create table  t_user(
   user_id      int(10)  not null auto_increment,
   login_id     varchar(32) not null ,
   cname        varchar(32) not null ,
   shop_id      int(10) not null,
   comp_id      int(10) not null,
   status       int(1)  not null  default 0 ,  
   create_date 	timestamp(0) not null,
   primary key (t_user) 
);


drop table t_products;
create table t_products  (
  prod_id 	 int(10) not null auto_increment,
  company_code 	 varchar(128),
  shop_code      int(10),
  rfid_no 	 varchar(32),
  caizhi_code 	 varchar(32),
  peidai_code 	 varchar(32),
  tezheng_code 	 varchar(32),
  xiangqian_code varchar(32),
  prod_name 	varchar(256),
  price 	double(12, 2),
  status	int(1) not null  default 0 ,  
  remark 	text,
  create_date 	timestamp(0) not null,
  primary key (prod_id) 
);



drop table t_inventory;
create table t_inventory  (
  id 	 int(20) not null auto_increment,
  rfid_no 	 varchar(32),
  create_date 	timestamp(0) not null,
  primary key (id) 
);


drop table  t_param;
create table  t_param(
   para_id        int(10) not null,
   para_code      varchar(32),
   parent_para_id int(10),
   cname        varchar(32),
   status       int(1) null default 0 ,  
   create_date 	timestamp(0) null  ,
   primary key (para_id) 
);



insert into t_param(para_id, parent_para_id, cname) values(10,  0,  '材质');

insert into t_param(para_id, parent_para_id, cname) values(101, 10, '黄金');
insert into t_param(para_id, parent_para_id, cname) values(102, 10, '钻石');
insert into t_param(para_id, parent_para_id, cname) values(103, 10, 'K金');
insert into t_param(para_id, parent_para_id, cname) values(104, 10, '14K金');
insert into t_param(para_id, parent_para_id, cname) values(105, 10, '18K金');
insert into t_param(para_id, parent_para_id, cname) values(106, 10, '铂金');
insert into t_param(para_id, parent_para_id, cname) values(107, 10, '950铂金');
insert into t_param(para_id, parent_para_id, cname) values(108, 10, '银饰');
insert into t_param(para_id, parent_para_id, cname) values(109, 10, '珍珠');
insert into t_param(para_id, parent_para_id, cname) values(110, 10, '宝石');
insert into t_param(para_id, parent_para_id, cname) values(111, 10, '足金');
insert into t_param(para_id, parent_para_id, cname) values(112, 10, '手表');


insert into t_param(para_id, parent_para_id, cname) values(20,  0,  '佩戴方式');

insert into t_param(para_id, parent_para_id, cname) values(200,  20,  '吊坠');
insert into t_param(para_id, parent_para_id, cname) values(201,  20,  '戒指');
insert into t_param(para_id, parent_para_id, cname) values(202,  20,  '项链');
insert into t_param(para_id, parent_para_id, cname) values(203,  20,  '素链');
insert into t_param(para_id, parent_para_id, cname) values(204,  20,  '坠链');
insert into t_param(para_id, parent_para_id, cname) values(205,  20,  '手镯');
insert into t_param(para_id, parent_para_id, cname) values(206,  20,  '耳饰');
insert into t_param(para_id, parent_para_id, cname) values(207,  20,  '手链/脚链');
insert into t_param(para_id, parent_para_id, cname) values(208,  20,  '金条/金章');
insert into t_param(para_id, parent_para_id, cname) values(209,  20,  '胸花');
insert into t_param(para_id, parent_para_id, cname) values(210,  20,  '袖扣');
insert into t_param(para_id, parent_para_id, cname) values(211,  20,  '银条/银章');
insert into t_param(para_id, parent_para_id, cname) values(212,  20,  '摆件/礼品');
insert into t_param(para_id, parent_para_id, cname) values(213,  20,  '定制');


insert into t_param(para_id, parent_para_id, cname) values(30,  0,  '特征');

insert into t_param(para_id, parent_para_id, cname) values(301, 30, '无镶');
insert into t_param(para_id, parent_para_id, cname) values(302, 30, '钻石0.25Car');
insert into t_param(para_id, parent_para_id, cname) values(303, 30, '钻石0.30Car');
insert into t_param(para_id, parent_para_id, cname) values(304, 30, '彩宝100');



insert into t_param(para_id, parent_para_id, cname) values(40,  0,  '手寸');

insert into t_param(para_id, parent_para_id, cname) values(400,  40,  '15');
insert into t_param(para_id, parent_para_id, cname) values(401,  40,  '16');
insert into t_param(para_id, parent_para_id, cname) values(402,  40,  '18');
insert into t_param(para_id, parent_para_id, cname) values(403,  40,  '19');
insert into t_param(para_id, parent_para_id, cname) values(404,  40,  '20');
insert into t_param(para_id, parent_para_id, cname) values(405,  40,  '23');
insert into t_param(para_id, parent_para_id, cname) values(406,  40,  '25');
insert into t_param(para_id, parent_para_id, cname) values(407,  40,  '36');
insert into t_param(para_id, parent_para_id, cname) values(408,  40,  '40');
insert into t_param(para_id, parent_para_id, cname) values(409,  40,  '42');
insert into t_param(para_id, parent_para_id, cname) values(410,  40,  '46');

update  t_param set para_code = para_id;

