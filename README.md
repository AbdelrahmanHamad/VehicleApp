## Live Demo (AWS)
The application is currently hosted on an AWS EC2 instance and can be accessed here:
**[http://44.201.252.42/](http://44.201.252.42/)**

# How to run the application

To run everything at once, you'll need **Docker Desktop**. You can download it here:
[https://www.docker.com/products/docker-desktop/](https://www.docker.com/products/docker-desktop/)

Once Docker is installed and running, follow these steps:

1.  Open your terminal in the project folder.
2.  Run the following command:
    ```bash
    docker compose up --build
    ```

After it finishes building and starting, you can access the app at:
*   **Web App**: [http://localhost:4200](http://localhost:4200)
*   **Backend API**: [http://localhost:7091](http://localhost:7091)
*   **Swagger API Docs**: [http://localhost:7091/swagger/index.html](http://localhost:7091/swagger/index.html)