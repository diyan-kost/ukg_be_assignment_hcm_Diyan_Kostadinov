## Description
A simple application for managing people in a small company, that can be developed further, to add new functionalities and support growing needs. 

The application currently supports three kinds of users - Employees, Managers and HR Admins. The user is associated with an employee but an employee may not have a user account. The user logs in with a username and a password. Upon successfull login they have access to different functionalities depending on their role:

* Employees can only view their own data. They can also edit some of their own data like names, address and user password but can't edit things like national id number.
* Managers, like employees can view their own data and edit parts of it, while additionaly being able to view their employees' data and edit some things like names and address.
* HR Admins have the highest access - they can view all employees in the company, view their data, edit almost all of their data, update salaries, create user accounts for employees that don't have, delete users and delete employees.

## TODO
Due to time constraints some things couldn't be finished in time. Further development of the app would include:

* Implement Entity Framework migrations
* More and fuller unit and integration tests
* More functionalities like being able to request paid leave
* Front end and user experience improvements

## Technologies used

* .NET 8
* Razor pages
* MSSQL
* Entity Framework

## Test data
Upon startup the application seeds test data to the database. The users include:

* HR Admin 
     * Username: miroslav.zahariev
     * Password: 1
* Manager
    * Username: yavor.alexandrov
    * Password: 1
* Employee: 
    * Username: nataliya.stoeva
    * Password: 1


