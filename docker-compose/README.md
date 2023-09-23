# Working with Docker-Compose

There are some prerequisites for using the included docker-compose.yml files:

1) Make sure you have docker installed (on windows install docker desktop)

2) Create and install an https certificate:

    ```
    dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\cert.pfx -p password!
    ```

3) It's possible that the above step gives you an `A valid HTTPS certificate is already present` error.
   In that case you will have to run the following command, and then  `Re-Run Step 2`

    ```
     dotnet dev-certs https --clean
    ```

4) Trust the certificate

    ```
     dotnet dev-certs https --trust
    ```


## Docker-Compose Commands


1) MSSQL
```
docker-compose -f docker-compose.mssql.yml up -d
docker-compose -f docker-compose.mssql.yml down
```

## Specifications

