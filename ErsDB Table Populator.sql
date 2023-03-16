Use ErsDb

INSERT INTO Employees(Name, Email, Password, Admin, ExpensesDue, ExpensesPaid)
VALUES('Bob','bob@email.com','password','true',0,0),
('Bill','bill@email.com','password','true',10,0),
('Betty','betty@email.com','password','true',110,0),
('Becky','becky@email.com','password','true',0,50),
('John','john@email.com','password','true',100,75);


INSERT INTO Items(Name, Price)
VALUES('Paper',10),
('Pens',5),
('Watercooler',100),
('Desktop',1500),
('Laptop',2000);

INSERT INTO Expenses(Description, Status, Total, EmployeeId)
VALUES('Paper order','NEW',10,1),
('Pens order','NEW',25,2),
('Watercooler order','NEW',110,3),
('Desktop','NEW',3000,4),
('Laptop order','NEW',2000,5),
('Oops need another laptop','NEW',2000,5);

INSERT INTO Expenselines(Quantity, ExpenseId, ItemId)
VALUES(1, 1, 1),
(5, 2, 2),
(1, 3, 3),
(1, 4, 4),
(1, 5, 5),
(1, 6, 5)

SELECT * FROM Employees
SELECT * FROM Items
SELECT * FROM Expenses
SELECT * FROM Expenselines

