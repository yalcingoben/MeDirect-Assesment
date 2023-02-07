# MeDirect Exchange Project

# Installation

You have to install **Docker Desktop** before using this application. Then you can follow the below steps.

#### Open the terminal in the project root folder. Run the below command.
```sh
docker-compose up -d
```
 # Usage
 API Swagger EndPoint : [http://localhost:9000/swagger/index.html](http://localhost:9000/swagger/index.html).
 
 You can use an API tester such as Postman to be able to send the request. But you have to have an API key to be able to reach API endpoints. I defined some API Keys under the **Valid Api Keys** in **appSettings.Development.json**. 
 
 **Valid API Keys :** [ "abc", "abcd", "abcde", "abcdef" ]
 
 ### Trade Request Example
 ![image](https://user-images.githubusercontent.com/7137668/217282854-6d4418a9-9719-4735-9d18-3c3651fee7e0.png)

# Authentication

API Keys are stored in the **appSettings.Development.json** file. It might store in a database solution in the following days.

# Features

1. Fixer and ExchangeRateAPI integration completed. You can do it from **appSettings.Development.json** if you want to change it.
2. Redis is used for Caching.
3. MongoDB is used for Exception Logging and saving the Trading History.
4. Rate Limiting feature is completed. You can reach the **Rate Limiting** node from **appSettings.Development.json**, if you want to change the RateLimiting parameters.
5. Unit test has been written for all projects.

