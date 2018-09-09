# CrudDemo
Short demo application    
[Live Site](https://testblz20180508040048.azurewebsites.net/)   
[Testing Repo](https://github.com/JordanMService/DemoTests)   
[Airtable View](https://airtable.com/invite/l?inviteId=invZZXZh1kaMBQ6QA&inviteToken=d4a56a98c27e2a0cd17d0ac3054e9d5367c293792ec5a6c4c73f080f60af0d03)  


### Frameworks
Frameworks and reasoning
  - Front end
    - React : Chosen for it's ability to properly seperate concerns and prototype quickly
    - Typescript : Chosen as it adds a level of stability and readability to Javascript as well as allowing for several Es6 features to be used
  - Back end
    - DotNetCore : Chosen as a simple cross platform web framework with built in inversion of control and secret managment
    - Airtable: Chosen as a simple data store as the data being stored is non relational and this is a free option I've been meaning to take a look at
  - Cloud
    - Azure : Chosen for it's easy deployment of a DotNetCore application
  - Testing
    - Xunit : Chosen as a simple test runner
    - Moq: Chosen for its ability to mock away system level dependancies
    
### Not Included Frameworks
Potential frameworks to add
  - Front end
    - Sass or PostCss as CSS frameworks to allow for more complex styling, Not implemented as the requirements for displaying did not need this level.
  - Back end
    - SignalR would have allowed for a subscription and pushing to clients instead of the client polling, this would be usefull in a larger application but since the usage of this app is quite small SignalR would have just been wasting cycles.
  
### Selected Tests
Testing was added to the ItemRepo class of the backend this was selected for the following reasonse
  - This is the most critical part of the application as it has the most complicated backend logic
  - If others are using our API it is important to have confidence in the core logic
  
### Input Validation
  - All inputs are validated on the front and backend
  - Key submit fields are kept disabled untill proper formatting is applied
  - Inputs are also verified on the backend and return appropriate error messages if needed
 
### Best Practices Followed
  - Frontend
    - Concerns properly seperated
    - Local scopes contain all data needed within the domain
    - Interfaces are used to ensure correct types are used in components
  - Backend
    - Repository logic is removed from controllers
    - Api controllers deal only with routing and HttpStatuses
    - All nessecary classes registered with IoC
    - API Keys kept in secrets file to keep out of source control
    - System libraries abstracted to providers to ensure testability
  
  
  
  
  
  
  
