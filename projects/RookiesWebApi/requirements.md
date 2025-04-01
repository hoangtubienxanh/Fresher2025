**MVC .NET Core Assignment #1**

Create the Person model with following info:
• First Name
• Last Name
• Gender
• Date Of Birth
• Phone Number
• Birth Place
• Is Graduated (Yes/No)
After having the model, you should prepare the dummy data.
From the model above, write a MVC application that contains Rookies controller with the following
actions:

1. Action to return a list of members who is Male
2. Action to return the oldest one based on “Age” (if return more than one record then select first
   record)
3. Action to return a new list that contains Full Name only ( Full Name = Last Name + First Name)
4. Action to redirect to the following actions based on the value provided by end user via Query
   String (action param) :
   o Action to return list of members who has birth year is 2000
   o Action to return list of members who has birth year greater than 2000
   o Action to return list of members who has birth year less than 2000
5. Action to return the excel file that contains dummy data of list persons
6. Define the new Route that to include “NashTech” before controller then action. For example:
   https://localhost:5001/NashTech/Rookies/Index
   Objectives:

- The requirement doesn’t require implementing a View to display the result. Just return text
  content, that’s fine.
- Rookie should know which Action Result should be applied.

**Assignment #2**
Reuse Model and Action controller from MVC .NET Core Assignment #1

1. Implement views to display all result data for actions in assignment #1.
2. Implement an Interface for Person class. The interface should have Create, Update, Delete and List all
   people function.
3. Use Dependency Injection to register above interface in the Controller
4. Add new form to Create/ Edit Person.
5. Implement a screen to display the list of people. User can click on the Person Name to navigate to
   Person Details screen. The Person Details screen should display full information of selected person. On
   Person Details screen, there is a “Delete” button which allows user to delete the selected person. After
   deleting successfully, the application will redirect user to the Confirmation screen with the message:
   Person [deleted person name] was removed from the list successfully!”
   Optional: Applied paging in the list of people screen.