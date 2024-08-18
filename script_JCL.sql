create database jclDataBase;
use jclDataBase;

create table usuario(
id_usuario smallint(4) primary key NOT NULL auto_increment,
nome varchar(30) NOT NULL,
endereco varchar(40) NULL,
email varchar(30) NOT NULL unique,
senha varchar(16) NULL,
regiao varchar(20) NULL,
tipoUsuario varchar(11) NOT NULL); 

create table telefoneDoUsuario(
id_telefone smallint(4) PRIMARY KEY AUTO_INCREMENT NOT NULL,
id_usuario smallint(4) NOT NULL,
telefone_usuario varchar(16) NULL); 

create table historico(
id_historio smallint(4) AUTO_INCREMENT PRIMARY KEY NOT NULL,
funcionario_id smallint(4) not null,
acao varchar(50) NOT NULL,
data_historico datetime not null); 

create table produto(
id_produto smallint(4) auto_increment primary key not null,
nome varchar(20) not null,
preco decimal(5, 2) not null,
desconto decimal(5, 2),
quantidade int not null,
descricao varchar(60) not null); 

create table compra(
id_compra smallint(4) AUTO_INCREMENT PRIMARY KEY NOT NULL,
id_produto smallint(4) not null,
id_cliente smallint(4) not null,
idFuncionario smallint(4) not null,
valor_compra decimal(10, 2) not null,
data_compra datetime not null,
quantidadeItens int not null,
desconto decimal(3, 2) null,
status varchar(20) not null,
relatorio varchar(120) not null);

ALTER TABLE telefoneDoUsuario ADD
CONSTRAINT id_usuario FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario);

Alter table historico ADD 
CONSTRAINT funcionario_id FOREIGN KEY (funcionario_id) REFERENCES usuario(id_usuario);

Alter table compra ADD 
CONSTRAINT id_cliente FOREIGN KEY (id_cliente) REFERENCES usuario(id_usuario);
Alter table compra ADD 
CONSTRAINT id_produto FOREIGN KEY (id_produto) REFERENCES produto(id_produto);
Alter table compra ADD
CONSTRAINT idFuncionario FOREIGN KEY (idFuncionario) REFERENCES usuario(id_usuario);

select * from produto;
insert into usuario values(null, 'Jose Ricardo', 'Rua das carmelitas - 123', 'jose@hotmail.com', '123456789', '', 'gerente');
insert into telefoneDoUsuario values(null, 1, '(19) 11111-1111');