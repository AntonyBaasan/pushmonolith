
# Features

### User can install the tool from npm registry

```PowerShell
npm install -g pushmonolith 
```

### User can deploy jar web application on the AWS server

The `deploy` command does below steps:

- Create AWS server, using CloudFormation
- Copy JAR file into the server
- Install and run application as init.d service ([read more](https://docs.spring.io/spring-boot/docs/current/reference/html/deployment.html#deployment.cloud.aws))
- Setup letsencrypt certificate for domain name `app.pushmonolith.com`

``` 
pushmonolith deploy app.jar
pushmonolith deploy -file app.jar 
pushmonolith deploy -dir dist
```

## Stage 2
### User can login to pushmonolith.com

``` 
pushmonolith login
```

### Authenticated user can see list of applications

``` 
pushmonolith list
```

### User can create a database on a same server
### User can create a load balancer
### letsencrypt is setup automatically

