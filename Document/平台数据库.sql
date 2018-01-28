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



insert into t_param(para_id, parent_para_id, cname) values(10,  0,  '����');

insert into t_param(para_id, parent_para_id, cname) values(101, 10, '�ƽ�');
insert into t_param(para_id, parent_para_id, cname) values(102, 10, '��ʯ');
insert into t_param(para_id, parent_para_id, cname) values(103, 10, 'K��');
insert into t_param(para_id, parent_para_id, cname) values(104, 10, '14K��');
insert into t_param(para_id, parent_para_id, cname) values(105, 10, '18K��');
insert into t_param(para_id, parent_para_id, cname) values(106, 10, '����');
insert into t_param(para_id, parent_para_id, cname) values(107, 10, '950����');
insert into t_param(para_id, parent_para_id, cname) values(108, 10, '����');
insert into t_param(para_id, parent_para_id, cname) values(109, 10, '����');
insert into t_param(para_id, parent_para_id, cname) values(110, 10, '��ʯ');
insert into t_param(para_id, parent_para_id, cname) values(111, 10, '���');
insert into t_param(para_id, parent_para_id, cname) values(112, 10, '�ֱ�');


insert into t_param(para_id, parent_para_id, cname) values(20,  0,  '�����ʽ');

insert into t_param(para_id, parent_para_id, cname) values(200,  20,  '��׹');
insert into t_param(para_id, parent_para_id, cname) values(201,  20,  '��ָ');
insert into t_param(para_id, parent_para_id, cname) values(202,  20,  '����');
insert into t_param(para_id, parent_para_id, cname) values(203,  20,  '����');
insert into t_param(para_id, parent_para_id, cname) values(204,  20,  '׹��');
insert into t_param(para_id, parent_para_id, cname) values(205,  20,  '����');
insert into t_param(para_id, parent_para_id, cname) values(206,  20,  '����');
insert into t_param(para_id, parent_para_id, cname) values(207,  20,  '����/����');
insert into t_param(para_id, parent_para_id, cname) values(208,  20,  '����/����');
insert into t_param(para_id, parent_para_id, cname) values(209,  20,  '�ػ�');
insert into t_param(para_id, parent_para_id, cname) values(210,  20,  '���');
insert into t_param(para_id, parent_para_id, cname) values(211,  20,  '����/����');
insert into t_param(para_id, parent_para_id, cname) values(212,  20,  '�ڼ�/��Ʒ');
insert into t_param(para_id, parent_para_id, cname) values(213,  20,  '����');


insert into t_param(para_id, parent_para_id, cname) values(30,  0,  '����');

insert into t_param(para_id, parent_para_id, cname) values(301, 30, '����');
insert into t_param(para_id, parent_para_id, cname) values(302, 30, '��ʯ0.25Car');
insert into t_param(para_id, parent_para_id, cname) values(303, 30, '��ʯ0.30Car');
insert into t_param(para_id, parent_para_id, cname) values(304, 30, '�ʱ�100');



insert into t_param(para_id, parent_para_id, cname) values(40,  0,  '�ִ�');

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

