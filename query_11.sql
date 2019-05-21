use falcon_vendo;

select * from employees;
select * from products;



INSERT INTO `falcon_vendo`.`employees` (`fname`, `lname`, `mname`, `credit`) VALUES ('JUSTINE', 'LASERNA', 'I', 0);
INSERT INTO `falcon_vendo`.`employees` (`fname`, `lname`, `mname`, `credit`) VALUES ('EMMANUEL', 'JAMORALIN', NULL, 0);
INSERT INTO `falcon_vendo`.`employees` (`fname`, `lname`, `mname`, `credit`) VALUES ('JETHRO', 'TORREGOZA', NULL, 0);

INSERT INTO `falcon_vendo`.`products` (`productName`, `price`, `Quantity`) VALUES ('Nescafe 3-in-1', 8, 10);
INSERT INTO `falcon_vendo`.`products` (`productName`, `price`, `Quantity`) VALUES ('Marlboro Red', 60, 5);
INSERT INTO `falcon_vendo`.`products` (`productName`, `price`, `Quantity`) VALUES ('Redhorse 1L', 80, 5);

show databases;

use falcon_vendo;

show tables;

select * from products;

alter table products
drop column is_sale;

alter table products
add column is_sale bool	default 0;

update products
set is_sale = 1
where id <13;
where id <3;