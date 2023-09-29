# AccessControl
This is a simple access control application.
When the application starts, the user has two options available, Register or LogIn.

Register Feature:
The register button will open a form where the user must insert the necessary details. From this form one can also set admin rights. (Admin Rights option will only be available if a user with admin rights is logged in) With admin rights the user will have access to the database and will be able to modify and delete data. Before saving the data in the database, the code will check if the id card number already exist in the database and if the same number is found, a notification will appear on top of the screen saying that data with the same id card number already exist and therefore the data will not be saved. All the fields needs to be filled in order for the data to be saved in the database. If a field is missing data a message under the field will be displayed saying that data is required. If all fields are successfully filled with no errors, on submit the data will be saved in the database and a message on top of the screen will appear confirming that data was successfully saved. The new registered user will then receive an email with his/her credentials.

LogIn Feature:
To log into the system, the user Id and the id card number associated with it must be entered for the logIn to be successfull. If the logIn is successfull, the text of the button will change to Log off. Once a user is successfully logged in, the Manage Database option will appear in the navigation bar.

Manage Database Feature:
Under the manage database option one will find three options, which are Display All Users, Delete User & Edit User Details. These featurs will be all available if the user will have admin rights. On the other hand if the user doesn't have admin rights, the Delete button will be disabled and the Edit options will be available but admin rights will not be accessable. 

Display All Users:
This option will display all the users in the database. Apart from showing all the users, one can also filter by UserId, Name or Id Card Number. 

Delete User Feature:
This will delete a user by entering the user Id. **(To add a feature where the logged in user cannot be deleted)**

Edit User Details Feature:
This will open a form where the user must enter a userId which details need to be edited. By pressing the download button, if the user exists, the data will be displayed in the fields below ready for editing. The Save Data button will also appear at the buttom of the screen if downlaod is successfull. **(To add a feature where the logged in user cannot be edited)**



