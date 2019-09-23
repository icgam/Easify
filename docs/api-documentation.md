## API Documentation ([Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework))

To document the API we have choosen [Swagger](http://swagger.io/) as our tool. It is integrated with the API and it is reachable via **http://{MY-COMPANY-URL}/swagger**, this enables you to explore the API and inspect/modify resources as you wish. [Swagger](http://swagger.io/) is configured to automatically attach CORRELATION-ID to each request to make your life easier. The only thing required for [Swagger](http://swagger.io/) to work correctly is a bit of configuration in the **\*.json** or **\*.{ENV}.json** files, to be more specific [Swagger](http://swagger.io/) needs to know the **TITLE** and **VERSION** of your API. Sample config section is provided below:

```json
{
  "Application": {
    "Title": "My API",
    "Version": "2.0"
  }
}
```

[Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework)