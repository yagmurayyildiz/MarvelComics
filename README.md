# Marvel Comics Search Application

## Overview
This application is a one-page ASP.NET MVC web application designed to search Marvel Comics using the official Marvel API. It follows the principles of clean architecture and is developed using Test-Driven Development (TDD) methodologies.

## Features
- **Comic Search**: Users can search for Marvel Comics by character.
- **Advanced Search Capabilities**: 
  - **Filtering**: Refine search results based on comic title.
  - **Sorting**: Order search results by title.
  - **Pagination and Dynamic Loading**: 
    - **Adjustable Result Count**: Users can choose the number of results displayed per page.
    - **Load More**: A 'Load More' button allows for additional results to be dynamically loaded, enhancing the user experience by avoiding full page reloads.

- **Clean Architecture**: Ensures that the application is easy to maintain, extend, and test.

## Getting Started

### Prerequisites
- .NET Framework 4.8.1 or later
- Visual Studio 2019 or later (recommended for development)

### Installation
1. Clone the repository:
https://github.com/yagmurayyildiz/MarvelComics.git

2. Open the solution in Visual Studio.
3. Restore the NuGet packages.

### Configuration

#### API Keys
This application requires specific API keys to function, specifically for accessing the Marvel Comics API. These keys are not included in the source code for security reasons. 

#### Setup Instructions:
1. Obtain your Marvel API keys from the [Marvel Developer Portal](https://developer.marvel.com/).
2. Create a `secrets.config` file in the root of the MVC project and the integration tests project.
3. Add your API keys to this file. The file should look something like this:

   ```xml
   <?xml version="1.0" encoding="utf-8" ?>
    <appSettings>
     <add key="APIKey" value="YOUR_MARVEL_PUBLIC_KEY" />
     <add key="PrivateKey" value="YOUR_MARVEL_PRIVATE_KEY" />
   </appSettings>


## Project Architecture

This project is structured using Clean Architecture, which is divided into three main layers:

1. **MarvelComics.Core**: 
   - This is the heart of the application.
   - Contains all the business logic, entities, interfaces, and application-specific logic.
   - It does not depend on any other layer, making it independent of external concerns.
   - Ensures that changes in other layers have minimal impact on the business logic.

2. **MarvelComics.Infrastructure**: 
   - Implements the interfaces defined in the Core layer.
   - In this project, it includes the integration with the Marvel Comics API.

3. **MarvelComics.WebUI**:
   - The presentation layer of the application built using ASP.NET MVC.
   - It interacts with the Core layer to send and receive data.
   - Responsible for rendering the UI, handling user inputs, and presenting the data to the end-user.
   - Includes views, controllers, view models, and other MVC components.

### Integration:
- The **WebUI** layer depends on the **Core** layer for its business logic.
- The **Core** layer, through its interfaces, indirectly interacts with the **Infrastructure** layer to access external resources.
- This separation ensures that the business logic (in the Core layer) remains unaffected by changes in external dependencies or the presentation layer.

### Testability:
- Each layer is designed to be testable independently.
- The Core layer can be tested without the UI or external dependencies.
- The Infrastructure layer's integration with external APIs can be tested in isolation.
- The WebUI layer's user interface and interactions can be tested separately, ensuring a smooth user experience.

## Testing
The application is developed using TDD. Unit tests are written using nUnit and cover major functionalities, ensuring code reliability and quality.

## Usage
After running the application, navigate to the main page. Enter your search query to find comics from the Marvel universe.

## External Resources
- Marvel API
- Autofac
- log4net
- NUnit
- Moq
