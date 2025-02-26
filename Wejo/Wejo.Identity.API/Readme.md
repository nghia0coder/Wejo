# Build Docker Images (Development Environment)

 docker build --build-arg ENVIRONMENT=Development -t wejo_identity_api -f Wejo.Identity.API/Dockerfile . 

# Run container 

 docker run -d -p 5000:8080 -e ASPNETCORE_ENVIRONMENT=Development --name wejo_identity_api wejo_identity_api 
