Feature: Login
	In order to access the website
	As a user
	I want to be able to login to the website

Background: 
	Given I navigate to the website home page

@smoke
Scenario Outline: Active user can log in successfully
	When I login with a valid users credentials <userName> <password>
	Then the user should be logged in successfully

	Examples:
		| userName                | password     |
		| standard_user           | schoolaccount |
		| standard_user1           | schoolaccount |
		| standard_user2           | schoolaccount |
#ToDo
#@smoke
#Scenario: Locked Out user cannot log in successfully
#	When I login with users credentials
#		| UserName        | Password     |
#		| locked_out_user | secret_sauce |
#	Then the user should not be logged in
