create domain emailData as varchar(50)(
CHECK (VALUE ~ '^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})'::text)
)

create domain phoneNumberData as varchar(13)(
CHECK (VALUE ~ '^(+([0-9]{1,3})([0-9]{10})'::text)
)

create table client(
login varchar(20) not null,
email edailData not null,
name varchar(50) nut null,
birthday date,
city varchar(20),
password varchar(50) not null,--mb hash
phoneNumber phoneNumberData,
isOrganization bool not null,
site varchar(50),
acceptedEvents int[],
PRIMARY KEY (login)
)

create table event(
id serial primary key,
createdBy varchar(20) NOT NULL REFERENCES client(login) ON DELETE CASCADE,
name varchar(100) not null,
type ENUM('eco', 'zoo', 'phone', 'intelectual', 'school', 'homeless', 'families', 'inclusive', 'culture', 'medecine') not null,
place varchar(100) not null,
time datetime not null,
description text
)


--Volonter Insert
--Insert into client(login, email, name, birthday, city, password, phoneNumber, isOrganization)
--values(
--'volonter1',
--'email1@gmail.com',
--'Volonter S.V.',
--'2020-02-02',
--'Lviv',
--'1234'
--'+380123456789',
--false
--)
--same for Organization if some data wasn't provided from user, don't add field if it can be null

--Ivent Insert
--Insert into event(createdBy, name, type, place, time, description)
--values(
--'volonter1' --Ми вприниципі знаєм хто створює нову подію, тому можна і не селектити а просто з проги взяти логін
--'some name'
--'culture'
--'Lviv Opera House'
--'2021-05-25 12:00:00'
--'Some description'
--)

