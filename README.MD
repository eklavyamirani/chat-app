
## Prerequisites

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/#scenario-two-install-the-compose-plugin)
- [VS Code w/ dev container extension](https://code.visualstudio.com/docs/devcontainers/tutorial)

## Additional Information

- The backend uses Entity Framework Core with PostgreSQL.
- The frontend is built using React App.

## Setup

### Using Dev Containers

1. **Open in VS Code**: Open the repository in Visual Studio Code.
2. **Reopen in Container**: Use the command palette (`Ctrl+Shift+P`) and select `Remote-Containers: Reopen in Container`.

## Configuration

### Backend

- Configuration files are located in the [`ChatAppBackend`](command:_github.copilot.openRelativePath?%5B%7B%22scheme%22%3A%22file%22%2C%22authority%22%3A%22%22%2C%22path%22%3A%22%2FUsers%2Feklavyamirani%2Frepositories%2Fchat-app%2FChatAppBackend%22%2C%22query%22%3A%22%22%2C%22fragment%22%3A%22%22%7D%5D "/Users/eklavyamirani/repositories/chat-app/ChatAppBackend") directory, such as `appsettings.json` and `appsettings.Development.json`.
    - Need to setup connection string for the postgres sql from the dev container
        ```
        "ConnectionStrings": {
            "ChatContext": "Server=localhost:5432;Database=postgres;username=<username>;password=<password>;"
        },
        ```

#### Frontend

1. **Navigate to the frontend directory**:
    ```sh
    cd chat-app-frontend
    ```

2. **Install dependencies**:
    ```sh
    npm install
    ```

#### Backend

1. **Navigate to the backend directory**:
    ```sh
    cd ChatAppBackend
    ```

2. **Restore .NET dependencies**:
    ```sh
    dotnet restore
    ```

## Running the Project

### Frontend

1. **Start the development server**:
    ```sh
    npm start
    ```

### Backend

1. **Run the backend**:
    ```sh
    dotnet run --project ChatAppBackend/ChatAppBackend.csproj
    ```

## Testing

### Frontend

1. **Run tests**:
    ```sh
    npm test
    ```

### Backend

1. **Run tests**:
    ```sh
    dotnet test
    ```

## License

This project is licensed under the MIT License.