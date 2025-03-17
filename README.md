# ToDoList Application

A web-based task management application built with ASP.NET Core. This application helps you organize tasks in lists, track completion status, and manage your daily activities efficiently.

## Features

- Create and manage multiple task lists
- Add, edit, and delete tasks
- Mark tasks as completed
- Filter tasks by description
- Show/hide completed tasks
- View tasks categorized by completion status
- Responsive UI built with Bootstrap

## Technologies

- ASP.NET Core 9.0+
- Entity Framework Core
- SQLite (Development) / SQL Server (Production)
- Bootstrap 5
- Docker support

## Setup & Running

### Local Development

1. Clone the repository:
   ```bash
   git clone https://github.com/Wojdom/.net-todo-listo
   cd ToDoList
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run --project ToDoList
   ```

4. Access the application at `https://localhost:5001` or `http://localhost:5000`

### Using Docker

1. Build the Docker image:
   ```bash
   docker build -t todolist:latest -f Dockerfile .
   ```

2. Run in development mode with SQLite:
   ```bash
   docker run -d -p 8080:80 \
     -e "ASPNETCORE_ENVIRONMENT=Development" \
     -v todolist-data:/app/Data \
     --name todolist-dev \
     todolist:latest
   ```

3. Run in production mode with SQL Server:
   ```bash
   docker run -d -p 8080:80 \
     -e "ConnectionStrings__TodoDbContext=Server=your_db_server;Database=ToDoList;User=sa;Password=YourPassword;TrustServerCertificate=True;" \
     --name todolist \
     todolist:latest
   ```

4. Access the application at `http://localhost:8080`

## Database Configuration

- Development: Uses SQLite by default (file stored in `/app/Data`)
- Production: Configure SQL Server connection string in environment variables

## Usage Guide

1. **Home Page**: Navigate to the main page to see an overview of your tasks
2. **Task Lists**: Create and manage task lists from the navigation menu
3. **Adding Tasks**: 
   - Click "Add New Task" on the Tasks page
   - Or add tasks directly to a list from the list details page
4. **Completing Tasks**: Click the "Complete" button next to any task
5. **Organizing**: Assign tasks to specific lists using the dropdown when creating/editing

## Development Notes

- Database migrations are automatically applied at startup
- Sample data is seeded on first run in development mode
- The application uses Entity Framework Core for data access
- Controllers follow RESTful patterns for consistency

## License

[MIT License](LICENSE)