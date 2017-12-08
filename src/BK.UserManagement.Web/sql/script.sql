//Right click on Connection> New Connection
//username=sys; password=...; role=SYSDBA; SID=db11g??

create user sec_admin identified by Password123;
grant create session to sec_admin;
grant create user to sec_admin;
grant drop user to sec_admin;
grant select on dba_users to sec_admin;

create table sec_user(
  ID number not null primary key,
  USER_ID number,
  USERNAME varchar2(30),
  PASSWORD varchar2(30)
);

select * from user$;

CREATE TABLESPACE tbs_perm_01
  DATAFILE 'tbs_perm_01.dat' 
    SIZE 20M
  ONLINE;
  
  CREATE TABLESPACE tbs_perm_02
  DATAFILE 'tbs_perm_02.dat' 
    SIZE 10M
    REUSE
    AUTOEXTEND ON NEXT 10M MAXSIZE 200M;
    
    
CREATE TEMPORARY TABLESPACE tbs_temp_01
  TEMPFILE 'tbs_temp_01.dbf'
    SIZE 5M
    AUTOEXTEND ON;
	
	
create table dba_user_info(
  USERNAME varchar2(30) not null,
  FIRST_NAME varchar2(255),
  LAST_NAME varchar2(255), 
  ADDRESS varchar2(255),
  PHONE varchar2(255),
  EMAIL varchar2(255)
);

grant select,insert,update,delete on sys.dba_user_info to system;
grant select on sys.dba_profiles to system with grant option; //Login by sysdba
grant select on sys.dba_users to john;
insert into dba_user_info(USERNAME, FIRST_NAME, LAST_NAME, ADDRESS, PHONE, EMAIL) VALUES( 'JOHN', 'Jogger', 'John', 'TPHCM', '0909112233', 'john112@gmail.com')
select * from sys.dba_user_info;