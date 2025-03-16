# Create new service
# First create dockerfile

# Second in docker compose 
- Add new service same structure in game service and identity service

# Third in jenkins file :
- Create new docker image variable name in environment
- Add env in stage Detect Changes
- Add if check in stage Build & Push Updated Service
- Add if check in stage Deploy Updated Service

# Scaffold Database

dotnet ef dbcontext scaffold "Host={Host};Database={DbName};Username={UserName};Password={Password}" Npgsql.EntityFrameworkCore.PostgreSQL -o Database -c WejoContext --project .\Wejo.Common.Domain -o Database -c WejoContext --force

# Notice : 

- When scaffold succced its have message like this : Could not load database collations. 
- > Just ignore its not affect anything
- Then, update IWejoContext manual if there is any new dbset. Make sure all done then build all project again
